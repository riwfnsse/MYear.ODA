/*======================================================================================================
 * �ֿ⣺
 * �ֿ���Ϊ�˼��ٵ������ݿ�ѹ�����ֿ�������ݿ�洢�ʹ�����ϵͳ��ģ�����ݵķ�������ҵ���߼��ķָ��ҵ��ֿ⡣
 * �ֿ�ļ������ƣ�
 * 1.���ܿ��ǿ������
 *    �ֿ���Ϊ��������ܣ������ǿ��������ʮ�ֵ��£���:oracle��dblink)
 *    ����ʵ���Ѷ�̫��ֻ�ܴӸ�������������ѯ�㹻����������Ȼ�����ڴ�������ɸѡ���ķ���Դ̫�࣬�ò���ʧ��
 * 2.�ֲ�ʽ�����������Ӧ�÷�������ODA����������������Ӧ�÷����ϲ����п�����񣬿������һ�㶼�����ݿ���濼�ǡ�
 *    Ŀǰ�ķֲ�ʽ���������������ݿ�Ķ��׶��ύ��PreCommit��doCommit�������׶��ύ��CanCommit��PreCommit��doCommit����
 *    ����֮�⻹����ҵ���ϼ�������,�磺
 *    ͬһ�������ڵı��������һ����������������ݣ�ԭ���ݼ������ݣ��ݴ浽������Ա��ع����ύ��CanCommit��PreCommit����
 *    �ύ��ع�ʱ(DoCommit)�ٴ������������ݵ�ҵ���
 *
 * ���ݿ⼯Ⱥ����д�������˵���������ݿ⣩��
 * ���ϵͳ��Ӧ�ٶȵĴ������������Կռ任�ٶȡ���ҵ��͸��������Ч�������ֱ��ݵ�����ά�����Ӷȼ��ɱ���
 *   �����ݿ����������Խ��Խ�ã�Ҳ����Լ���ģ�Լ�����£�ֻ������ֵ�����������������ݿ�Ӳ�����ܶ�һ��) 
 *   ��ⶨһ�����ݿ�һ���ڵ������������reads + 2��writes = 1200,90%��10%д,writes = 2* reads(д��ʱ�Ƕ�������)
 *   ��ӷ��������������reads/9 = writes / (N + 1) ��N �ӷ������������)
 *   
 * �ֱ�
 * �ֱ���Ϊ����ߵ������ܵ���Ӧ�ٶȣ������е����ݴ洢�ṹ�Ż�������
 *  1.ˮƽ�ֱ�(���������ݷֱ�)���������ԭ����ͬ��
 *     ODA�ֱ�ʽ��һ������ͼ��һ��Ϊ�ﻯ��ͼ)�Լ�����������
 *  
 *  2.��ֱ�ֱ����ֶηֱ�)��һ�㲻������ֱ���Ϊ�����ֱ�Գ���Ĳ�ѯ�з����ã����Ӹ����ԡ����Ͳ�ѯ�ٶ�)��
 *     ����һЩ���ֶ��� Blob��Clob�ֶλ����Ȳ��������ݣ����⡢���ߡ����ࡢ������������������ظ�����ͳ����Ϣ)
 *     Ϊ����߱�Ĵ����ٶȺͼ��ٶԴ��ֶβ�����Ϊ�����������ֱ�ָ
 ========================================================================================================*/

using MYear.ODA.Adapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;
using System.Timers;

