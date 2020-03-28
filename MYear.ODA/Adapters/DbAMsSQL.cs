using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace MYear.ODA.Adapter
{
    public class DbAMsSQL : DBAccess
    {
        public DbAMsSQL(string ConnectionString)
            : base(ConnectionString)
        {
        }
        public override string[] ObjectFlag
        {
            get { return new string[] { "[", "]" }; }
        }
        private SqlConnection _DBConn = null;
        protected override IDbConnection GetConnection()
        {
            if (_DBConn == null)
            {
                _DBConn = new SqlConnection(ConnString); 
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
            return new SqlDataAdapter((SqlCommand)SelectCmd);
        }
        public override DateTime GetDBDateTime()
        {
            IDbCommand Cmd = OpenCommand();
            try
            {
                string sql = "SELECT getdate() as  DB_DATETIME ";
                Cmd.CommandText = sql;
                Cmd.CommandType = CommandType.Text;
                return Convert.ToDateTime(Cmd.ExecuteScalar());
            }
            finally
            {
                CloseCommand(Cmd);
            }
        }
        public override DbAType DBAType { get { return DbAType.MsSQL; } }
        public override string[] GetUserTables()
        {
            DataTable dt_table = this.Select("SELECT T.NAME TABLE_NAME FROM SYSOBJECTS T WHERE  TYPE='U' ORDER BY TABLE_NAME  ", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["TABLE_NAME"].ToString();
            }
            return str;
        }
        public override string[] GetUserViews()
        {
            DataTable dt_table = this.Select("SELECT V.NAME VIEW_NAME FROM SYSOBJECTS  V WHERE V.TYPE ='V' ORDER BY  VIEW_NAME", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["VIEW_NAME"].ToString();
            }
            return str;
        }
        public override DataTable GetTableColumns()
        {
            StringBuilder sql_tabcol = new StringBuilder().Append( "SELECT DISTINCT SOBJ.NAME AS TABLE_NAME, SCOL.NAME AS COLUMN_NAME , SCOL.COLID AS COL_SEQ,")
            //.Append(" CASE SYT.NAME  WHEN 'sysname'  THEN 'OVarchar' WHEN 'sql_varint'  THEN 'OVarchar'    WHEN 'varchar' THEN 'OVarchar' WHEN 'char'  THEN 'OChar' ")
            //.Append(" WHEN 'nchar'  THEN 'OChar' WHEN 'ntext'  THEN 'OVarchar'  WHEN 'nvarchar'  THEN 'OVarchar'  WHEN 'text'  THEN 'OVarchar' WHEN 'varchar'  THEN 'OVarchar' ")
            //.Append(" WHEN 'bigint'  THEN 'ODecimal'  WHEN 'decimal'  THEN 'ODecimal'   WHEN 'float'  THEN 'ODecimal'   WHEN 'money'  THEN 'ODecimal' ")
            //.Append(" WHEN 'numeric'  THEN 'ODecimal'    WHEN 'real'  THEN 'ODecimal'   WHEN 'smallmoney'  THEN 'ODecimal' ")
            //.Append(" WHEN 'int'  THEN 'OInt'  WHEN 'smallint'  THEN 'OInt'  WHEN 'bit'  THEN 'OInt'  ")
            //.Append(" WHEN 'datetime'  THEN 'ODatetime'  WHEN 'smalldatetime'  THEN 'ODatetime'  WHEN 'smalldatetime'  THEN 'ODatetime'  WHEN 'date'  THEN 'ODatetime' ")
            //.Append(" WHEN 'binary'  THEN 'OBinary'  WHEN 'image'  THEN 'OBinary'  WHEN 'timestamp'  THEN 'OBinary'    WHEN 'varbinary'  THEN 'OBinary' ")
            //.Append(" ELSE  SYT.NAME  END AS DATATYPE ,")
             .Append(" SYT.NAME AS DATATYPE ,")
            .Append(" CASE SCOL.ISNULLABLE WHEN 0 THEN 'Y' ELSE 'N' END AS NOT_NULL,")
            .Append(" SCOL.LENGTH AS LENGTH, SCOL.XSCALE AS SCALE,ext.value AS DIRECTION  ")
            .Append(" FROM SYSOBJECTS SOBJ ")
            .Append(" inner join SYSCOLUMNS SCOL ")
            .Append(" on SOBJ.ID = SCOL.ID  ")
            .Append(" inner join SYS.TYPES SYT ")
            .Append(" on SYT.IS_USER_DEFINED = 0 ")
            .Append(" AND SYT.SYSTEM_TYPE_ID = SCOL.XTYPE ")
            .Append(" left join sys.extended_properties ext ")
            .Append(" on ext.name= 'MS_Description' ")
            .Append(" and ext.major_id =OBJECT_ID (UPPER(SOBJ.NAME)) ")
            .Append(" and ext.minor_id = SCOL.colid ")
            .Append(" WHERE  SOBJ.XTYPE  = 'U '")
            .Append(" AND SYT.NAME <> 'sysname'")
            .Append(" ORDER BY  TABLE_NAME , SCOL.COLID  ");
            DataTable Dt = this.Select(sql_tabcol.ToString(), null);
            Dt.TableName = "TABLE_COLUMN";
            return Dt;
        }
        public override DataTable GetViewColumns()
        {
            StringBuilder sql_view = new StringBuilder().Append("SELECT DISTINCT SOBJ.NAME AS TABLE_NAME, SCOL.NAME AS COLUMN_NAME , SCOL.COLID AS COL_SEQ,  ")
            .Append("CASE SCOL.ISNULLABLE WHEN 0 THEN 'Y' ELSE 'N' END AS NOT_NULL,")
            //.Append(" CASE SYT.NAME  WHEN 'sysname'  THEN 'OVarchar' WHEN 'sql_varint'  THEN 'OVarchar'    WHEN 'varchar' THEN 'OVarchar' WHEN 'char'  THEN 'OChar' ")
            //.Append(" WHEN 'nchar'  THEN 'OChar' WHEN 'ntext'  THEN 'OVarchar'  WHEN 'nvarchar'  THEN 'OVarchar'  WHEN 'text'  THEN 'OVarchar' WHEN 'varchar'  THEN 'OVarchar' ")
            //.Append(" WHEN 'bigint'  THEN 'ODecimal'  WHEN 'decimal'  THEN 'ODecimal'   WHEN 'float'  THEN 'ODecimal'   WHEN 'money'  THEN 'ODecimal' ")
            //.Append(" WHEN 'numeric'  THEN 'ODecimal'    WHEN 'real'  THEN 'ODecimal'   WHEN 'smallmoney'  THEN 'ODecimal' ")
            //.Append(" WHEN 'int'  THEN 'OInt'  WHEN 'smallint'  THEN 'OInt'  WHEN 'bit'  THEN 'OInt'  ")
            //.Append(" WHEN 'datetime'  THEN 'ODatetime'  WHEN 'smalldatetime'  THEN 'ODatetime'  WHEN 'smalldatetime'  THEN 'ODatetime'  WHEN 'date'  THEN 'ODatetime' ")
            //.Append(" WHEN 'binary'  THEN 'OBinary'  WHEN 'image'  THEN 'OBinary'  WHEN 'timestamp'  THEN 'OBinary'    WHEN 'varbinary'  THEN 'OBinary' ")
            //.Append(" ELSE  SYT.NAME  END AS DATATYPE ,")
             .Append("SYT.NAME AS DATATYPE,SCOL.LENGTH AS LENGTH, SCOL.XSCALE AS SCALE, EXT.VALUE AS DIRECTION ") 
            .Append(" FROM SYSOBJECTS SOBJ ")
            .Append(" inner join SYSCOLUMNS SCOL ")
            .Append(" on SOBJ.ID = SCOL.ID  ")
            .Append(" inner join SYS.TYPES SYT ")
            .Append(" on SYT.IS_USER_DEFINED = 0 ")
            .Append(" AND SYT.SYSTEM_TYPE_ID = SCOL.XTYPE ")
            .Append(" left join sys.extended_properties ext ")
            .Append(" on ext.name= 'MS_Description' ")
            .Append(" and ext.major_id = OBJECT_ID (UPPER(SOBJ.NAME)) ")
            .Append(" and ext.minor_id = SCOL.colid ")
            .Append(" WHERE  SOBJ.XTYPE  = 'V '")
            .Append(" AND SYT.NAME <> 'sysname'")
            .Append(" ORDER BY  TABLE_NAME , COLUMN_NAME  ");
            DataTable Dt = this.Select(sql_view.ToString(), null);
            Dt.TableName = "TABLE_COLUMN";
            return Dt;
        }

        public override string[] GetPrimarykey(string TableName)
        {
            string PrimaryCols = new StringBuilder().Append("SELECT DISTINCT  B.COLUMN_NAME ")
            .Append(" FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS A ")
            .Append(" INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE B ")
            .Append(" ON A.CONSTRAINT_NAME = B.CONSTRAINT_NAME ")
            .Append(" WHERE A.CONSTRAINT_TYPE = 'PRIMARY KEY'")
            .Append(" AND A.TABLE_NAME ='").Append(TableName).Append("'").ToString();
            DataTable Dt = this.Select(PrimaryCols, null);
            if (Dt != null && Dt.Rows.Count > 0)
            {
                List<string> cols = new List<string>();
                for (int i = 0; i < Dt.Rows.Count; i++)
                    cols.Add(Dt.Rows[i]["COLUMN_NAME"].ToString());
                return cols.ToArray();
            }
            return null;
        }


        public override Dictionary<string, string[]> GetPrimarykey()
        {
            string PrimaryCols = new StringBuilder().Append("SELECT DISTINCT A.TABLE_NAME, B.COLUMN_NAME ")
                .Append(" FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS A ")
                .Append(" INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE B ")
                .Append(" ON A.CONSTRAINT_NAME = B.CONSTRAINT_NAME ")
                .Append(" WHERE A.CONSTRAINT_TYPE = 'PRIMARY KEY'")
                .Append(" ORDER BY A.TABLE_NAME ").ToString();

            DataTable Dt = this.Select(PrimaryCols, null);

            Dictionary<string, string[]> pkeys = new Dictionary<string, string[]>();
            string tbName = "";
            if (Dt != null && Dt.Rows.Count > 0)
            {
                tbName = Dt.Rows[0]["TABLE_NAME"].ToString();
                List<string> cols = new List<string>();
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    if (tbName != Dt.Rows[i]["TABLE_NAME"].ToString())
                    { 
                        pkeys.Add(tbName, cols.ToArray());
                        cols = new List<string>();
                        tbName = Dt.Rows[i]["TABLE_NAME"].ToString();
                    }
                    cols.Add(Dt.Rows[i]["COLUMN_NAME"].ToString());
                }
            }
            return pkeys;
        }

        public override string[] GetUserProcedure()
        {
            DataTable dt_table = Select("SELECT name as PROCEDURE_NAME FROM sys.objects o WHERE   o.type = 'P'", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["PROCEDURE_NAME"].ToString();
            }
            return str;
        }

       

        public override DataTable Select(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord, string Orderby)
        { 
            int sidx = SQL.IndexOf("SELECT ", 0, StringComparison.InvariantCultureIgnoreCase);
            int distinct = SQL.IndexOf(" DISTINCT ", 0, StringComparison.InvariantCultureIgnoreCase); 
            SQL = SQL.Remove(sidx, "SELECT ".Length); 

            if (string.IsNullOrWhiteSpace(Orderby))
            {
                if ((distinct < 0 || distinct > "SELECT * FROM ".Length))
                {
                    SQL = SQL.Insert(sidx, "SELECT row_number() over(order by GETDATE()) AS R_ID_1, ");
                }
                else
                {
                    distinct = SQL.IndexOf("DISTINCT ", 0, StringComparison.InvariantCultureIgnoreCase);
                    SQL = SQL.Remove(distinct, "DISTINCT ".Length);
                    SQL = SQL.Insert(sidx, "SELECT DISTINCT ROW_NUMBER() OVER(order by GETDATE()) AS R_ID_1, ");
                }               
            }
            else
            {
                SQL = SQL.Replace(Orderby, "");
                if ((distinct < 0 || distinct > "SELECT * FROM ".Length))
                {
                    SQL = SQL.Insert(sidx, "SELECT ROW_NUMBER() OVER(" + Orderby + ") AS R_ID_1, ");
                }
                else
                {
                    // SQL = SQL.Insert(sidx, "SELECT DISTINCT ROW_NUMBER() OVER(" + Orderby + ") AS R_ID_1, ");
                    distinct = SQL.IndexOf("DISTINCT ", 0, StringComparison.InvariantCultureIgnoreCase);
                    SQL = SQL.Remove(distinct, "DISTINCT ".Length);
                    SQL = SQL.Insert(sidx, "SELECT DISTINCT ROW_NUMBER() OVER(" + Orderby + ") AS R_ID_1, ");
                }
            }
            DataTable dt = Select("SELECT A_B_1.* FROM ( " + SQL + " ) AS A_B_1 WHERE A_B_1.R_ID_1 > " + StartIndex.ToString() + " AND A_B_1.R_ID_1 <= " + (StartIndex + MaxRecord).ToString(), ParamList); 
            dt.Columns.Remove("R_ID_1");
            return dt; 
        }
        public override List<T> Select<T>(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord, string Orderby)
        {
            int sidx = SQL.IndexOf("SELECT ", 0, StringComparison.InvariantCultureIgnoreCase);
            int distinct = SQL.IndexOf(" DISTINCT ", 0, StringComparison.InvariantCultureIgnoreCase);
            SQL = SQL.Remove(sidx, "SELECT ".Length);

            if (string.IsNullOrWhiteSpace(Orderby))
            {
                if ((distinct < 0 || distinct > "SELECT * FROM ".Length))
                {
                    SQL = SQL.Insert(sidx, "SELECT row_number() over(order by GETDATE()) AS R_ID_1, ");
                }
                else
                {
                    distinct = SQL.IndexOf("DISTINCT ", 0, StringComparison.InvariantCultureIgnoreCase);
                    SQL = SQL.Remove(distinct, "DISTINCT ".Length);
                    SQL = SQL.Insert(sidx, "SELECT DISTINCT ROW_NUMBER() OVER(order by GETDATE()) AS R_ID_1, ");

                }
            }
            else
            {
                SQL = SQL.Replace(Orderby, "");
                if ((distinct < 0 || distinct > "SELECT * FROM ".Length))
                {
                    SQL = SQL.Insert(sidx, "SELECT ROW_NUMBER() OVER(" + Orderby + ") AS R_ID_1, ");
                }
                else
                {
                    distinct = SQL.IndexOf("DISTINCT ", 0, StringComparison.InvariantCultureIgnoreCase);
                    SQL = SQL.Remove(distinct, "DISTINCT ".Length);
                    SQL = SQL.Insert(sidx, "SELECT DISTINCT ROW_NUMBER() OVER(" + Orderby + ") AS R_ID_1, ");
                }
            }
            return Select<T>("SELECT A_B_1.* FROM ( " + SQL + " ) AS A_B_1 WHERE A_B_1.R_ID_1 > " + StartIndex.ToString() + " AND A_B_1.R_ID_1 <= " + (StartIndex + MaxRecord).ToString(), ParamList);
        }

        public override bool Import(DataTable Data, ODAParameter[] Prms)
        {
            SqlBulkCopy sqlbulkcopy = null;
            IDbConnection conn = null;
            DataTable ImportData = Data.Copy();
            try
            {
                if (this.Transaction == null)
                {
                    conn = this.GetConnection();
                    sqlbulkcopy = new SqlBulkCopy((SqlConnection)conn);
                }
                else
                {
                    sqlbulkcopy = new SqlBulkCopy((SqlConnection)this.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)this.Transaction);
                }

                for (int i = 0; i < Prms.Length; i++)
                {
                    if (ImportData.Columns.Contains(Prms[i].ParamsName))
                    {
                        SqlBulkCopyColumnMapping colMap = new SqlBulkCopyColumnMapping(Prms[i].ParamsName, Prms[i].ParamsName);
                        sqlbulkcopy.ColumnMappings.Add(colMap);
                    }
                }
                sqlbulkcopy.BulkCopyTimeout = 600000;
                //需要操作的数据库表名  
                sqlbulkcopy.DestinationTableName = ImportData.TableName;
                //将内存表表写入  
                sqlbulkcopy.WriteToServer(ImportData);
                return true;
            }
            catch (Exception ex)
            {
                throw new ODAException(202,string.Format("Import data into table [{0}] error:{1}", ImportData.TableName,ex.Message));
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                if (sqlbulkcopy != null)
                {
                    sqlbulkcopy.Close();
                    sqlbulkcopy = null;
                }
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
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = pr.ParamsName;
                    if (pr.Size < 0)
                        param.Size = 1;
                    else
                        param.Size = pr.Size;
                    param.Direction = pr.Direction;
                    switch (pr.DBDataType)
                    {
                        case ODAdbType.ODatetime:
                            param.SqlDbType = SqlDbType.DateTime;
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
                            }
                            break;
                        case ODAdbType.ODecimal:
                            param.SqlDbType = SqlDbType.Decimal;
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
                            param.SqlDbType = SqlDbType.Image;
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
                            param.SqlDbType = SqlDbType.Int;
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
                            param.SqlDbType = SqlDbType.Char;
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
                            param.SqlDbType = SqlDbType.VarChar;
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
                            param.SqlDbType = SqlDbType.VarChar;
                            param.Value = pr.ParamsValue;
                            break;
                    }
                    ((SqlParameterCollection)Cmd.Parameters).Add(param);
                }
            }
            Cmd.CommandText = SQL;
            FireExecutingCommand(Cmd);
        }
    }
}
