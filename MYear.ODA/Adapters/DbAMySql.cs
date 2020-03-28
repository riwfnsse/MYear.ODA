using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
namespace MYear.ODA.Adapter
{
    public class DbAMySql : DBAccess
    {
        static DbAMySql()
        {
            ODAReflection.DBTypeMapping.Add(typeof(MySqlDateTime), typeof(DateTime));
            ODAReflection.DBTypeMapping.Add(typeof(MySqlDecimal), typeof(decimal));
        }

        public override string[] ObjectFlag
        {
            get { return new string[] { "`", "`" }; }
        }
        public DbAMySql(string ConnectionString)
            : base(ConnectionString)
        {
        }
        private MySqlConnection _DBConn = null;
        protected override IDbConnection GetConnection()
        {
            if (_DBConn == null)
            {
                _DBConn = new MySqlConnection(ConnString);
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
            return new MySqlDataAdapter((MySqlCommand)SelectCmd);
        }
        public override DateTime GetDBDateTime()
        {
            IDbCommand Cmd = OpenCommand();
            try
            {
                string sql = "SELECT sysdate() AS DB_DATETIME ";
                Cmd.CommandText = sql;
                Cmd.CommandType = CommandType.Text;
                return Convert.ToDateTime(Cmd.ExecuteScalar());
            }
            finally
            {
                CloseCommand(Cmd);
            }
        }
        public override DbAType DBAType { get { return DbAType.MySql; } }
        public override string[] GetUserTables()
        {
            DataTable dt_table = Select(new StringBuilder().Append("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ")
                .Append( " WHERE TABLE_NAME NOT IN (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS WHERE  TABLE_SCHEMA =DATABASE()) ")
                .Append(" AND TABLE_SCHEMA =DATABASE() ").ToString(), null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["TABLE_NAME"].ToString();
            }
            return str;
        }
        public override string[] GetUserViews()
        {
            DataTable dt_table = Select("SELECT TABLE_NAME VIEW_NAME FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA =DATABASE()", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["VIEW_NAME"].ToString();
            }
            return str;
        }

        public override DataTable GetTableColumns()
        {
            StringBuilder sql_tabcol =new StringBuilder().Append( "SELECT C.TABLE_NAME,C.COLUMN_NAME, C.ORDINAL_POSITION COL_SEQ, ")
            //.Append(" CASE C.DATA_TYPE WHEN 'char' THEN 'OChar' WHEN 'varchar' THEN 'OVarchar' WHEN 'tinytext' THEN 'OVarchar' ")
            //.Append(" WHEN 'text' THEN 'OVarchar' WHEN 'mediumtext' THEN 'OVarchar' WHEN 'longtext' THEN 'OVarchar' WHEN 'enum' THEN 'OChar' ")
            //.Append(" WHEN 'set' THEN 'OVarchar' WHEN 'geometry' THEN 'OVarchar' WHEN 'point' THEN 'OVarchar' WHEN 'linestring' THEN 'OVarchar' ")
            //.Append(" WHEN 'polygon' THEN 'OVarchar' WHEN 'multipoint' THEN 'OVarchar' WHEN 'multilinestring' THEN 'OVarchar'  ")
            //.Append(" WHEN 'multipolygon' THEN 'OVarchar' WHEN 'geometrycollection' THEN 'OVarchar' ")
            //.Append(" WHEN 'int' THEN 'OInt' WHEN 'tynyint' THEN 'OInt' WHEN 'smallint' THEN 'OInt' WHEN 'mediumint' THEN 'OInt'")
            //.Append(" WHEN 'bigint' THEN 'ODecimal' WHEN 'real' THEN 'ODecimal'  WHEN 'double' THEN 'ODecimal' WHEN 'float' THEN 'ODecimal' ")
            //.Append(" WHEN 'numeric' THEN 'ODecimal' WHEN 'decimal' THEN 'ODecimal' ")
            //.Append(" WHEN 'binary' THEN 'OBinary' WHEN 'varbinary' THEN 'OBinary' WHEN 'blob' THEN 'OBinary' WHEN 'mediumblob' THEN 'OBinary' WHEN 'longblob' THEN 'OBinary' ")
            //.Append(" WHEN 'date' THEN 'ODatetime' WHEN 'year' THEN 'ODatetime' WHEN 'time' THEN 'ODatetime' WHEN 'timestamp' THEN 'ODatetime' ")
            //.Append(" WHEN 'datetime' THEN 'ODatetime' END AS DATATYPE ,")
            .Append(" C.DATA_TYPE  AS DATATYPE ,")
            .Append(" CASE C.IS_NULLABLE WHEN 'NO' THEN 'Y' ELSE 'N' END AS NOT_NULL, ")
            .Append(" CASE WHEN C.CHARACTER_MAXIMUM_LENGTH IS NULL THEN 0 ELSE  CASE WHEN  C.CHARACTER_MAXIMUM_LENGTH > 65534 THEN 0 ELSE  C.CHARACTER_MAXIMUM_LENGTH END END LENGTH,")
            .Append(" CASE WHEN C.NUMERIC_SCALE IS NULL THEN 0 ELSE C.NUMERIC_SCALE  END SCALE,'INPUT' DIRECTION  ")
            .Append(" FROM INFORMATION_SCHEMA.COLUMNS C ")
            .Append(" WHERE C.TABLE_NAME NOT IN (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS V WHERE V.TABLE_SCHEMA  = DATABASE() AND  C.TABLE_SCHEMA =DATABASE()) ")
            .Append(" AND C.TABLE_SCHEMA =DATABASE() ")
            .Append(" ORDER BY C.TABLE_NAME, C.ORDINAL_POSITION ");
            DataTable Dt = Select(sql_tabcol.ToString(), null);
            Dt.TableName = "TABLE_COLUMN";
            return Dt;
        }
        public override DataTable GetViewColumns()
        {
            
              StringBuilder sql_view = new StringBuilder().Append("C.TABLE_NAME,C.COLUMN_NAME, C.ORDINAL_POSITION COL_SEQ,")
            //.Append("  CASE C.DATA_TYPE WHEN 'char' THEN 'OChar' WHEN 'varchar' THEN 'OVarchar' WHEN 'tinytext' THEN 'OVarchar' ")
            //.Append(" WHEN 'text' THEN 'OVarchar' WHEN 'mediumtext' THEN 'OVarchar' WHEN 'longtext' THEN 'OVarchar' WHEN 'enum' THEN 'OChar' ")
            //.Append(" WHEN 'set' THEN 'OVarchar' WHEN 'geometry' THEN 'OVarchar' WHEN 'point' THEN 'OVarchar' WHEN 'linestring' THEN 'OVarchar' ")
            //.Append(" WHEN 'polygon' THEN 'OVarchar' WHEN 'multipoint' THEN 'OVarchar' WHEN 'multilinestring' THEN 'OVarchar'  ")
            //.Append(" WHEN 'multipolygon' THEN 'OVarchar' WHEN 'geometrycollection' THEN 'OVarchar' ")
            //.Append(" WHEN 'int' THEN 'OInt' WHEN 'tynyint' THEN 'OInt' WHEN 'smallint' THEN 'OInt' WHEN 'mediumint' THEN 'OInt' ")
            //.Append(" WHEN 'bigint' THEN 'ODecimal' WHEN 'real' THEN 'ODecimal'  WHEN 'double' THEN 'ODecimal' WHEN 'float' THEN 'ODecimal' ")
            //.Append(" WHEN 'numeric' THEN 'ODecimal' WHEN 'decimal' THEN 'ODecimal' ")
            //.Append(" WHEN 'binary' THEN 'OBinary' WHEN 'varbinary' THEN 'OBinary' WHEN 'blob' THEN 'OBinary' WHEN 'mediumblob' THEN 'OBinary' WHEN 'longblob' THEN 'OBinary' ")
            //.Append(" WHEN 'date' THEN 'ODatetime' WHEN 'year' THEN 'ODatetime' WHEN 'time' THEN 'ODatetime' WHEN 'timestamp' THEN 'ODatetime' ")
            //.Append(" WHEN 'datetime' THEN 'ODatetime' END AS DATATYPE ,")
            .Append(" C.DATA_TYPE  AS DATATYPE ,")
            .Append(" CASE C.IS_NULLABLE WHEN 'NO' THEN 'Y' ELSE 'N' END AS NOT_NULL, ")
            .Append(" CASE WHEN C.CHARACTER_MAXIMUM_LENGTH IS NULL THEN 0 ELSE  CASE WHEN  C.CHARACTER_MAXIMUM_LENGTH > 65534 THEN 0 ELSE  C.CHARACTER_MAXIMUM_LENGTH END  END LENGTH,")
            .Append(" CASE WHEN C.NUMERIC_SCALE IS NULL THEN 0 ELSE C.NUMERIC_SCALE  END SCALE ,'INPUT' DIRECTION  ")
            .Append(" FROM INFORMATION_SCHEMA.COLUMNS C, INFORMATION_SCHEMA.VIEWS V ")
            .Append(" WHERE C.TABLE_NAME = V.TABLE_NAME ")
            .Append(" AND C.TABLE_SCHEMA =DATABASE() ")
            .Append(" AND V.TABLE_SCHEMA  = DATABASE() ")
            .Append(" ORDER BY C.TABLE_NAME,C.ORDINAL_POSITION ");
            DataTable Dt = Select(sql_view.ToString(), null);
            Dt.TableName = "VIEW_COLUMN";
            return Dt;
        }

        public override string[] GetPrimarykey(string TableName)
        {
            var db = this.GetConnection();
            string PrimaryCols = new StringBuilder().Append("SELECT DISTINCT  CU.TABLE_NAME,CU.COLUMN_NAME ")
                .Append(" FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU,INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC ")
            .Append(" WHERE  CU.TABLE_NAME = TC.TABLE_NAME ")
            .Append(" AND CU.TABLE_SCHEMA = TC.TABLE_SCHEMA ")
            .Append(" AND TC.TABLE_SCHEMA = '" + db.Database + "'")
            .Append(" AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY' ")
            .Append(" AND CU.TABLE_NAME ='").Append(TableName).Append("'").ToString();

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
            var db = this.GetConnection();
            string PrimaryCols = new StringBuilder().Append("SELECT DISTINCT  CU.TABLE_NAME,CU.COLUMN_NAME ")
                .Append(" FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU,INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC ")
            .Append(" WHERE  CU.TABLE_NAME = TC.TABLE_NAME ")
            .Append(" AND CU.TABLE_SCHEMA = TC.TABLE_SCHEMA ")
            .Append(" AND TC.TABLE_SCHEMA = '" + db.Database + "'")
            .Append(" AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY' ")
            .Append(" ORDER BY CU.TABLE_NAME").ToString();
             
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

        public override DataTable Select(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord, string Orderby)
        {
            string BlockStr = SQL + " limit " + StartIndex.ToString() + "," + MaxRecord.ToString(); ///取出MaxRecord条记录
            return Select(BlockStr, ParamList);
        }

        public override DataTable Select(string SQL, ODAParameter[] ParamList)
        {
            IDbCommand Cmd = OpenCommand();
            IDataReader Dr = null;
            try
            {
                Cmd.CommandType = CommandType.Text;
                SetCmdParameters(ref Cmd, SQL, ParamList);
                Dr = Cmd.ExecuteReader();
                DataTable dt = new DataTable("RECORDSET");

                List<int> mysqlDtIdx = new List<int>();
                if (Dr.FieldCount > 0)
                {
                    for (int num = 0; num < Dr.FieldCount; num++)
                    {
                        DataColumn column = new DataColumn();
                        if (dt.Columns.Contains(Dr.GetName(num)))
                            column.ColumnName = Dr.GetName(num) + num.ToString();
                        else
                            column.ColumnName = Dr.GetName(num);

                        var dtype = Dr.GetFieldType(num);

                        if (dtype == typeof(MySqlDateTime))
                        {
                            mysqlDtIdx.Add(num);
                            column.DataType = typeof(DateTime);
                        }
                        else
                        {
                            column.DataType = dtype;
                        }
                        dt.Columns.Add(column);
                    }
                    while (Dr.Read())
                    {
                        object[] val = new object[dt.Columns.Count];
                        Dr.GetValues(val);

                        for (int i = 0; i < mysqlDtIdx.Count; i++)
                        {
                            if (val[mysqlDtIdx[i]] is MySqlDateTime
                                &&((MySqlDateTime)val[mysqlDtIdx[i]]).IsValidDateTime)
                            {
                                val[mysqlDtIdx[i]] = ((MySqlDateTime)val[mysqlDtIdx[i]]).Value; 
                            }
                            else
                            {
                                val[mysqlDtIdx[i]] = null;
                            }
                        } 
                        dt.Rows.Add(val);
                    }
                } 
                return dt;
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


        public override List<T> Select<T>(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord, string Orderby)
        {
            IDbCommand Cmd = OpenCommand();
            IDataReader Dr = null;
            try
            {
                string BlockStr = SQL + " limit " + StartIndex.ToString() + "," + MaxRecord.ToString();
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

        public override bool Import(DataTable Data, ODAParameter[] Prms)
        {
            bool HaveBlob = false; 
            for(int k = 0; k < Prms.Length;k ++)
            {
                if(Prms[k].DBDataType == ODAdbType.OBinary)
                {
                    HaveBlob = true;
                    break;
                }
            } 
            if (HaveBlob)
                return base.Import(Data, Prms);

            int ImportCount = 0;
            MySqlConnection conn = null;
            DataTable ImportData = Data.Copy();
            string tmpPath = Path.GetTempFileName();
            try
            {
                MySqlBulkLoader bulk = null;
                if (this.Transaction != null)
                {
                    bulk = new MySqlBulkLoader((MySqlConnection)this.Transaction.Connection);
                }
                else
                {
                    conn = (MySqlConnection)this.GetConnection();
                    bulk = new MySqlBulkLoader((MySqlConnection)conn);
                }

                bool noCol = true;
                for(int m = 0; m < Prms.Length; m ++ )
                {
                    noCol = true;
                    for (int n = 0; n < ImportData.Columns.Count; n ++ )
                    {
                        if(Prms[m].ColumnName == ImportData.Columns[n].ColumnName)
                        {
                            noCol = false;
                            break;
                        } 
                    }
                    if(noCol)
                    {
                        ImportData.Columns.Add(new DataColumn(Prms[m].ColumnName)); 
                    }
                    ImportData.Columns[Prms[m].ColumnName].SetOrdinal(m);///对DataTable字段排序
                    bulk.Columns.Add(Prms[m].ParamsName);
                } 
                
                string csv = DataTableToCsv(ImportData, Prms.Length);
                File.WriteAllText(tmpPath, csv); 
                bulk.FieldTerminator = ",";
                bulk.FieldQuotationCharacter = '"';
                bulk.EscapeCharacter = '"';
                bulk.LineTerminator = "\r\n";
                bulk.FileName = tmpPath;
                bulk.NumberOfLinesToSkip = 0;
                bulk.TableName = ImportData.TableName; 
                ImportCount = bulk.Load();
                return ImportCount > 0;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                if (File.Exists(tmpPath))
                    File.Delete(tmpPath);
            }
        }
        /// <summary>
        ///将DataTable转换为标准的CSV
        /// </summary>
        /// <param name="Table">数据表</param>
        /// <returns>返回标准的CSV</returns>
        private static string DataTableToCsv(DataTable Table,int MaxCols)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in Table.Rows)
            {
                for (int i = 0; i < MaxCols; i++)
                {
                    colum = Table.Columns[i]; 
                    if (i != 0)
                        sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else
                    {
                        sb.Append(row[colum].ToString());
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public override object GetExpressResult(string ExpressionString)
        {
            IDbCommand Cmd = OpenCommand();
            try
            {
                string sql = new StringBuilder().Append(" SELECT" ).Append( ExpressionString ).Append( " AS VALUE ").ToString();
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
                    MySqlParameter param = new MySqlParameter();
                    param.ParameterName = pr.ParamsName;
                    if (pr.Size < 0)
                        param.Size = 1;
                    else
                        param.Size = pr.Size;
                    param.Direction = pr.Direction;
                    switch (pr.DBDataType)
                    {
                        case ODAdbType.ODatetime:
                            param.MySqlDbType = MySqlDbType.DateTime;
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
                            param.MySqlDbType = MySqlDbType.Decimal;
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
                            param.MySqlDbType = MySqlDbType.Blob;
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
                            param.MySqlDbType = MySqlDbType.Int32;
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
                            param.MySqlDbType = MySqlDbType.VarChar;
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
                            param.MySqlDbType = MySqlDbType.VarChar;
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
                            param.MySqlDbType = MySqlDbType.VarChar;
                            param.Value = pr.ParamsValue;
                            break;
                    }
                    ((MySqlParameterCollection)Cmd.Parameters).Add(param);
                }
            }
            Cmd.CommandText = SQL;
            FireExecutingCommand(Cmd);
        }
    }
}
