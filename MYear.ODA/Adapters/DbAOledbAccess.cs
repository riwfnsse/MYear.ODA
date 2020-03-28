#if NET_FW
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace MYear.ODA.Adapter
{
    public class DbAOledbAccess : DBAccess
    {
        private const char DBParamsMark =':';

        [Description("Access建立时自动添加了系统表。但Access默认是不显示的，要想看到这些表，得手动设置一下：选择菜单“工具”－“选项”－“视图”，在“系统对象”前面打勾，就能看到如下七个表了： "
            + " MSysAccessObjects、MSysAccessXML、MSysAces、MSysImexColumns、MSysObjects、MSysQueries、MSysRelationShips "
            + " 看是看到了，但还不能读取表里的数据，还需要设置权限：选择菜单“工具”－“安全”－“用户与组的权限”，把这些表的读写权限都勾上"
            //+ " ，OK！一切尽在掌握了，想怎么用就怎么用。"
            //+ " 遗憾的是，微软并没给出这些表的文档说明，具体功能也只好望文生义了。较常用的MSysObjects表，很显然储存的是一些对象，里面包含了两个字段Name和Type，可以依靠它们来判断数某个表或某个查询是否存在。"
            //+ " 例：SELECT [Name] FROM [MSysObjects] WHERE (Left([Name],1)<>'~') AND ([Type]=1)  ORDER BY [Name] "
            //+ "  其中已知的Type值和对应的类型分别是：1－表；5－查询；-32768－窗体；-32764－报表；-32761－模块；-32766－宏。"
            + @" Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\directory\demo.mdb;User Id=admin;Password=; ")]
        public DbAOledbAccess(string ConnectionString)
            : base(ConnectionString)
        {
        }
        public override char ParamsMark
        {
            get { return DbAOledbAccess.DBParamsMark; }
        }
        private OleDbConnection _DBConn = null;
        protected override IDbConnection GetConnection()
        {
            if (_DBConn == null)
            {
                _DBConn = new OleDbConnection(ConnString);
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
            return new OleDbDataAdapter((OleDbCommand)SelectCmd);
        }

        public override string[] GetUserTables()
        {
            DataTable dt_table = Select("SELECT [Name] AS TABLE_NAME  FROM [MSysObjects] WHERE (Left([Name],1)<>'~') AND ([Type]=1)  AND [Name] NOT IN ( 'MSysAccessObjects','MSysAccessXML','MSysAces','MSysImexColumns','MSysObjects','MSysQueries','MSysRelationShips')  ORDER BY [Name]", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["TABLE_NAME"].ToString().Trim();
            }
            return str;
        }
        public override string[] GetPrimarykey(string TableName)
        {
            return null;
        }
        public override Dictionary<string, string[]> GetPrimarykey()
        {
            return new Dictionary<string, string[]>();
        }
        public override DbAType DBAType { get { return DbAType.OledbAccess; } }

        public override string[] GetUserViews()
        {
            DataTable dt_table = Select("SELECT [Name] AS VIEW_NAME  FROM [MSysObjects] WHERE (Left([Name],1)<>'~') AND ([Type]=5)  AND [Name] NOT IN ( 'MSysAccessObjects','MSysAccessXML','MSysAces','MSysImexColumns','MSysObjects','MSysQueries','MSysRelationShips')  ORDER BY [Name]", null);
            string[] str = new string[dt_table.Rows.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = dt_table.Rows[i]["VIEW_NAME"].ToString().Trim();
            }
            return str;
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
                    dbSql = dbSql.Replace(pr.ParamsName, pr.ParamsName.Replace(ODAParameter.ODAParamsMark, DbAOledbAccess.DBParamsMark));
                    OleDbParameter param = new OleDbParameter();
                    param.ParameterName = pr.ParamsName;
                    if (pr.Size < 0)
                        param.Size = 1;
                    else
                        param.Size = pr.Size;
                    param.Direction = pr.Direction;
                    switch (pr.DBDataType)
                    {
                        case ODAdbType.ODatetime:
                            param.OleDbType = OleDbType.Date;
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
                            param.OleDbType = OleDbType.Decimal;
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
                            param.OleDbType = OleDbType.Binary;
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
                            param.OleDbType = OleDbType.Integer;
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
                            param.OleDbType = OleDbType.Char;
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
                            param.OleDbType = OleDbType.VarChar;
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
                            param.OleDbType = OleDbType.VarChar;
                            param.Value = pr.ParamsValue;
                            break;
                    }
                    ((OleDbParameterCollection)Cmd.Parameters).Add(param);
                }
            }
            Cmd.CommandText = dbSql;
            FireExecutingCommand(Cmd);
        }
    }
}
#endif