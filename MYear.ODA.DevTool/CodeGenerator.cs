using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MYear.ODA;

namespace MYear.ODA.DevTool
{
    public class CodeGenerator
    {
        private DBAccess _DBA = null;
        public CodeGenerator(DBAccess DBA)
        {
            _DBA = DBA;
        }

        public string Pascal(string InputString)
        {
            string result = InputString;
            if (result.IndexOf('_') > 0)
            {
                char[] arr = result.ToCharArray();
                if (char.IsLower(arr[0]))
                    arr[0] = char.ToUpper(arr[0]);
                for (int i = 1; i < arr.Length; i++)
                {
                    if (arr[i] == '_' && i + 1 < arr.Length)
                    {
                        i++;
                        if (char.IsLower(arr[i]))
                            arr[i] = char.ToUpper(arr[i]);

                    }
                    else
                    {
                        if (char.IsUpper(arr[i]))
                            arr[i] = char.ToLower(arr[i]);
                    }
                }
                result = new string(arr).Replace("_", "");
            }
            else
            {
                char[] arr = result.ToCharArray();
                bool hasLower = false;
                bool hasUpper = false;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (char.IsLower(arr[i]))
                    {
                        hasLower = true;
                        break;
                    }
                }
                for (int i = 0; i < arr.Length; i++)
                {
                    if (char.IsUpper(arr[i]))
                    {
                        hasUpper = true;
                        break;
                    }
                }
                if (!hasLower)
                {
                    for (int i = 1; i < arr.Length; i++)
                    {
                        arr[i] = char.ToLower(arr[i]);
                    }
                    result = new string(arr);
                }
                if (!hasUpper)
                {
                    arr[0] = char.ToUpper(arr[0]);
                    result = new string(arr);
                }
            }
            return result;
        }

        public string CsharpTypeDefaultValue(string CsharpType)
        {
            string rtl = "";
            switch (CsharpType)
            {
                case "byte[]":
                    rtl = "new byte[]{}";
                    break;
                case "int":
                case "decimal":
                    rtl = "-1";
                    break;
                case "System.DateTime":
                case "DateTime":
                    rtl = "Convert.ToDateTime(\"1900-01-01\")";
                    break;
                case "string":
                    rtl = "\"\"";
                    break;
            }
            return rtl;
        }

