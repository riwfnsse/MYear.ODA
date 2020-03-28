#if NET_STD || NET_CORE
using AdoNetCore.AseClient;
#elif NET_FW
using Sybase.Data.AseClient;
#endif 

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace MYear.ODA.Adapter
{
    public class DbASybase : DBAccess
    {
        private const char DBParamsMark = ':';
        public DbASybase(string ConnectionString)
            : base(ConnectionString)
        {
        }
        public override char ParamsMark
        {
            get { return DbASybase.DBParamsMark; }
        }
        private AseConnection _DBConn = null;
        protected override IDbConnection GetConnection()
        {
            if (_DBConn == null)
            {
                _DBConn = new AseConnection(ConnString);
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
            return new AseDataAdapter((AseCommand)SelectCmd);
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
        public override string[] GetUserTables()
        {
            DataTable dt_table = this.Select("SELECT T.name TABLE_NAME FROM sysobjects T WHERE  T.type='U' ORDER BY TABLE_NAME", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["TABLE_NAME"].ToString();
            }
            return str;
        }
        public override string[] GetUserViews()
        {
            DataTable dt_table = this.Select("SELECT V.name VIEW_NAME FROM sysobjects  V WHERE V.type ='V' AND V.name <>'sysquerymetrics' ORDER BY  VIEW_NAME", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["VIEW_NAME"].ToString();
            }
            return str;
        }
        public override DbAType DBAType { get { return DbAType.Sybase; } }

        public override DataTable GetTableColumns()
        {
            string sql_tabcol = new StringBuilder().Append(" SELECT SOBJ.name AS TABLE_NAME, SCOL.name AS COLUMN_NAME ,")
            //.Append(" CASE STYPE.name  WHEN 'sysname'  THEN 'OVarchar' WHEN 'sql_varint'  THEN 'OVarchar'    WHEN 'varchar' THEN 'OVarchar' WHEN 'char'  THEN 'OChar' ")
            //.Append(" WHEN 'nchar'  THEN 'OChar' WHEN 'ntext'  THEN 'OVarchar'  WHEN 'nvarchar'  THEN 'OVarchar'  WHEN 'text'  THEN 'OVarchar' WHEN 'varchar'  THEN 'OVarchar' ")
            //.Append(" WHEN 'bigint'  THEN 'ODecimal'  WHEN 'decimal'  THEN 'ODecimal'   WHEN 'float'  THEN 'ODecimal'   WHEN 'money'  THEN 'ODecimal' ")
            //.Append(" WHEN 'numeric'  THEN 'ODecimal'    WHEN 'real'  THEN 'ODecimal'   WHEN 'smallmoney'  THEN 'ODecimal' ")
            //.Append(" WHEN 'int'  THEN 'OInt'  WHEN 'smallint'  THEN 'OInt'    WHEN 'bit'  THEN 'OInt'  ")
            //.Append(" WHEN 'datetime'  THEN 'ODatetime'  WHEN 'smalldatetime'  THEN 'ODatetime'  WHEN 'smalldatetime'  THEN 'ODatetime'  ")
            //.Append(" WHEN 'binary'  THEN 'OBinary'  WHEN 'image'  THEN 'OBinary'  WHEN 'timestamp'  THEN 'OBinary'    WHEN 'varbinary'  THEN 'OBinary' ")
            //.Append(" ELSE  STYPE.name  END AS DATATYPE ,")
            .Append("  CASE STYPE.name AS DATATYPE ,")
            .Append(" SCOL.length AS LENGTH,SCOL.scale AS SCALE, 'INPUT' AS DIRECTION ")
            .Append(" FROM sysobjects SOBJ,syscolumns SCOL,systypes STYPE ")
            .Append(" WHERE  SOBJ.type  = 'U' ")
            .Append(" AND SOBJ.id = SCOL.id")
            .Append(" AND STYPE.usertype = SCOL.usertype")
            .Append(" ORDER BY  TABLE_NAME , COLUMN_NAME ").ToString();
            DataTable Dt = Select(sql_tabcol, null);
            Dt.TableName = "TABLE_COLUMN";
            return Dt;
        }
        public override DataTable GetViewColumns()
        {
            string sql_view = new StringBuilder().Append("SELECT SOBJ.name AS TABLE_NAME, SCOL.name AS COLUMN_NAME ,")
            //.Append(" CASE STYPE.name  WHEN 'sysname'  THEN 'OVarchar' WHEN 'sql_varint'  THEN 'OVarchar'    WHEN 'varchar' THEN 'OVarchar' WHEN 'char'  THEN 'OChar' ")
            //.Append(" WHEN 'nchar'  THEN 'OChar' WHEN 'ntext'  THEN 'OVarchar'  WHEN 'nvarchar'  THEN 'OVarchar'  WHEN 'text'  THEN 'OVarchar' WHEN 'varchar'  THEN 'OVarchar' ")
            //.Append(" WHEN 'bigint'  THEN 'ODecimal'  WHEN 'decimal'  THEN 'ODecimal'   WHEN 'float'  THEN 'ODecimal'   WHEN 'money'  THEN 'ODecimal' ")
            //.Append(" WHEN 'numeric'  THEN 'ODecimal'    WHEN 'real'  THEN 'ODecimal'   WHEN 'smallmoney'  THEN 'ODecimal' ")
            //.Append(" WHEN 'int'  THEN 'OInt'  WHEN 'smallint'  THEN 'OInt'    WHEN 'bit'  THEN 'OInt'  ")
            //.Append(" WHEN 'datetime'  THEN 'ODatetime'  WHEN 'smalldatetime'  THEN 'ODatetime'  WHEN 'smalldatetime'  THEN 'ODatetime'  ")
            //.Append(" WHEN 'binary'  THEN 'OBinary'  WHEN 'image'  THEN 'OBinary'  WHEN 'timestamp'  THEN 'OBinary'    WHEN 'varbinary'  THEN 'OBinary' ")
            //.Append(" ELSE  STYPE.name  END AS DATATYPE ,")

            .Append(" STYPE.name AS DATATYPE ,")
            .Append(" SCOL.length AS LENGTH,SCOL.scale AS SCALE,  'INPUT' AS DIRECTION ")
            .Append(" FROM sysobjects SOBJ,syscolumns SCOL,systypes STYPE ")
            .Append(" WHERE  SOBJ.type  = 'V' ")
            .Append(" AND SOBJ.name <>'sysquerymetrics'")
            .Append(" AND SOBJ.id = SCOL.id")
            .Append(" AND STYPE.usertype = SCOL.usertype")
            .Append(" ORDER BY  TABLE_NAME , COLUMN_NAME ").ToString();
            DataTable Dt = Select(sql_view, null);
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
#if NET_FW
        public override bool Import(DataTable Data,ODAParameter[] Prms)
        {
            DataTable ImportData = Data.Copy();
            IDbCommand Cmd = OpenCommand();
            try
            {
                AseBulkCopy sqlbulkcopy = new AseBulkCopy((AseConnection)Cmd.Connection);
                for (int i = 0; i < Prms.Length; i++)
                {
                    if (ImportData.Columns.Contains(Prms[i].ParamsName))
                    {
                        AseBulkCopyColumnMapping colMap = new AseBulkCopyColumnMapping(ImportData.Columns[i].ColumnName, Prms[i].ParamsName);
                        sqlbulkcopy.ColumnMappings.Add(colMap);
                    }
                }
                sqlbulkcopy.BulkCopyTimeout = 600000;
                //需要操作的数据库表名  
                sqlbulkcopy.DestinationTableName = ImportData.TableName;
                //将内存表表写入  
                sqlbulkcopy.WriteToServer(ImportData);
                sqlbulkcopy.Close();
                return true;
            }
            finally
            {
                CloseCommand(Cmd);
            }
        }
#endif
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
                    dbSql = dbSql.Replace(pr.ParamsName, pr.ParamsName.Replace(ODAParameter.ODAParamsMark, DbASybase.DBParamsMark));
                    AseParameter param = new AseParameter();
                    param.ParameterName = pr.ParamsName.Replace(ODAParameter.ODAParamsMark, DbASybase.DBParamsMark);
                    if (pr.Size < 0)
                        param.Size = 1;
                    else
                        param.Size = pr.Size;
                    param.Direction = pr.Direction;
                    switch (pr.DBDataType)
                    {
                        case ODAdbType.ODatetime:
                            param.AseDbType = AseDbType.DateTime;
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
                            param.AseDbType = AseDbType.Decimal;
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
                            param.AseDbType = AseDbType.Image;
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
                            param.AseDbType = AseDbType.Integer;
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
                            param.AseDbType = AseDbType.UniChar;
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
                            param.AseDbType = AseDbType.UniVarChar;
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
                            param.AseDbType = AseDbType.VarChar;
                            param.Value = pr.ParamsValue;
                            break;
                    }
                    ((AseParameterCollection)Cmd.Parameters).Add(param);
                }
            }
            Cmd.CommandText = dbSql;
            FireExecutingCommand(Cmd);
        }
    }
}
