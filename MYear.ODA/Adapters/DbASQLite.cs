using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;
using System.Text;

namespace MYear.ODA.Adapter
{
    public class DbASQLite : DBAccess
    {
        public DbASQLite(string ConnectionString)
            : base(ConnectionString)
        {

        }

        private SQLiteConnection _DBConn = null;
        protected override IDbConnection GetConnection()
        {
            if (_DBConn == null)
            {
                _DBConn = new SQLiteConnection(ConnString);
                _DBConn.Disposed += _DBConn_Disposed;
            }
            if (_DBConn.State == ConnectionState.Closed)
                _DBConn.Open();
            return _DBConn;
        }

        private void _DBConn_Disposed(object sender, EventArgs e)
        {
            _DBConn = null;
        }

        public static void CreateDataBase(string FileName)
        {
            SQLiteConnection.CreateFile(FileName);
        }
        protected override DbDataAdapter GetDataAdapter(IDbCommand SelectCmd)
        {
            return new SQLiteDataAdapter((SQLiteCommand)SelectCmd);
        }

        public override string[] GetUserTables()
        {
            DataTable dt_table = Select("SELECT Name AS TABLE_NAME  FROM SQLITE_MASTER WHERE TYPE='table' ", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["TABLE_NAME"].ToString().Trim();
            }
            return str;
        }
        public override string[] GetUserViews()
        {
            DataTable dt_table = Select("SELECT Name AS VIEW_NAME  FROM SQLITE_MASTER WHERE TYPE='view' ", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["VIEW_NAME"].ToString().Trim();
            }
            return str;
        }
        public override string[] GetPrimarykey(string TableName)
        {
            SQLiteConnection conn = (SQLiteConnection)this.GetConnection();
            List<string> list = new List<string>();
            using (SQLiteCommand sQLiteCommand2 = new SQLiteCommand(string.Format(CultureInfo.InvariantCulture, "PRAGMA [main].table_info([{0}])", TableName), conn))
            {
                using (SQLiteDataReader sQLiteDataReader2 = sQLiteCommand2.ExecuteReader())
                {
                    while (sQLiteDataReader2.Read())
                    {
                        if (sQLiteDataReader2.GetInt32(5) > 0 )
                        {
                            list.Add(sQLiteDataReader2.GetString(1)); 
                        }
                    }
                }
            }
            return list.ToArray();
        }

        public override Dictionary<string, string[]> GetPrimarykey()
        {
            string[] tables = GetUserTables(); 
            Dictionary<string, string[]> pkeys = new Dictionary<string, string[]>();
            foreach (var t in tables)
            {
                string[] pKeys = GetPrimarykey(t);
                pkeys.Add(t, pKeys);
            }
            return pkeys;
        }

        public override DbAType DBAType { get { return DbAType.SQLite; } }

        public override DataTable Select(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord, string Orderby)
        {
            string BlockStr = new StringBuilder().Append(SQL)
                .Append(" limit ")
                .Append(MaxRecord.ToString())
                .Append(" offset ")
                .Append(StartIndex.ToString()).ToString();
            return Select(BlockStr, ParamList);
        }

        public override List<T> Select<T>(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord, string Orderby)
        {
            IDbCommand Cmd = OpenCommand();
            IDataReader Dr = null;
            try
            {
                string BlockStr = new StringBuilder().Append( SQL )
                    .Append(" limit " )
                    .Append(MaxRecord.ToString() )
                    .Append(" offset ")
                    .Append( StartIndex.ToString()).ToString();
                Cmd.CommandType = CommandType.Text;
                SetCmdParameters(ref Cmd, BlockStr, ParamList);
                Dr = Cmd.ExecuteReader();
                var rlt = GetList<T>(Dr);
             
                return rlt;
            }
            finally
            {
                if (Dr != null) 
                {
                    try
                    {
                        Cmd.Cancel();
                    }
                    catch { }
                    Dr.Close();
                    Dr.Dispose();
                }
                CloseCommand(Cmd);
            }
        }
        
