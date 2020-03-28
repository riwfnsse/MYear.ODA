/*======================================================================================================
 * 分库：
 * 分库是为了减少单个数据库压力而分开多个数据库存储和处理多个系统或模块数据的方法，是业务逻辑的分割，即业务分库。
 * 分库的技术限制：
 * 1.不能跨库强关连。
 *    分库是为了提高性能，但跨库强关连性能十分低下（例:oracle的dblink)
 *    技术实现难度太大，只能从各个库中作数查询足够完整的数据然后在内存中连接筛选，耗费资源太多，得不偿失。
 * 2.分布式事务很难做，应用服务器（ODA）做不到，所以在应用服务上不能有跨库事务，跨库事务一般都是数据库层面考虑。
 *    目前的分布式事务解决方案是数据库的二阶段提交（PreCommit、doCommit）或三阶段提交（CanCommit、PreCommit、doCommit）。
 *    除此之外还可以业务结合技术考虑,如：
 *    同一个事务内的表各自增加一个事务表，把事务数据（原数据及新数据）暂存到事务表，以备回滚和提交（CanCommit、PreCommit），
 *    提交或回滚时(DoCommit)再从事务表更新数据到业务表。
 *
 * 数据库集群（读写分离或者说是主从数据库）：
 * 提高系统响应速度的纯技术方案，以空间换速度。对业务透明，付带效果是容灾备份但增加维护复杂度及成本。
 *   从数据库的数量不是越多越好，也是有约束的，约束如下（只是理论值，不考虑网络且数据库硬用性能都一样) 
 *   如测定一个数据库一秒内的最大吞吐量：reads + 2×writes = 1200,90%读10%写,writes = 2* reads(写耗时是读的两倍)
 *   则从服务器最大数量：reads/9 = writes / (N + 1) （N 从服务器最大数量)
 *   
 * 分表：
 * 分表是为了提高单个功能的响应速度，而进行的数据存储结构优化方案。
 *  1.水平分表(按数据内容分表)，与分区表原理相同；
 *     ODA分表方式：一个主视图（一般为物化视图)以及多个子物理表。
 *  
 *  2.垂直分表（按字段分表)，一般不作此类分表，因为这样分表对常规的查询有反作用（增加复杂性、降低查询速度)；
 *     但有一些大字段如 Blob、Clob字段或冷热不均的数据（标题、作者、分类、文章内容与浏览量、回复数等统计信息)
 *     为了提高表的处理速度和减少对大字段操作或为了做缓存而垂直分割。
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
    /// 数据库访问上下文
    /// </summary>
    public class ODAContext
    {
        #region 数据库连接管理
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
        private static IDBAccess NewDBConnect(DbAType dbtype, string Connecting)
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
                case DbAType.OdbcInformix:
                    DBA = new DbAOdbcInformix(Connecting);
                    break;
#if FW
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
#if FW
                case DbAType.Sybase:
                    DBA = new DbASybase(Connecting);
                    break;
#endif
            }
            return DBA;
        }
        #endregion
        /// <summary>
        /// 新建ODAContext产生的ODA连接实例
        /// </summary>
        private ODAConnect _Conn = null;
        /// <summary>
        ///  ODA连接实例由配置文件中生产
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
        /// 数据库当前的时间
        /// </summary>
        public DateTime DBDatetime
        {
            get
            {   /////第一次取数据库时间,并保存与本地的时间差异,下一次取本地时间
                if (_DBTimeDiff == null)
                {
                    if (!_IsConfig)
                    {
                        if (string.IsNullOrWhiteSpace(_Conn.ConnectionString))
                            _DBTimeDiff = (ODAContext.NewDBConnect(_Conn.DBtype, _Conn.ConnectionString).GetDBDateTime() - DateTime.Now).TotalSeconds;
                    }
                    else if (ODAConfig != null)
                    {
                        if (ODAConfig.Pattern == ODAPattern.Single || ODAConfig.Pattern == ODAPattern.MasterSlave)
                        {
                            if (!string.IsNullOrWhiteSpace(ODAConfig.ODADataBase.ConnectionString))
                            {
                                _DBTimeDiff = (ODAContext.NewDBConnect(ODAConfig.ODADataBase.DBtype, ODAConfig.ODADataBase.ConnectionString).GetDBDateTime() - DateTime.Now).TotalSeconds;
                            }
                        }
                        else if (ODAConfig.DispersedDataBase != null && ODAConfig.DispersedDataBase.Length > 0)
                        {
                            foreach (var db in ODAConfig.DispersedDataBase)
                            {
                                if (!string.IsNullOrWhiteSpace(db.ConnectionString))
                                {
                                    _DBTimeDiff = (ODAContext.NewDBConnect(db.DBtype, db.ConnectionString).GetDBDateTime() - DateTime.Now).TotalSeconds;
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
        /// 获取当前数据库访问上下文的访问命令实列
        /// </summary>
        /// <typeparam name="U">命令类型</typeparam>
        /// <param name="Schema">别名</param>
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

        #region 事务管理
        private ODATransaction _Tran = null;
        /// <summary>
        /// 指示是否已启动了事务
        /// </summary>
        public bool IsTransactionBegined { get { return _Tran != null; } }
        /// <summary>
        /// 开启事务，默认30秒超时
        /// </summary>
        public virtual void BeginTransaction()
        {
            BeginTransaction(30);
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="TimeOut">事务超时时长，小于或等于0时事务不会超时,单位:秒</param>
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
        /// 提交事务
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
        /// 回滚事务
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
        /// 防止死锁，检查事务对象的顺序
        /// </summary>
        /// <param name="Cmd"></param>
        protected void CheckTransaction(ODAScript Cmd)
        {
            if (ODAConfig.RegularObject != null && ODAConfig.RegularObject.Length > 0)
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

        #region 库数据路由算法 
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
                        ///没有找到从数据库则返回主数据库
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
        /// 分库路由器,如果操作存在事务,返回的DB连接已在事务中
        /// 数据库主从集群
        /// </summary>
        /// <param name="ODASql"></param> 
        /// <returns></returns>
        private IDBAccess DatabaseRouting(ODAScript ODASql)
        {
            ODAConnect conn = GetConnect(ODASql);
            if (_Tran == null)
            {
                return ODAContext.NewDBConnect(conn.DBtype, conn.ConnectionString);
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
                    IDBAccess DBA = ODAContext.NewDBConnect(conn.DBtype, conn.ConnectionString);
                    DBA.BeginTransaction();
                    _Tran.DoCommit += DBA.Commit;
                    _Tran.RollBacking += DBA.RollBack;
                    _Tran.TransDB.Add(conn.ConnectionString, DBA);
                    return DBA;
                }
            }
        }

        #endregion
        #region SQL语句执行。（待扩展：使用消息队列实现多数据实时同步）
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
                foreach (object  op in prms)
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