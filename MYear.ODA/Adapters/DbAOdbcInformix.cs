using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Text;

namespace MYear.ODA.Adapter
{
    public class DbAOdbcInformix : DBAccess
    {
        private const char DBParamsMark = '?'; 
        public DbAOdbcInformix(string ConnectionString)
            : base(ConnectionString)
        {

        }
        public override char ParamsMark
        {
            get { return DbAOdbcInformix.DBParamsMark; }
        }
        private OdbcConnection _DBConn = null;
        protected override IDbConnection GetConnection()
        {
            if (_DBConn == null)
            {
                _DBConn = new OdbcConnection(ConnString);
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
        protected override DbDataAdapter GetDataAdapter(IDbCommand SelectCmd)
        {
            return new OdbcDataAdapter((OdbcCommand)SelectCmd);
        }
        public override DateTime GetDBDateTime()
        {
            IDbCommand Cmd = OpenCommand();
            try
            {
                string sql = "SELECT TO_CHAR( CURRENT ,'%Y-%m-%d %H:%M:%S')  FROM  SYSTABLES WHERE TABID = 1";
                Cmd.CommandText = sql;
                Cmd.CommandType = CommandType.Text;
                return Convert.ToDateTime(Cmd.ExecuteScalar());
            }
            finally
            {
                CloseCommand(Cmd);
            }
        }
        public override string[] GetUserTables()
        {
            DataTable dt_table = Select("SELECT TABNAME AS TABLE_NAME,TABID FROM SYSTABLES WHERE TABID>99 AND TABTYPE='T' ORDER BY TABNAME ", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["TABLE_NAME"].ToString();
            }
            return str;
        }
        public override string[] GetUserViews()
        {
            DataTable dt_table = Select("SELECT TABNAME AS VIEW_NAME,TABID FROM SYSTABLES WHERE TABID>99 AND TABTYPE='V' ORDER BY TABNAME", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["VIEW_NAME"].ToString();
            }
            return str;
        }
        public override DbAType DBAType { get { return DbAType.OdbcInformix; } }

        public override DataTable GetTableColumns()
        {
            StringBuilder sql_tabcol = new StringBuilder().Append("SELECT T1.TABNAME AS TABLE_NAME,C1.COLNAME AS COLUMN_NAME , ")
            // .Append(" DECODE(C1.COLTYPE,0,'OChar',1,'OInt',2,'OInt',3,'ODecimal',4,'ODecimal',5,'ODecimal',6, ")
            // .Append(" 'OInt',7,'ODatetime',8,'ODecimal',10,'ODatetime',11,'OInt',12,'OVarchar',13,'OVarchar',14, ")
            // .Append(" 'ODecimal',15,'OChar',16,'OVarchar',17,'ODecimal',256,'OChar',257,'OInt',258,'OInt',259,")
            //.Append(" 'ODecimal',260,'ODecimal',261,'ODecimal',162,'OInt',263,'ODatetime',264,'ODecimal',266,'ODatetime',267,")
            //.Append(" 'OInt',268,'OVarchar',269,'OVarchar',270,'ODecimal',271,'OChar',272,'OVarchar',273,'ODecimal') AS DATATYPE,")
            .Append(" 'N' AS NOT_NULL,0 AS COL_SEQ,6 AS SCALE, ")
           .Append(" C1.COLTYPE AS DATATYPE,")
           .Append(" C1.COLLENGTH AS LENGTH , 'INPUT' AS DIRECTION ")
            .Append(" FROM  SYSCOLUMNS  C1,SYSTABLES T1 ")
            .Append(" WHERE C1.TABID=T1.TABID  ")
            .Append(" AND T1.TABTYPE='T' ")
            .Append(" ORDER BY T1.TABNAME,C1.COLNO ");
            DataTable Dt = Select(sql_tabcol.ToString(), null);
            Dt.TableName = "TABLE_COLUMN";
            return Dt;
        }
        public override DataTable GetViewColumns()
        {
            StringBuilder sql_view = new StringBuilder().Append("SELECT T1.TABNAME AS TABLE_NAME,C1.COLNAME AS COLUMN_NAME , ")
            //.Append(" DECODE(C1.COLTYPE,0,'OChar',1,'OInt',2,'OInt',3,'ODecimal',4,'ODecimal',5,'ODecimal',6, ")
            //.Append(" 'OInt',7,'ODatetime',8,'ODecimal',10,'ODatetime',11,'OInt',12,'OVarchar',13,'OVarchar',14, ")
            //.Append(" 'ODecimal',15,'OChar',16,'OVarchar',17,'ODecimal',256,'OChar',257,'OInt',258,'OInt',259,")
            //.Append(" 'ODecimal',260,'ODecimal',261,'ODecimal',162,'OInt',263,'ODatetime',264,'ODecimal',266,'ODatetime',267,")
            //.Append(" 'OInt',268,'OVarchar',269,'OVarchar',270,'ODecimal',271,'OChar',272,'OVarchar',273,'ODecimal') AS DATATYPE,")
            .Append(" 'N' AS NOT_NULL,0 AS COL_SEQ,6 AS SCALE, ")
            .Append(" C1.COLTYPE AS DATATYPE,")
            .Append(" C1.COLLENGTH AS LENGTH , 'INPUT' AS DIRECTION ")
            .Append(" FROM  SYSCOLUMNS  C1,SYSTABLES T1 ")
            .Append(" WHERE C1.TABID=T1.TABID  ")
            .Append(" AND T1.TABTYPE='V' ")
            .Append(" ORDER BY T1.TABNAME,C1.COLNO ");
            DataTable Dt = Select(sql_view.ToString(), null);
            Dt.TableName = "VIEW_COLUMN";
            return Dt;
        }

        public override string[] GetPrimarykey(string TableName)
        {
            return null;
        }

        public override Dictionary<string, string[]> GetPrimarykey()
        {
            return new Dictionary<string, string[]>();
        }

        public override DataTable Select(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord, string Orderby)
        {
            string BlockStr = "SELECT SKIP " + StartIndex.ToString() + " FIRST " + MaxRecord.ToString() + " "; ////取出MaxRecord记录
            BlockStr += SQL.Trim().Substring(6);
            return Select(BlockStr, ParamList);
        }
        public override List<T> Select<T>(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord, string Orderby)
        {
            IDbCommand Cmd = OpenCommand();
            IDataReader Dr = null;
            try
            {
                string BlockStr = BlockStr = "SELECT SKIP " + StartIndex.ToString() + " FIRST " + MaxRecord.ToString() + " "; ////取出MaxRecord记录
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
                    Cmd.Cancel();
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
            string dbSql = SQL;
            if (ParamList != null)
            {
                foreach (ODAParameter pr in ParamList)
                {
                    dbSql = dbSql.Replace(pr.ParamsName, pr.ParamsName.Replace(ODAParameter.ODAParamsMark, DbAOdbcInformix.DBParamsMark)); 
                    OdbcParameter param = new OdbcParameter();
                    param.ParameterName = pr.ParamsName.Replace(ODAParameter.ODAParamsMark, DbAOdbcInformix.DBParamsMark);
                    if (pr.Size < 0)
                        param.Size = 1;
                    else
                        param.Size = pr.Size;
                    param.Direction = pr.Direction;
                    switch (pr.DBDataType)
                    {
                        case ODAdbType.ODatetime:
                            param.OdbcType = OdbcType.DateTime;
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
                            param.OdbcType = OdbcType.Decimal;
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
                            param.OdbcType = OdbcType.Binary;
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
                            param.OdbcType = OdbcType.Int;
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
                            param.OdbcType = OdbcType.Char;
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
                            param.OdbcType = OdbcType.VarChar;
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
                            param.OdbcType = OdbcType.VarChar;
                            param.Value = pr.ParamsValue;
                            break;
                    }
                    ((OdbcParameterCollection)Cmd.Parameters).Add(param);
                }
            }

            Cmd.CommandText = dbSql;
            FireExecutingCommand(Cmd);
        }
    }
}