namespace MYear.ODA
{
    /// <summary>
    /// ���ݿ����������
    /// </summary>
    public class ODAContext
    {
        #region ���ݿ����ӹ���
        public static ODAConfiguration ODAConfig { get; private set; }
        public static void SetODAConfig(DbAType DbType, string ConectionString)
        {
            if (string.IsNullOrWhiteSpace(ConectionString))
                throw new ODAException(30010, "DataBase Conection String should be setted.");
            ODAConfig = new ODAConfiguration()
            {
                Pattern = ODAPattern.Single,
                ODADataBase = new ODAConnect()
                {
                    DBtype = DbType,
                    ConnectionString = ConectionString
                }
            };
        }
        public int CommandTimeOut { get; set; }
        public static void SetODAConfig(ODAConfiguration Config)
        {
            if (Config == null)
                throw new ODAException(30011, "Config can be null.");
            if (Config.Pattern == ODAPattern.Dispersed)
            {
                if (Config.DispersedDataBase == null || Config.DispersedDataBase.Length == 0)
                    throw new ODAException(30012, "Dispersed DataBase can be null.");
                foreach (var Db in Config.DispersedDataBase)
                {
                    if (string.IsNullOrWhiteSpace(Db.DataBaseId))
                        throw new ODAException(30013, "Dispersed DataBase ID should be setted.");
                }
            }
            else if (Config.Pattern == ODAPattern.MasterSlave)
            {
                if (Config.ODADataBase == null || string.IsNullOrWhiteSpace(Config.ODADataBase.ConnectionString))
                    throw new ODAException(30014, "Master DataBase should be setted.");
            }
            else
            {
                if (Config.ODADataBase == null || string.IsNullOrWhiteSpace(Config.ODADataBase.ConnectionString))
                    throw new ODAException(30015, "DataBase should be setted.");
            }
            ODAConfig = Config;
        }

        private static double? _DBTimeDiff = null;
        private IDBAccess NewDBConnect(DbAType dbtype, string Connecting)
        {
            IDBAccess DBA = null;
            switch (dbtype)
            {
                case DbAType.DB2:
                    DBA = new DbADB2(Connecting);
                    break;
                case DbAType.MsSQL:
                    DBA = new DbAMsSQL(Connecting);
                    break;
                case DbAType.MySql:
                    DBA = new DbAMySql(Connecting);
                    break;
#if NET_FW
                case DbAType.OdbcInformix:
                    DBA = new DbAOdbcInformix(Connecting);
                    break;
                case DbAType.OledbAccess:
                    DBA = new DbAOledbAccess(Connecting);
                    break;
#endif
                case DbAType.Oracle:
                    DBA = new DbAOracle(Connecting);
                    break;
                case DbAType.SQLite:
                    DBA = new DbASQLite(Connecting);
                    break;

                case DbAType.Sybase:
                    DBA = new DbASybase(Connecting);
                    break;

            }
            if (this.CommandTimeOut != 0)
                DBA.CommandTimeOut = this.CommandTimeOut;
            return DBA;
        }
        #endregion
        /// <summary>
        /// �½�ODAContext������ODA����ʵ��
        /// </summary>
        private ODAConnect _Conn = null;
        /// <summary>
        ///  ODA����ʵ���������ļ�������
        /// </summary>
        private bool _IsConfig = false;
        public ODAContext()
        {
            if (ODAConfig == null)
                throw new ODAException(30001, "ODAConfig was not setted!");
            _IsConfig = true;
        }
        public ODAContext(DbAType DbType, string ConectionString)
        {
            if (string.IsNullOrWhiteSpace(ConectionString))
                throw new ODAException(30002, "DataBase Conection String should be setted.");
            _Conn = new ODAConnect()
            {
                DBtype = DbType,
                ConnectionString = ConectionString
            };
            _IsConfig = false;
        }
        /// <summary>
        /// ���ݿ⵱ǰ��ʱ��
        /// </summary>
        public DateTime DBDatetime
        {
            get
            {   /////��һ��ȡ���ݿ�ʱ��,�������뱾�ص�ʱ�����,��һ��ȡ����ʱ��
                if (_DBTimeDiff == null)
                {
                    if (!_IsConfig)
                    {
                        if (string.IsNullOrWhiteSpace(_Conn.ConnectionString))
                            _DBTimeDiff = (NewDBConnect(_Conn.DBtype, _Conn.ConnectionString).GetDBDateTime() - DateTime.Now).TotalSeconds;
                    }
                    else if (ODAConfig != null)
                    {
                        if (ODAConfig.Pattern == ODAPattern.Single || ODAConfig.Pattern == ODAPattern.MasterSlave)
                        {
                            if (!string.IsNullOrWhiteSpace(ODAConfig.ODADataBase.ConnectionString))
                            {
                                _DBTimeDiff = (NewDBConnect(ODAConfig.ODADataBase.DBtype, ODAConfig.ODADataBase.ConnectionString).GetDBDateTime() - DateTime.Now).TotalSeconds;
                            }
                        }
                        else if (ODAConfig.DispersedDataBase != null && ODAConfig.DispersedDataBase.Length > 0)
                        {
                            foreach (var db in ODAConfig.DispersedDataBase)
                            {
                                if (!string.IsNullOrWhiteSpace(db.ConnectionString))
                                {
                                    _DBTimeDiff = (NewDBConnect(db.DBtype, db.ConnectionString).GetDBDateTime() - DateTime.Now).TotalSeconds;
                                }
                                break;
                            }
                        }
                    }
                    if (_DBTimeDiff == null)
                        _DBTimeDiff = 0;
                }
                return DateTime.Now.AddSeconds(_DBTimeDiff.Value);
            }
        }

