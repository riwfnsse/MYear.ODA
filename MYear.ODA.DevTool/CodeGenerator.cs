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

        public string GenerateContext(params string[] TablesAndViews)
        {
            System.Text.StringBuilder ctxStr = new System.Text.StringBuilder();

            ctxStr.AppendLine("using MYear.ODA;");
            ctxStr.AppendLine("using MYear.ODA.Cmd;");
            ctxStr.AppendLine();
            ctxStr.AppendLine("namespace MYear.ODA.Ctx");
            ctxStr.AppendLine("{");
             
            ctxStr.AppendLine("\tinternal static class BizContext");
            ctxStr.AppendLine("\t{");

            ctxStr.AppendLine("\t\tinternal static ODAContext GetContext()");
            ctxStr.AppendLine("\t\t{");
            ctxStr.AppendLine("\t\t\treturn new ODAContext(DbAType." + Enum.GetName(typeof(DbAType), _DBA.DBAType) + ",\"" + _DBA.ConnString + "\");");
            ctxStr.AppendLine("\t\t}");


            for (int i = 0; i < TablesAndViews.Length; i++)
            {
                string cmd = "Cmd" + Pascal(TablesAndViews[i]);
                ctxStr.AppendLine("\t\tinternal static " + cmd + " " + TablesAndViews[i] + "(this ODAContext Ctx)");
                ctxStr.AppendLine("\t\t{");
                ctxStr.AppendLine("\t\t\treturn Ctx.GetCmd<" + cmd + ">();");
                ctxStr.AppendLine("\t\t}"); 
            }
            ctxStr.AppendLine("\t}");
            ctxStr.AppendLine("}");
            return ctxStr.ToString();
        } 
    }
}