        public string[] Generate_Code(params string[] TablesAndViews)
        {
            System.Text.StringBuilder strbldr = new System.Text.StringBuilder();
            System.Text.StringBuilder ModelCode = new System.Text.StringBuilder();

            strbldr.AppendLine( "using System;");
            strbldr.AppendLine( "using System.Data;");
            strbldr.AppendLine( "using System.Collections.Generic;");
            strbldr.AppendLine( "using System.Reflection;");
            strbldr.AppendLine( "using MYear.ODA;");
            strbldr.AppendLine("using MYear.ODA.Model;");
            strbldr.AppendLine();
            strbldr.AppendLine("namespace MYear.ODA.Cmd");
            strbldr.AppendLine("{");

            ModelCode.AppendLine("using System;");
            ModelCode.AppendLine();
            ModelCode.AppendLine("namespace MYear.ODA.Model");
            ModelCode.AppendLine("{");

            DataTable pdt_tables = _DBA.GetTableColumns();
            DataTable pdt_views = _DBA.GetViewColumns();

            for (int i = 0; i < TablesAndViews.Length; i++)
            {
                string TablePascalName = TablesAndViews[i];
                System.Text.StringBuilder strCmd = new System.Text.StringBuilder();
                System.Text.StringBuilder strModel = new System.Text.StringBuilder();
                System.Text.StringBuilder GetColumnList = new StringBuilder();
                GetColumnList.AppendLine("\t\t public override List<ODAColumns> GetColumnList() ");
                GetColumnList.AppendLine("\t\t { ");
                GetColumnList.Append("\t\t\t return new List<ODAColumns>() { ");

                DataRow[] drs = pdt_tables.Select("TABLE_NAME ='" + TablesAndViews[i] + "'");
                if (drs.Length < 1)
                {
                    drs = pdt_views.Select("TABLE_NAME ='" + TablesAndViews[i] + "'");
                    strCmd.AppendLine("\t\t public override bool Insert(params IODAColumns[] Cols) { throw new ODAException(\"Not suport Insert CmdName \" + CmdName);}");
                    strCmd.AppendLine("\t\t public override bool Update(params IODAColumns[] Cols) {  throw new ODAException(\"Not Suport Update CmdName \" + CmdName);}");
                    strCmd.AppendLine("\t\t public override bool Delete() {  throw new ODAException(\"Not Suport Delete CmdName \" + CmdName);}");
                }

                for (int j = 0; j < drs.Length; j++)
                {
                    int Scale = 0;
                    int.TryParse(drs[j]["SCALE"].ToString().Trim(), out Scale);
                    int length = 2000;
                    int.TryParse(drs[j]["LENGTH"].ToString().Trim(), out length);
                    string ColumnName = drs[j]["COLUMN_NAME"].ToString().Trim();
                    string ColumnPascalName = "Col" + this.Pascal(ColumnName);

                    DBColumnInfo CsharpColumnInfo = new DBColumnInfo()
                    {
                        ColumnName = ColumnName,
                        ColumnType = drs[j]["DATATYPE"].ToString().Trim(),
                        Length = length,
                        Scale = Scale,
                        NotNull = drs[j]["NOT_NULL"].ToString().Trim().ToUpper() == "Y",
                    };
                    DBColumnInfo ODAColInfo = new DBColumnInfo()
                    {
                        ColumnName = ColumnName,
                        ColumnType = drs[j]["DATATYPE"].ToString().Trim(),
                        Length = length,
                        Scale = Scale,
                        NotNull = drs[j]["NOT_NULL"].ToString().Trim().ToUpper() == "Y",
                    };


                    CurrentDatabase.GetTargetsType(CurrentDatabase.DataSource.DBAType.ToString(), "CSHARP", ref CsharpColumnInfo);
                    CurrentDatabase.GetTargetsType( CurrentDatabase.DataSource.DBAType.ToString(), "ODA",ref ODAColInfo);
                    strModel.AppendLine("\t\t public " + CsharpColumnInfo.ColumnType + " " + ColumnName + " {get; set;}");
                    strCmd.AppendLine("\t\t public ODAColumns " + ColumnPascalName + "{ get { return new ODAColumns(this, \"" + ColumnName + "\", ODAdbType." + ODAColInfo.ColumnType + ", " + ODAColInfo.Length + "," + (drs[j]["NOT_NULL"].ToString().Trim() == "Y" ? "true" : "false") + " ); } }");
                    GetColumnList.Append(ColumnPascalName + ",");
                }

                strCmd.AppendLine("\t\t public override string CmdName { get { return \"" + TablesAndViews[i] + "\"; }}");
                strCmd.AppendLine( GetColumnList.Remove(GetColumnList.Length -1,1).ToString() + "};");
                strCmd.AppendLine("\t\t }");

                ModelCode.AppendLine("\tpublic partial class " + TablePascalName);
                ModelCode.AppendLine("\t{");
                ModelCode.Append(strModel);
                ModelCode.AppendLine("\t}");
    
                ////Cmd的代碼
                strbldr.Append("\tinternal partial class ");
                strbldr.Append("Cmd" + Pascal(TablePascalName));
                strbldr.AppendLine(":ORMCmd<" + TablePascalName + ">");
                strbldr.AppendLine("\t{"); 
                strbldr.Append(strCmd);
                strbldr.AppendLine("\t}");
            }

            strbldr.AppendLine("}");
            ModelCode.AppendLine("}");
            return new string[] { strbldr.ToString(), ModelCode.ToString() };
        }