        public override object GetExpressResult(string ExpressionString)
        {
            IDbCommand Cmd = OpenCommand();
            try
            {
                string sql = " SELECT" + ExpressionString + " AS VALUE ";
                Cmd.CommandText = sql;
                Cmd.CommandType = CommandType.Text;
                return Cmd.ExecuteScalar();
            }
            finally
            {
                CloseCommand(Cmd);
            }
        }
        protected override void SetCmdParameters(ref IDbCommand Cmd, string SQL, params ODAParameter[] ParamList)
        {
            if (ParamList != null)
            {
                foreach (ODAParameter pr in ParamList)
                {
                    SQLiteParameter param = new SQLiteParameter();
                    param.ParameterName = pr.ParamsName;
                    if (pr.Size < 0)
                        param.Size = 1;
                    else
                        param.Size = pr.Size;
                    param.Direction = pr.Direction;
                    switch (pr.DBDataType)
                    {
                        case ODAdbType.ODatetime:
                            param.DbType = DbType.DateTime;
                            if (pr.ParamsValue == null || pr.ParamsValue is DBNull)
                            {
                                param.Value = System.DBNull.Value;
                            }
                            else
                            {
                                if (pr.ParamsValue is DateTime || pr.ParamsValue is DateTime?)
                                {
                                    param.Value = pr.ParamsValue;
                                }
                                else if (string.IsNullOrWhiteSpace(pr.ParamsValue.ToString().Trim()))
                                {
                                    param.Value = System.DBNull.Value;
                                }
                                else
                                {
                                    param.Value = Convert.ToDateTime(pr.ParamsValue);
                                }
                            }
                            break;
                        case ODAdbType.ODecimal:
                            param.DbType = DbType.Decimal;
                            if (pr.ParamsValue == null || pr.ParamsValue is DBNull)
                            {
                                param.Value = System.DBNull.Value;
                            }
                            else
                            {
                                if (pr.ParamsValue is decimal || pr.ParamsValue is decimal?)
                                {
                                    param.Value = pr.ParamsValue;
                                }
                                else if (string.IsNullOrWhiteSpace(pr.ParamsValue.ToString().Trim()))
                                {
                                    param.Value = System.DBNull.Value;
                                }
                                else
                                {
                                    param.Value = Convert.ToDecimal(pr.ParamsValue);
                                }
                            }
                            break;
                        case ODAdbType.OBinary:
                            param.DbType = DbType.Binary;
                            if (pr.ParamsValue == null || pr.ParamsValue is DBNull)
                            {
                                param.Value = System.DBNull.Value;
                            }
                            else
                            {
                                param.Value = pr.ParamsValue;
                                if (pr.ParamsValue is byte[])
                                {
                                    param.Size = ((byte[])pr.ParamsValue).Length;
                                }
                                else
                                {
                                    throw new ODAException(201, "Params :" + pr.ParamsName + " Type must be byte[]");
                                }
                            }
                            break;
                        case ODAdbType.OInt:
                            param.DbType = DbType.Int32;
                            if (pr.ParamsValue == null || pr.ParamsValue is DBNull)
                            {
                                param.Value = System.DBNull.Value;
                            }
                            else
                            {
                                if (pr.ParamsValue is int || pr.ParamsValue is int?)
                                {
                                    param.Value = pr.ParamsValue;
                                }
                                else if (string.IsNullOrWhiteSpace(pr.ParamsValue.ToString().Trim()))
                                {
                                    param.Value = System.DBNull.Value;
                                }
                                else
                                {
                                    param.Value = Convert.ToInt32(pr.ParamsValue);
                                }
                            }
                            break;
                        case ODAdbType.OChar:
                            param.DbType = DbType.StringFixedLength;
                            if (pr.ParamsValue == null || pr.ParamsValue is DBNull)
                            {
                                param.Value = System.DBNull.Value;
                            }
                            else
                            {
                                if (pr.ParamsValue.ToString().Trim() == "")
                                {
                                    param.Value = System.DBNull.Value;
                                }
                                else
                                {
                                    param.Value = pr.ParamsValue.ToString().Trim();
                                }
                            }
                            break;
                        case ODAdbType.OVarchar:
                            param.DbType = DbType.String;
                            if (pr.ParamsValue == null || pr.ParamsValue is DBNull)
                            {
                                param.Value = System.DBNull.Value;
                            }
                            else
                            {
                                if (pr.ParamsValue.ToString().Trim() == "")
                                {
                                    param.Value = System.DBNull.Value;
                                }
                                else
                                {
                                    param.Value = pr.ParamsValue.ToString().Trim();
                                }
                            }
                            break;
                        default:
                            param.DbType = DbType.String;
                            param.Value = pr.ParamsValue;
                            break;
                    }
                    ((SQLiteParameterCollection)Cmd.Parameters).Add(param);  
                }  
            }
            Cmd.CommandText = SQL;
            FireExecutingCommand(Cmd);
        }
    }
}

