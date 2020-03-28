using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace MYear.ODA.Adapter
{
    public class DbAOracle : DBAccess
    {
        private const char DBParamsMark = ':'; 
        public DbAOracle(string ConnectionString)
            : base(ConnectionString)
        {

        }
        public override char ParamsMark
        {
            get { return DbAOracle.DBParamsMark; }
        }
        public override string[] ObjectFlag 
        {
            get { return new string[] { "\"", "\"" }; }
        }
        private OracleConnection _DBConn = null;
        protected override IDbConnection GetConnection()
        {
            if (_DBConn == null)
            {
                _DBConn = new OracleConnection(ConnString);
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
        public override DataSet ExecuteProcedure(string SQL, ODAParameter[] ParamList)
        {
            IDbCommand Cmd = OpenCommand();
            DataSet ds_rtl = new DataSet("ReturnValues");
            DataTable dt_values = new DataTable("ValuesCollection");
            dt_values.Columns.Add("ParamName");
            dt_values.Columns.Add("ReturnValue");
            try
            {
                Cmd.CommandType = CommandType.StoredProcedure;
                SetCmdParameters(ref Cmd, SQL, ParamList);
                Cmd.ExecuteNonQuery();
                foreach (OracleParameter opc in Cmd.Parameters)
                {
                    if (opc.Direction == System.Data.ParameterDirection.InputOutput || opc.Direction == System.Data.ParameterDirection.Output)
                    {
                        // if (opc.OracleType == System.Data.OracleClient.OracleType.Cursor)
                        if (opc.OracleDbType == OracleDbType.RefCursor)
                        {
                            DataTable dt = new DataTable(opc.ParameterName);
                            //OracleDataReader odr = (System.Data.OracleClient.OracleDataReader)opc.Value;
                            OracleDataReader odr = (OracleDataReader)opc.Value;
                            for (int num = 0; num < odr.FieldCount; num++)
                            {
                                DataColumn column = new DataColumn();
                                column.DataType = odr.GetFieldType(num);
                                column.ColumnName = odr.GetName(num);
                                dt.Columns.Add(column);
                            }
                            while (odr.Read())
                            {
                                DataRow row = dt.NewRow();
                                for (int num = 0; num < odr.FieldCount; num++)
                                {
                                    row[num] = odr[num];
                                }
                                dt.Rows.Add(row);
                            }
                            ds_rtl.Tables.Add(dt);
                            odr.Close();
                            odr.Dispose();
                        }
                        else
                        {
                            DataRow dr = dt_values.NewRow();
                            dr["ParamName"] = opc.ParameterName;
                            dr["ReturnValue"] = opc.Value;
                            dt_values.Rows.Add(dr);
                        }
                    }
                }
                ds_rtl.Tables.Add(dt_values);
                return ds_rtl;
            }
            finally
            {
                dt_values.Dispose();
                ds_rtl.Dispose();
                CloseCommand(Cmd);
            }
        }
        protected override DbDataAdapter GetDataAdapter(IDbCommand SelectCmd)
        {
            ((OracleCommand)SelectCmd).BindByName = true;
            return new OracleDataAdapter((OracleCommand)SelectCmd);
        }
        public override DateTime GetDBDateTime()
        {
            IDbCommand Cmd = OpenCommand();
            try
            {
                Cmd.CommandText = "SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') DB_DATETIME FROM DUAL ";
                Cmd.CommandType = CommandType.Text;
                //return Convert.ToDateTime(((OracleCommand)Cmd).ExecuteOracleScalar());
                return Convert.ToDateTime(((OracleCommand)Cmd).ExecuteScalar());
            }
            finally
            {
                CloseCommand(Cmd);
            }
        }
        public override string[] GetUserTables()
        {
            DataTable dt_table = Select("SELECT TABLE_NAME FROM USER_TABLES ORDER BY TABLE_NAME", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["TABLE_NAME"].ToString();
            }
            return str;
        }
        public override string[] GetUserViews()
        {
            DataTable dt_table = Select("SELECT VIEW_NAME FROM USER_VIEWS ORDER BY VIEW_NAME", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["VIEW_NAME"].ToString();
            }
            return str;
        }
        /*
         *
         *
         * 存储过程 SELECT DBMS_METADATA.GET_DDL(U.OBJECT_TYPE, u.object_name)
FROM USER_OBJECTS u
where U.OBJECT_TYPE IN ('PROCEDURE'，'PACKAGE');
         
            视图
             select dbms_metadata.get_ddl('VIEW','VM_EXCEPTION_INFO') from dual;
             SELECT DBMS_METADATA.GET_DDL('VIEW',u.VIEW_name) FROM USER_VIEWS u;
             */
        public override string[] GetPrimarykey(string TableName)
        {
            string PrimaryCols = string.Format("SELECT DISTINCT  A.COLUMN_NAME  FROM USER_CONS_COLUMNS A, USER_CONSTRAINTS B  WHERE A.CONSTRAINT_NAME = B.CONSTRAINT_NAME  AND B.CONSTRAINT_TYPE = 'P' AND A.TABLE_NAME ='{0}'", TableName);
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
            string PrimaryCols = string.Format("SELECT DISTINCT A.TABLE_NAME, A.COLUMN_NAME  FROM USER_CONS_COLUMNS A, USER_CONSTRAINTS B  WHERE A.CONSTRAINT_NAME = B.CONSTRAINT_NAME  AND B.CONSTRAINT_TYPE = 'P' ORDER BY A.TABLE_NAME ");

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


        public override DbAType DBAType { get { return DbAType.Oracle; } }

 
        public override string[] GetUserProcedure()
        {
            DataTable dt_table = Select("SELECT ARG.OBJECT_NAME PROCEDURE_NAME FROM USER_OBJECTS O,USER_ARGUMENTS ARG WHERE  O.OBJECT_TYPE='PROCEDURE' AND O.OBJECT_NAME = ARG.OBJECT_NAME AND ARG.PACKAGE_NAME IS NULL "
            + " UNION SELECT ARG.PACKAGE_NAME||'.'|| ARG.OBJECT_NAME PROCEDURE_NAME FROM USER_OBJECTS O,USER_ARGUMENTS ARG WHERE  O.OBJECT_TYPE='PACKAGE' AND O.OBJECT_NAME = ARG.PACKAGE_NAME ", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["PROCEDURE_NAME"].ToString();
            }
            return str;
        }

        public override DataTable GetTableColumns()
        {
            string sql_tabcol = new StringBuilder().Append("SELECT TC.TABLE_NAME,TC.COLUMN_NAME,CASE TC.NULLABLE WHEN 'N' THEN 'Y' ELSE 'N' END  NOT_NULL,TC.COLUMN_ID COL_SEQ,")
            //.Append( " DECODE(TC.DATA_TYPE,'CHAR','OChar','VARCHAR','OVarchar','VARCHAR2','OVarchar','NVARCHAR2','OVarchar','MLSLABEL','OVarchar',")
            //.Append(" 'UROWID','OVarchar','URITYPE','OVarchar','CHARACTER','OVarchar','CLOB','OVarchar','INTEGER','OInt','INT','OInt',")
            //.Append(" 'SMALLINT','OInt','DATE','ODatetime','LONG','ODecimal','DECIMAL','ODecimal','NUMERIC','ODecimal','REAL','ODecimal',")
            //.Append(" 'NUMBER','ODecimal','BLOB','OBinary','BFILE','OBinary','OVarchar') DATATYPE," )
            .Append(" TC.DATA_TYPE  DATATYPE,") 
            .Append(" DECODE(TC.DATA_TYPE,'BLOB',2000000000,'CLOB',2000000000, TC.DATA_LENGTH)  LENGTH,TC.DATA_SCALE SCALE,")
            .Append(" TCC.COMMENTS DIRECTION ")
            .Append(" FROM USER_TABLES  TB,USER_TAB_COLUMNS TC ,USER_COL_COMMENTS  TCC")
            .Append(" WHERE TB.TABLE_NAME = TC.TABLE_NAME ")
            .Append(" AND TC.TABLE_NAME = TCC.table_name(+) ")
            .Append(" AND TC.COLUMN_NAME = TCC.column_name(+) ")
            .Append(" ORDER BY TC.TABLE_NAME,TC.COLUMN_ID ").ToString();
            DataTable Dt = Select(sql_tabcol, null);
            Dt.TableName = "TABLE_COLUMN";
            return Dt;
        }
        public override DataTable GetViewColumns()
        {
            string sql_view = new StringBuilder().Append("SELECT TC.TABLE_NAME,TC.COLUMN_NAME,CASE TC.NULLABLE WHEN 'N' THEN 'Y' ELSE 'N' END NOT_NULL,TC.COLUMN_ID COL_SEQ,")
                //.Append(" DECODE(TC.DATA_TYPE,'CHAR','OChar','VARCHAR','OVarchar','VARCHAR2','OVarchar','NVARCHAR2','OVarchar','MLSLABEL','OVarchar','UROWID','OVarchar','URITYPE','OVarchar','CHARACTER','OVarchar','CLOB','OVarchar', ")
                //.Append(" 'INTEGER','OInt','INT','OInt','SMALLINT','OInt','DATE','ODatetime','LONG','ODecimal','DECIMAL','ODecimal','NUMERIC','ODecimal','REAL','ODecimal','NUMBER','ODecimal','BLOB','OBinary','BFILE','OBinary','OVarchar') DATATYPE, ")
                .Append(" TC.DATA_TYPE  DATATYPE,")
                .Append(" DECODE(TC.DATA_TYPE,'BLOB',2000000000,'CLOB',2000000000, TC.DATA_LENGTH)  LENGTH,TC.DATA_SCALE SCALE,TCC.COMMENTS DIRECTION ")
                .Append(" FROM USER_VIEWS TV,USER_TAB_COLUMNS TC ,USER_COL_COMMENTS  TCC")
                .Append(" WHERE TV.VIEW_NAME = TC.TABLE_NAME ")
                .Append(" AND TC.TABLE_NAME = TCC.table_name(+) ")
                .Append(" AND TC.COLUMN_NAME = TCC.column_name(+) ")
                .Append(" ORDER BY  TC.TABLE_NAME,TC.COLUMN_NAME ").ToString();
            DataTable Dt = Select(sql_view, null);
            Dt.TableName = "VIEW_COLUMN";
            return Dt;
        }
        public override DataTable GetUserProcedureArguments(string ProcedureName)
        {
            string SqlArg = new StringBuilder().Append("SELECT arg.object_name PROCEDURE_NAME, arg.ARGUMENT_NAME,arg.DATA_TYPE,")
            .Append(" DECODE(arg.DATA_TYPE,'CHAR','OChar','VARCHAR','OVarchar','VARCHAR2','OVarchar','NVARCHAR2','OVarchar','MLSLABEL','OVarchar','UROWID','OVarchar','URITYPE','OVarchar','CHARACTER','OVarchar','CLOB','OVarchar', ")
            .Append(" 'INTEGER','OInt','INT','OInt','SMALLINT','OInt','DATE','ODatetime','LONG','ODecimal','DECIMAL','ODecimal','NUMERIC','ODecimal','REAL','ODecimal','NUMBER','ODecimal','BLOB','OBinary','BFILE','OBinary','PL/SQL TABLE','OArrary','REF CURSOR','OTable','OVarchar') DATATYPE, ")
            .Append(" arg.POSITION,arg.IN_OUT DIRECTION, ")
            .Append(" NVL(DECODE(arg.DATA_TYPE,'BLOB',0,'CLOB',0,arg.DATA_LENGTH),0)  LENGTH")
            .Append(" from user_objects o,user_arguments arg")
            .Append(" where  o.object_type='PROCEDURE' ")
            .Append(" and o.OBJECT_NAME = arg.OBJECT_NAME ")
            .Append(" and arg.PACKAGE_NAME is null")
            .Append(" and o.OBJECT_ID = arg.OBJECT_ID")
            .Append(" and o.OBJECT_NAME = @ProcedureName")
            .Append(" union ")
            .Append(" select arg.package_name||'.'|| arg.object_name PROCEDURE_NAME, arg.ARGUMENT_NAME, arg.DATA_TYPE,")
            .Append(" DECODE(arg.DATA_TYPE,'CHAR','OChar','VARCHAR','OVarchar','VARCHAR2','OVarchar','NVARCHAR2','OVarchar','MLSLABEL','OVarchar','UROWID','OVarchar','URITYPE','OVarchar','CHARACTER','OVarchar','CLOB','OVarchar', ")
            .Append(" 'INTEGER','OInt','INT','OInt','SMALLINT','OInt','DATE','ODatetime','LONG','ODecimal','DECIMAL','ODecimal','NUMERIC','ODecimal','REAL','ODecimal','NUMBER','ODecimal','BLOB','OBinary','BFILE','OBinary','PL/SQL TABLE','OArrary','REF CURSOR','OTable','OVarchar') DATATYPE, ")
            .Append(" arg.POSITION,arg.IN_OUT DIRECTION,")
            .Append(" NVL(DECODE(arg.DATA_TYPE,'BLOB',0,'CLOB',0,arg.DATA_LENGTH),0)  LENGTH")
            .Append(" from user_objects o,user_arguments arg")
            .Append(" where  o.object_type='PACKAGE' ")
            .Append(" and o.OBJECT_NAME = arg.package_name ")
            .Append(" and o.OBJECT_ID = arg.OBJECT_ID")
            .Append(" and arg.package_name||'.'|| arg.object_name =@ProcedureName")
            .Append(" ORDER BY PROCEDURE_NAME ,POSITION ").ToString();

            ODAParameter p = new ODAParameter() { DBDataType = ODAdbType.OVarchar, Direction = ParameterDirection.Input, ParamsName = "@ProcedureName", ParamsValue = ProcedureName, Size = 200 };

            DataTable Dttmp = Select(SqlArg, new ODAParameter[] { p });
            Dttmp.TableName = "PROCEDURE_ARGUMENTS";
            return Dttmp;
        }

        public override DataTable Select(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord, string Orderby)
        {
            string BlockStr = new StringBuilder().Append("SELECT * FROM (SELECT ROWNUM AS R_ID_1 ,T_T_1.* FROM ( ")
            .Append(SQL)
            .Append(") T_T_1 ) WHERE R_ID_1 > ").Append( StartIndex.ToString() ).Append(" AND R_ID_1 <= " ).Append((StartIndex + MaxRecord).ToString()).ToString();  ///取出MaxRecord条记录
            DataTable dt = Select(BlockStr, ParamList);
            dt.Columns.Remove("R_ID_1");
            return dt;
        }
        public override List<T> Select<T>(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord, string Orderby)
        {
            IDbCommand Cmd = OpenCommand();
            IDataReader Dr = null;
            try
            {
                string BlockStr = new StringBuilder().Append("SELECT * FROM (SELECT ROWNUM AS R_ID_1 ,T_T_1.* FROM ( ")
               .Append(SQL)
                .Append(") T_T_1 ) WHERE R_ID_1 > ").Append(StartIndex.ToString()).Append(" AND R_ID_1 <= ").Append((StartIndex + MaxRecord).ToString()).ToString();  ///取出MaxRecord条记录
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
            string Sqlcols = "";
            string Sqlprms = "";
            DataTable ImportData = Data.Copy();
            IDbCommand Cmd = OpenCommand();
            try
            {
                for (int i = 0; i < Prms.Length; i++)
                {
                    Sqlcols += "," + Prms[i].ColumnName;
                    Sqlprms += "," + this.ParamsMark + Prms[i].ParamsName;
                    OracleParameter oraPrms = new OracleParameter();
                    oraPrms.ParameterName = this.ParamsMark + Prms[i].ParamsName;
                    oraPrms.OracleDbType = GetOracleType(Prms[i].DBDataType);
                    oraPrms.Size = Prms[i].Size;
                    oraPrms.Direction = ParameterDirection.Input;
                    oraPrms.Value = new object[ImportData.Rows.Count];
                    Cmd.Parameters.Add(oraPrms);  
                    if (!ImportData.Columns.Contains(Prms[i].ColumnName))
                    {
                        ImportData.Columns.Add(new DataColumn(Prms[i].ColumnName));
                    }
                    ImportData.Columns[Prms[i].ColumnName].SetOrdinal(i); 
                }
                string sql = "INSERT INTO " + ImportData.TableName + " ( " + Sqlcols.TrimStart(',') + ") VALUES (" + Sqlprms.TrimStart(',') + ") ";
                for (int i = 0; i < ImportData.Rows.Count; i++)  
                    for (int j = 0; j < Cmd.Parameters.Count; j++)
                        ((object[])((OracleParameter)Cmd.Parameters[j]).Value)[i] = ImportData.Rows[i].ItemArray[j];

                Cmd.CommandText = sql;
                Cmd.CommandType = CommandType.Text;
                ((OracleCommand)Cmd).ArrayBindCount = ImportData.Rows.Count;
                return Cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                CloseCommand(Cmd);
            }
        }

        private OracleDbType GetOracleType(ODAdbType OdaType)
        {
            switch (OdaType)
            {
                case ODAdbType.ODatetime:
                    return OracleDbType.Date;
                case ODAdbType.ODecimal:
                    return OracleDbType.Decimal;
                case ODAdbType.OBinary:
                    return OracleDbType.Blob;
                case ODAdbType.OInt:
                    return OracleDbType.Int32;
                case ODAdbType.OChar:
                    return OracleDbType.Char;
                case ODAdbType.OVarchar:
                    return OracleDbType.Varchar2;
                case ODAdbType.OArrary:
                    return OracleDbType.Raw;
                default:
                    return OracleDbType.Varchar2;
            }
        }

        public override object GetExpressResult(string ExpressionString)
        {
            IDbCommand Cmd = OpenCommand();
            try
            {
                Cmd.CommandText = "SELECT " + ExpressionString + " AS VALUE FROM DUAL ";
                Cmd.CommandType = CommandType.Text;
                // return ((OracleCommand)Cmd).ExecuteOracleScalar();
                return ((OracleCommand)Cmd).ExecuteScalar();
            }
            finally
            {
                CloseCommand(Cmd);
            }
        }
        public override long GetSequenceNextVal(string SequenceName)
        {
            IDbCommand Cmd = OpenCommand();
            try
            {
                string sql = " SELECT " + SequenceName + ".NEXTVAL FROM DUAL";
                Cmd.CommandText = sql;
                Cmd.CommandType = CommandType.Text;
                //return long.Parse(((OracleCommand)Cmd).ExecuteOracleScalar().ToString());
                return long.Parse(((OracleCommand)Cmd).ExecuteScalar().ToString());
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
                    dbSql = dbSql.Replace(pr.ParamsName, pr.ParamsName.Replace(ODAParameter.ODAParamsMark, DbAOracle.DBParamsMark)); 
                    OracleParameter param = new OracleParameter();
                    param.ParameterName = pr.ParamsName.Replace(ODAParameter.ODAParamsMark, DbAOracle.DBParamsMark);
                    if (pr.Size < 0)
                        param.Size = 1;
                    else
                        param.Size = pr.Size;
                    param.Direction = pr.Direction;
                    switch (pr.DBDataType)
                    {
                        case ODAdbType.ODatetime:
                            // param.OracleType = OracleType.DateTime;
                            param.OracleDbType = OracleDbType.Date;
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
                            //param.OracleType = OracleType.Number;
                            param.OracleDbType = OracleDbType.Decimal;
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
                            //param.OracleType = OracleType.Blob;
                            param.OracleDbType = OracleDbType.Blob;
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
                            //  param.OracleType = OracleType.Int32;
                            param.OracleDbType = OracleDbType.Int32;
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
                            // param.OracleType = OracleType.Char;
                            param.OracleDbType = OracleDbType.Char;
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
                            // param.OracleType = OracleType.VarChar;
                            param.OracleDbType = OracleDbType.Varchar2;
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
                        case ODAdbType.OArrary:
                            // param.OracleType = OracleType.VarChar;
                            param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                            param.OracleDbType = OracleDbType.Varchar2;
                            if (pr.ParamsValue == null || pr.ParamsValue is System.DBNull)
                            {
                                param.Value = System.DBNull.Value;
                            }
                            else
                            {
                                param.Value = pr.ParamsValue;
                                if (param.Value is Array)
                                    param.Size = ((Array)param.Value).Length;
                            }
                            break;
                        default:
                            // param.OracleType = OracleType.VarChar;
                            param.OracleDbType = OracleDbType.Varchar2;
                            param.Value = pr.ParamsValue;
                            break;
                    }
                    ((OracleParameterCollection)Cmd.Parameters).Add(param); 
                }
            }
            Cmd.CommandText = dbSql;
            FireExecutingCommand(Cmd);
        }
    }
}