        public string GenerateORMBase(string DBConnectString)
        {
            StringBuilder OrmBaseStr = new StringBuilder();
            string DBAstr = " " + _DBA.GetType().FullName + "(@\"" + DBConnectString + "\")";
            OrmBaseStr.AppendLine("using System;");
            OrmBaseStr.AppendLine("using System.Data;");
            OrmBaseStr.AppendLine("using System.Collections.Generic;");
            OrmBaseStr.AppendLine("using System.Reflection;");
            OrmBaseStr.AppendLine("using MYear.ODA;");
            OrmBaseStr.AppendLine("namespace MYear.ODA.Cmd");
            OrmBaseStr.AppendLine("{");
            OrmBaseStr.AppendLine(" public abstract class ORMCmd<T> : ODACmd where T : class ");
            OrmBaseStr.AppendLine(" { ");
            OrmBaseStr.AppendLine(" public override string ParamsMark { get { return  DBA.ParamsMark; } }");
            OrmBaseStr.AppendLine(" protected override IDBAccess DBA  { get { return new  " + DBAstr + "; }}");
            OrmBaseStr.AppendLine(" public virtual long GetSequence() ");
            OrmBaseStr.AppendLine(" { ");
            OrmBaseStr.AppendLine("return DBA.GetSequenceNextVal(this.Tran == null ? null : this.Tran.TranID, this.CmdName + \"_SEQ\");");
            OrmBaseStr.AppendLine("}");
            OrmBaseStr.AppendLine("  public abstract List<ODAColumns> GetColumnList();");
            OrmBaseStr.AppendLine("  public List<ODAColumns> BindColumnValues(T Model)");
            OrmBaseStr.AppendLine(" {");
            OrmBaseStr.AppendLine("     PropertyInfo[] Pis = Model.GetType().GetProperties();");
            OrmBaseStr.AppendLine("    List<ODAColumns> Cs = GetColumnList();");
            OrmBaseStr.AppendLine("    List<ODAColumns> CList = new List<ODAColumns>();");
            OrmBaseStr.AppendLine("    foreach (PropertyInfo Pi in Pis)");
            OrmBaseStr.AppendLine("  {");
            OrmBaseStr.AppendLine("        object V = Pi.GetValue(Model, null);");
            OrmBaseStr.AppendLine("       if (V != DBNull.Value && V != null)");
            OrmBaseStr.AppendLine("            foreach (ODAColumns C in Cs)");
            OrmBaseStr.AppendLine("             if (C.ColumnName == Pi.Name)");
            OrmBaseStr.AppendLine("               {");
            OrmBaseStr.AppendLine("                   C.SetCondition(CmdConditionSymbol.EQUAL, V);");
            OrmBaseStr.AppendLine("                    CList.Add(C);");
            OrmBaseStr.AppendLine("               }");
            OrmBaseStr.AppendLine("  }");
            OrmBaseStr.AppendLine("     return CList;");
            OrmBaseStr.AppendLine("  }");
            OrmBaseStr.AppendLine("public List<T> SelectM(params ODAColumns[] Cols)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("return base.Select<T>(Cols);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public List<T> SelectM(int StartIndex, int MaxRecord, out int TotalRecord, params ODAColumns[] Cols)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine(" return base.Select<T>(StartIndex, MaxRecord, out  TotalRecord, Cols);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public bool Insert(T Model)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine(" return Insert(BindColumnValues(Model).ToArray());");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public bool Update(T Model)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("return Update(BindColumnValues(Model).ToArray());");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public new ORMCmd<T> Distinct");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("get{_Distinct = true;return this;}");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public new ORMCmd<T> ListCmd(params ODACmd[] Cmds)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("return (ORMCmd<T>)base.ListCmd(Cmds);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public new ORMCmd<T> LeftJoin(ODACmd JoinCmd, params ODAColumns[] On)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("return (ORMCmd<T>)base.LeftJoin(JoinCmd, On);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public new ORMCmd<T> InnerJoin(ODACmd JoinCmd, params ODAColumns[] On)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("return (ORMCmd<T>)base.InnerJoin(JoinCmd, On);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public new ORMCmd<T> StartWithConnectBy(string StartWithExpress, string ConnectByParent, string PriorChild, string ConnectColumn, string ConnectStr, int MaxLevel)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("return (ORMCmd<T>)base.StartWithConnectBy(StartWithExpress, ConnectByParent, PriorChild, ConnectColumn, ConnectStr, MaxLevel);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public new ORMCmd<T> OrderbyAsc(params ODAColumns[] ColumnNames)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("return (ORMCmd<T>)base.OrderbyAsc(ColumnNames);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public new ORMCmd<T> OrderbyDesc(params ODAColumns[] ColumnNames)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("return (ORMCmd<T>)base.OrderbyDesc(ColumnNames);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public new ORMCmd<T> Groupby(params ODAColumns[] ColumnNames)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("return (ORMCmd<T>)base.Groupby(ColumnNames);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public new ORMCmd<T> Having(params ODAColumns[] Params)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("return (ORMCmd<T>)base.Having(Params);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine(" public new ORMCmd<T> Where(params ODAColumns[] Cols)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("return (ORMCmd<T>)base.Where(Cols);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public new ORMCmd<T> Or(params ODAColumns[] Cols)");
            OrmBaseStr.AppendLine("{ ");
            OrmBaseStr.AppendLine("return (ORMCmd<T>)base.Or(Cols);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine("public new ODAParameter[] GetCountSql(out string CountSql, ODAColumns Col)");
            OrmBaseStr.AppendLine("{");
            OrmBaseStr.AppendLine("return base.GetCountSql(out CountSql, Col);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine(" public new ODAParameter[] GetSelectSql(out string SelectSql, params ODAColumns[] Cols)");
            OrmBaseStr.AppendLine("{");
            OrmBaseStr.AppendLine("    return base.GetSelectSql(out SelectSql, Cols);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine(" public new ODAParameter[] GetUpdateSql(out string Sql, params ODAColumns[] Cols)");
            OrmBaseStr.AppendLine("{");
            OrmBaseStr.AppendLine("    return base.GetUpdateSql(out Sql, Cols);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine(" public new ODAParameter[] GetInsertSql(out string Sql, params ODAColumns[] Cols)");
            OrmBaseStr.AppendLine("{");
            OrmBaseStr.AppendLine("    return base.GetInsertSql(out Sql, Cols);");
            OrmBaseStr.AppendLine(" }");
            OrmBaseStr.AppendLine(" public new ODAParameter[] GetDeleteSql(out string Sql, params ODAColumns[] Cols)");
            OrmBaseStr.AppendLine(" {");
            OrmBaseStr.AppendLine("    return base.GetDeleteSql(out Sql, Cols);");
            OrmBaseStr.AppendLine("  }");
            OrmBaseStr.AppendLine("}");
            OrmBaseStr.AppendLine("}");

            return OrmBaseStr.ToString();
        }
    }
}