        /// <summary>
        /// ��ȡ��ǰ���ݿ���������ĵķ�������ʵ��
        /// </summary>
        /// <typeparam name="U">��������</typeparam>
        /// <param name="Schema">����</param>
        /// <returns></returns>
        public virtual U GetCmd<U>(string Schema = "") where U : IODACmd, new()
        {
            U cmd = new U();
            cmd.GetDBAccess = GetDBAccess;
            if (!string.IsNullOrWhiteSpace(Schema))
                cmd.Schema = Schema;
            cmd.Alias = this.GetAlias();
            cmd.GetAlias = this.GetAlias;
            cmd.DBCharSet = Encoding.UTF8;
            return cmd;
        }

        #region �������
        private ODATransaction _Tran = null;
        /// <summary>
        /// ָʾ�Ƿ�������������
        /// </summary>
        public bool IsTransactionBegined { get { return _Tran != null; } }
        /// <summary>
        /// ��������Ĭ��30�볬ʱ
        /// </summary>
        public virtual void BeginTransaction()
        {
            BeginTransaction(30);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="TimeOut">����ʱʱ����С�ڻ����0ʱ���񲻻ᳬʱ,��λ:��</param>
        /// <returns></returns>
        public virtual void BeginTransaction(int TimeOut)
        {
            if (_Tran != null)
                throw new ODAException(30021, "Transaction had begun!");
            _Tran = new ODATransaction(TimeOut);
            _Tran.TransactionTimeOut = this.RollBack;
            var Sql = new ODAScript()
            {
                ScriptType = SQLType.BeginTransation,
            };
            Sql.SqlScript.Append("Begin Tran:" + _Tran.TransactionId);
            FireODASqlEvent(new ExecuteEventArgs()
            {
                SqlParams = Sql,
            });
        }
        /// <summary>
        /// �ύ����
        /// </summary>
        public void Commit()
        {
            if (_Tran != null)
            {
                var Sql = new ODAScript()
                {
                    ScriptType = SQLType.Commit,
                };
                Sql.SqlScript.Append("Commit Tran:" + _Tran.TransactionId);
                FireODASqlEvent(new ExecuteEventArgs()
                {
                    SqlParams = Sql,
                });
                _Tran.Commit();
                _Tran = null;
            }
        }
        /// <summary>
        /// �ع�����
        /// </summary>
        public void RollBack()
        {
            if (_Tran != null)
            {
                _Tran.RollBack();
                var Sql = new ODAScript()
                {
                    ScriptType = SQLType.Rollback,
                };
                Sql.SqlScript.Append("Rollback Tran:" + _Tran.TransactionId);
                FireODASqlEvent(new ExecuteEventArgs()
                {
                    SqlParams = Sql,
                });
                _Tran = null;
            }
        }
        /// <summary>
        /// ��ֹ�����������������˳��
        /// </summary>
        /// <param name="Cmd"></param>
        protected void CheckTransaction(ODAScript Cmd)
        {
            if (ODAConfig != null && ODAConfig.RegularObject != null && ODAConfig.RegularObject.Length > 0)
            {
                if (_Tran == null || _Tran.CurrentObject == null)
                    return;
                int MaxSeq = 0;
                for (int i = 0; i < ODAConfig.RegularObject.Length; i++)
                {
                    if (_Tran.CurrentObject == ODAConfig.RegularObject[i])
                        MaxSeq = i;
                }
                if (Cmd.TableList != null && Cmd.TableList.Count > 0)
                {
                    for (int i = 0; i < ODAConfig.RegularObject.Length; i++)
                    {
                        if (Cmd.TableList[0] == ODAConfig.RegularObject[i])
                        {
                            if (i >= MaxSeq)
                            {
                                _Tran.CurrentObject = Cmd.TableList[0];
                                return;
                            }
                            throw new ODAException(30022, string.Format("Access ojbect [{0}] Not orderly", Cmd.TableList[0]));
                        }
                    }
                }
            }
        }
        #endregion

        #region ������·���㷨 
        private ODAConnect GetConnect(ODAScript ODASql)
        {
            if (!_IsConfig)
            {
                return _Conn;
            }
            else
            {
                switch (ODAConfig.Pattern)
                {
                    case ODAPattern.Single:
                        return ODAConfig.ODADataBase;
                    case ODAPattern.MasterSlave:
                        if (ODASql.ScriptType == SQLType.Select && _Tran == null)
                        {
                            if (ODAConfig.DispersedDataBase != null && ODAConfig.DispersedDataBase.Length > 0)
                            {
                                int curDate = int.Parse(System.DateTime.Now.ToString("ssfffff"));
                                int curDt = curDate % ODAConfig.DispersedDataBase.Length;
                                return ODAConfig.DispersedDataBase[curDt];
                            }
                        }
                        ///û���ҵ������ݿ��򷵻������ݿ�
                        return ODAConfig.ODADataBase;
                    case ODAPattern.Dispersed:
                        if (string.IsNullOrWhiteSpace(ODASql.DataBaseId))
                        {
                            return ODAConfig.ODADataBase;
                        }
                        else
                        {
                            foreach (var db in ODAConfig.DispersedDataBase)
                            {
                                if (ODASql.DataBaseId == db.DataBaseId)
                                    return db;
                            }
                        }
                        throw new ODAException(30031, string.Format("Can not rout to Dispersed DataBase[{0}]", ODASql.DataBaseId));
                    default:
                        throw new ODAException(30032, string.Format("Can not rout to Dispersed DataBase[{0}]", "Impossible Exception"));
                }
            }
        }

        /// <summary>
        /// �ֿ�·����,���������������,���ص�DB��������������
        /// ���ݿ����Ӽ�Ⱥ
        /// </summary>
        /// <param name="ODASql"></param> 
        /// <returns></returns>
        private IDBAccess DatabaseRouting(ODAScript ODASql)
        {
            ODAConnect conn = GetConnect(ODASql);
            if (_Tran == null)
            {
                return NewDBConnect(conn.DBtype, conn.ConnectionString);
            }
            else
            {
                this.CheckTransaction(ODASql);
                if (_Tran.TransDB.ContainsKey(conn.ConnectionString))
                {
                    return _Tran.TransDB[conn.ConnectionString];
                }
                else
                {
                    IDBAccess DBA = NewDBConnect(conn.DBtype, conn.ConnectionString);
                    DBA.BeginTransaction();
                    _Tran.DoCommit += DBA.Commit;
                    _Tran.RollBacking += DBA.RollBack;
                    _Tran.TransDB.Add(conn.ConnectionString, DBA);
                    return DBA;
                }
            }
        }

        #endregion
        #region SQL���ִ�С�������չ��ʹ����Ϣ����ʵ�ֶ�����ʵʱͬ����
        private static event ODASqlEventHandler _CurrentExecutingODASql;
        public static event ODASqlEventHandler CurrentExecutingODASql
        {
            add
            {
                if (_CurrentExecutingODASql != null)
                {
                    Delegate[] dls = _CurrentExecutingODASql.GetInvocationList();
                    foreach (Delegate dl in dls)
                        if (dl.Method == value.Method)
                            return;
                }
                _CurrentExecutingODASql += value;
            }
            remove
            {
                _CurrentExecutingODASql -= value;
            }
        }
        private static event Action<string, object[]> _CurrentExecutingSql;
        public static event Action<string, object[]> CurrentExecutingSql
        {
            add
            {
                if (_CurrentExecutingSql != null)
                {
                    Delegate[] dls = _CurrentExecutingODASql.GetInvocationList();
                    foreach (Delegate dl in dls)
                        if (dl.Method == value.Method)
                            return;
                }
                _CurrentExecutingSql += value;
            }
            remove
            {
                _CurrentExecutingSql -= value;
            }
        }

        public static ODAScript LastODASQL { get; private set; }
        public string LastSQL { get; private set; }
        public object[] SQLParams { get; private set; }
        private void FireODASqlEvent(ExecuteEventArgs args)
        {
            LastODASQL = args.SqlParams;
            _CurrentExecutingODASql?.Invoke(this, args);
        }

        private void ExecutingCommand(IDbCommand AdoCmd)
        {
            LastSQL = AdoCmd.CommandText;
            object[] pms = null;
            if (AdoCmd.Parameters != null)
            {
                pms = new object[AdoCmd.Parameters.Count];
                try
                {
                    AdoCmd.Parameters.CopyTo(pms, 0);
                }
                catch
                {
                    for (int i = 0; i < AdoCmd.Parameters.Count; i++)
                    {
                        pms[i] = AdoCmd.Parameters[i];
                    }
                }
                SQLParams = pms;
            }
            _CurrentExecutingSql?.Invoke(LastSQL, pms);

        }

        public string DebugSQL
        {
            get
            {
                return GetDebugSql(LastSQL, SQLParams);
            }
        }
        private string GetDebugSql(string Sql, params object[] prms)
        {
            string debugSql = Sql;
            if (prms != null)
            {
                foreach (object op in prms)
                {
                    DbParameter p = op as DbParameter;
                    if (p.Value != null)
                    {
                        string ParamsValue = p.Value.ToString();
                        string ParamsName = p.ParameterName;
                        switch (p.DbType)
                        {
                            case DbType.Byte:
                            case DbType.Currency:
                            case DbType.Decimal:
                            case DbType.Double:
                            case DbType.Int16:
                            case DbType.Int32:
                            case DbType.Int64:
                            case DbType.UInt16:
                            case DbType.UInt32:
                            case DbType.UInt64:
                            case DbType.VarNumeric:
                            case DbType.SByte:
                            case DbType.Single:
                                break;

                            case DbType.Time:
                            case DbType.DateTime2:
                            case DbType.DateTimeOffset:
                            case DbType.Date:
                            case DbType.DateTime:
                            case DbType.String:
                            case DbType.AnsiStringFixedLength:
                            case DbType.StringFixedLength:
                            case DbType.AnsiString:
                            case DbType.Xml:
                            case DbType.Guid:
                            case DbType.Binary:
                            case DbType.Boolean:
                            case DbType.Object:
                                ParamsValue = "'" + ParamsValue + "'";
                                break;
                        }
                        System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("(" + ParamsName + "\\b|\\t)");
                        debugSql = rgx.Replace(debugSql, ParamsValue);
                    }
                }
            }
            return debugSql;
        }

        protected virtual IDBAccess GetDBAccess(ODAScript ODASql)
        {
            IDBAccess DBA = DatabaseRouting(ODASql);
            if (DBA.ExecutingCommand == null)
                DBA.ExecutingCommand = ExecutingCommand;
            ExecuteEventArgs arg = new ExecuteEventArgs()
            {
                DBA = DBA,
                SqlParams = ODASql,
            };
            FireODASqlEvent(arg);
            return arg.DBA;
        }
        private int _Alias = 0;
        protected virtual string GetAlias()
        {
            string Alias = string.Format("T{0}", _Alias);
            _Alias++;
            return Alias;
        }
        #endregion
    }
}