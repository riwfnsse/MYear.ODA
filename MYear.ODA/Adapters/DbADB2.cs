#if NET_STD || NET_CORE
using IBM.Data.DB2.Core;
#elif NET_FW
using IBM.Data.DB2;
#endif 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace MYear.ODA.Adapter
{
    public class DbADB2 : DBAccess
    {
        public DbADB2(string ConnectionString)
            : base(ConnectionString)
        {
        }
        public override DbAType DBAType { get { return DbAType.DB2; } }
        public override string[] ObjectFlag
        {
            get { return new string[] { "\"", "\"" }; }
        }

        private DB2Connection _DBConn = null;
        protected override IDbConnection GetConnection()
        {
            if (_DBConn == null)
            {
                _DBConn = new DB2Connection(ConnString);
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
            return new DB2DataAdapter((DB2Command)SelectCmd);
        }
        public override DateTime GetDBDateTime()
        {
            IDbCommand Cmd = OpenCommand();
            try
            {
                Cmd.CommandText = "SELECT TO_CHAR(CURRENT  TIMESTAMP,'YYYY-MM-DD HH24:MI:SS')  DB_DATETIME  FROM SYSIBM.DUAL ";
                Cmd.CommandType = CommandType.Text;
                return Convert.ToDateTime(((DB2Command)Cmd).ExecuteScalar());
            }
            finally
            {
                CloseCommand(Cmd);
            }
        }
        public override string[] GetUserTables()
        {
            string sql = "SELECT T.NAME TABLE_NAME FROM SYSIBM.SYSTABLES T WHERE T.TYPE='T' AND T.TBSPACE='USERSPACE1'";
            DataTable dt_table = Select(sql, null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["TABLE_NAME"].ToString();
            }
            return str;
        }
        public override string[] GetUserViews()
        {
            string sql = "SELECT T.NAME TABLE_NAME FROM SYSIBM.SYSVIEWS T WHERE T.DEFINERTYPE = 'U'";
            DataTable dt_table = Select(sql, null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["TABLE_NAME"].ToString();
            }
            return str;
        }
        public override DataTable GetTableColumns()
        {
            StringBuilder sql = new StringBuilder().Append("SELECT T.NAME TABLE_NAME,C.NAME AS COLUMN_NAME,C.COLNO COL_SEQ,")
                //.Append(" CASE C.COLTYPE WHEN 'CHAR' THEN 'OChar' WHEN 'VARCHAR' THEN 'OVarchar'")
                //.Append(" WHEN 'TIMESTMP' THEN 'ODatetime' WHEN 'DECIMAL' THEN 'ODecimal' WHEN 'INTEGER' THEN 'OInt'")
                //.Append(" WHEN 'BLOB' THEN 'OBinary' ELSE  C.COLTYPE END DATATYPE,")
                .Append(" C.COLTYPE DATATYPE,")
                .Append(" CASE c.nulls WHEN 'N' THEN 'Y' ELSE 'N' END NOT_NULL,C.LENGTH AS LENGTH, C.SCALE, C.REMARKS DIRECTION ")
                .Append(" FROM SYSIBM.SYSCOLUMNS C")
                .Append(" INNER JOIN SYSIBM.SYSTABLES T ON C.TBNAME=T.NAME")
                .Append(" WHERE T.TBSPACE ='USERSPACE1'")
                .Append(" AND T.TYPE='T'")
                .Append(" ORDER BY T.NAME ,C.COLNO");
            DataTable Dt = Select(sql.ToString(), null);
            Dt.TableName = "TABLE_COLUMN";
            return Dt;
        }
        public override DataTable GetViewColumns()
        {
            StringBuilder sql = new StringBuilder().Append("SELECT T.NAME TABLE_NAME,C.NAME AS COLUMN_NAME,C.COLNO COL_SEQ,")
                //.Append(" CASE C.COLTYPE WHEN 'CHAR' THEN 'OChar' WHEN 'VARCHAR' THEN 'OVarchar'")
                //.Append(" WHEN 'TIMESTMP' THEN 'ODatetime' WHEN 'DECIMAL' THEN 'ODecimal' WHEN 'INTEGER' THEN 'OInt'")
                //.Append(" WHEN 'BLOB' THEN 'OBinary' ELSE C.COLTYPE END DATATYPE,")
                .Append(" C.COLTYPE DATATYPE,")
                .Append(" CASE c.nulls WHEN 'N' THEN 'Y' ELSE 'N' END NOT_NULL, C.LENGTH AS LENGTH,C.SCALE, C.REMARKS DIRECTION")
                .Append(" FROM SYSIBM.SYSCOLUMNS C")
                .Append(" INNER JOIN SYSIBM.SYSVIEWS T ON C.TBNAME = T.NAME")
                .Append(" WHERE T.DEFINERTYPE = 'U' ORDER BY T.NAME ,C.COLNO");
            DataTable Dt = this.Select(sql.ToString(), null);
            Dt.TableName = "TABLE_COLUMN";
            return Dt;
        }
        public override string[] GetPrimarykey(string TableName)
        {
            StringBuilder sql = new StringBuilder().Append("SELECT DISTINCT N.TBNAME TABLE_NAME, N.COLNAMES FROM  SYSIBM.SYSTABLES D  ")
                 .Append(" INNER JOIN SYSIBM.SYSINDEXES N ON N.TBNAME = D.NAME")
                 .Append(" WHERE D.TBSPACE = 'USERSPACE1'")
                 .Append(" AND D.TYPE = 'T'")
                 .Append(" AND UNIQUERULE = 'P'")
                 .Append(" AND N.TBNAME = '")
                 .Append( TableName).Append( "'");
            DataTable Dt = this.Select(sql.ToString(), null);
            if (Dt != null && Dt.Rows.Count > 0)
            {
                return Dt.Rows[0]["COLNAMES"].ToString().Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
            }
            return null;
        }


        public override Dictionary<string, string[]> GetPrimarykey()
        {
            StringBuilder sql = new StringBuilder().Append("SELECT DISTINCT N.TBNAME TABLE_NAME, N.COLNAMES FROM  SYSIBM.SYSTABLES D  ")
              .Append(" INNER JOIN SYSIBM.SYSINDEXES N ON N.TBNAME = D.NAME")
              .Append(" WHERE D.TBSPACE = 'USERSPACE1'")
              .Append(" AND D.TYPE = 'T'")
              .Append(" AND UNIQUERULE = 'P'")
              .Append(" ORDER BY N.TBNAME");

            DataTable Dt = this.Select(sql.ToString(), null); 
            Dictionary<string, string[]> pkeys = new Dictionary<string, string[]>();
            if (Dt != null && Dt.Rows.Count > 0)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    string[] pKey = Dt.Rows[i]["COLNAMES"].ToString().Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                    pkeys.Add(Dt.Rows[i]["COLNAMES"].ToString(), pKey);
                }
            }
            return pkeys;
        }
        public override DataTable Select(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord, string Orderby)
        {
            string BlockStr = "select * from (select row_number() over() as r_id_1,t_1.* from ( ";
            BlockStr += SQL;
            BlockStr += ") t_1 ) t_t_1 where t_t_1.r_id_1 > " + StartIndex.ToString() + " and t_t_1.r_id_1  <= " + (StartIndex + MaxRecord).ToString();
            DataTable dt = Select(BlockStr, ParamList);
            dt.Columns.Remove("r_id_1");
            return dt;
        }

        public override List<T> Select<T>(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord, string Orderby)
        {
            IDbCommand Cmd = OpenCommand();
            IDataReader Dr = null; 
            try
            {
                string BlockStr = "select * from (select row_number() over() as r_id_1,t_1.* from ( ";
                BlockStr += SQL;
                BlockStr += ") t_1 ) t_t_1 where t_t_1.r_id_1 > " + StartIndex.ToString() + " and t_t_1.r_id_1  <= " + (StartIndex + MaxRecord).ToString();
                Cmd.CommandType = CommandType.Text;
                SetCmdParameters(ref Cmd, BlockStr, ParamList);
                Dr = Cmd.ExecuteReader();
                var rlt = GetList<T>(Dr, StartIndex, MaxRecord);
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
        public override bool Import(DataTable Data, ODAParameter[] Prms)
        {
            DB2BulkCopy bulkcopy = null;
            IDbConnection conn = null;
            DataTable ImportData = Data.Copy();
            try
            {
                if (this.Transaction == null)
                {
                    conn = this.GetConnection();
                    bulkcopy = new DB2BulkCopy((DB2Connection)conn);
                }
                else
                {
                    bulkcopy = new DB2BulkCopy((DB2Connection)this.Transaction.Connection, DB2BulkCopyOptions.Default);
                }
                for (int i = 0; i < Prms.Length; i++)
                {
                    if (ImportData.Columns.Contains(Prms[i].ParamsName))
                    {
                        DB2BulkCopyColumnMapping colMap = new DB2BulkCopyColumnMapping(Prms[i].ParamsName, Prms[i].ParamsName);
                        bulkcopy.ColumnMappings.Add(colMap);
                    }
                }
                bulkcopy.BulkCopyTimeout = 600000;
                //需要操作的数据库表名  
                bulkcopy.DestinationTableName = ImportData.TableName;
                //将内存表表写入  
                bulkcopy.WriteToServer(ImportData);
                return true;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                if (bulkcopy != null)
                {
                    bulkcopy.Close();
                    bulkcopy = null;
                }
            }
        }
        public override object GetExpressResult(string ExpressionString)
        {
            IDbCommand Cmd = OpenCommand();
            try
            {
                string sql = " SELECT" + ExpressionString + " AS VALUE FROM SYSIBM.SYSDUMMY1";
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
                    DB2Parameter param = new DB2Parameter();
                    param.ParameterName = pr.ParamsName;
                    if (pr.Size < 0)
                        param.Size = 1;
                    else
                        param.Size = pr.Size;
                    param.Direction = pr.Direction;
                    switch (pr.DBDataType)
                    {
                        case ODAdbType.ODatetime:
                            param.DB2Type = DB2Type.Date;
                            if (pr.ParamsValue == null || pr.ParamsValue is DBNull)
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                if (pr.ParamsValue is DateTime || pr.ParamsValue is DateTime?)
                                {
                                    param.Value = pr.ParamsValue;
                                }
                                else if (string.IsNullOrWhiteSpace(pr.ParamsValue.ToString().Trim()))
                                {
                                    param.Value = DBNull.Value;
                                }
                                else
                                {
                                    param.Value = Convert.ToDateTime(pr.ParamsValue);
                                }
                            }
                            break;
                        case ODAdbType.ODecimal:
                            param.DB2Type = DB2Type.Decimal; 
                            if (pr.ParamsValue == null || pr.ParamsValue is DBNull)
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                if (pr.ParamsValue is decimal || pr.ParamsValue is decimal?)
                                {
                                    param.Value = pr.ParamsValue;
                                }
                                else if (string.IsNullOrWhiteSpace(pr.ParamsValue.ToString().Trim()))
                                {
                                    param.Value = DBNull.Value;
                                }
                                else
                                {
                                    param.Value = Convert.ToDecimal(pr.ParamsValue);
                                }
                            }
                            break;
                        case ODAdbType.OBinary:
                            param.DB2Type = DB2Type.Blob;
                            if (pr.ParamsValue == null || pr.ParamsValue is DBNull)
                            {
                                param.Value = DBNull.Value;
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
                            param.DB2Type = DB2Type.Integer;
                            if (pr.ParamsValue == null || pr.ParamsValue is DBNull)
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                if (pr.ParamsValue is int || pr.ParamsValue is int?)
                                {
                                    param.Value = pr.ParamsValue;
                                }
                                else if (string.IsNullOrWhiteSpace(pr.ParamsValue.ToString().Trim()))
                                {
                                    param.Value = DBNull.Value;
                                }
                                else
                                {
                                    param.Value = Convert.ToInt32(pr.ParamsValue);
                                }
                            }
                            break;
                        case ODAdbType.OChar:
                            param.DB2Type = DB2Type.Char;
                            param.DbType = DbType.StringFixedLength;
                            if (pr.ParamsValue == null || pr.ParamsValue is DBNull)
                            {
                                param.Value = DBNull.Value;
                            }
                            else
                            {
                                if (pr.ParamsValue.ToString().Trim() == "")
                                {
                                    param.Value = DBNull.Value;
                                }
                                else
                                {
                                    param.Value = pr.ParamsValue.ToString().Trim();
                                }
                            }
                            break;
                        case ODAdbType.OVarchar:
                            param.DB2Type = DB2Type.VarChar;
                            param.DbType = DbType.String;
                            if (pr.ParamsValue == null || pr.ParamsValue is DBNull)
                            {
                                param.Value = DBNull.Value;
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
                            param.DB2Type = DB2Type.VarChar;
                            param.DbType = DbType.String;
                            param.Value = pr.ParamsValue;
                            break;
                    }
                    ((DB2ParameterCollection)Cmd.Parameters).Add(param);
                }
            }
            Cmd.CommandText = SQL;
            FireExecutingCommand(Cmd);
        }
    }
}

