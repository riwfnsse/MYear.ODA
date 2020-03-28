using System.Collections.Generic;

namespace MYear.ODA
{
    public class ODAView : ODACmd
    {
        private ODACmd _Cmd = null;
        private IODAColumns[] SelectCols = null;
        public override string CmdName
        {
            get
            {
                return _Cmd.CmdName;
            }
        } 
        public ODAColumns[] ViewColumns
        {
            get
            {
                int vcl = SelectCols == null ? 0 : SelectCols.Length;
                ODAColumns[] vc = new ODAColumns[vcl];
                for (int i = 0; i < vcl && SelectCols != null; i++)
                {
                    if (string.IsNullOrWhiteSpace(SelectCols[i].AliasName))
                        vc[i] = new ODAColumns(this, SelectCols[i].ColumnName, SelectCols[i].DBDataType, SelectCols[i].Size);
                    else
                        vc[i] = new ODAColumns(this, SelectCols[i].AliasName, SelectCols[i].DBDataType, SelectCols[i].Size);
                }
                return vc;
            }
        }
        protected override ODACmd BaseCmd
        {
            get
            {
                return _Cmd;
            }
        }

        protected override string DBObjectMap
        {
            get
            {
                return ((IODACmd)_Cmd).DBObjectMap;
            }
            set
            {
                ((IODACmd)_Cmd).DBObjectMap = value;
            }
        }

        protected override ODAScript GetCmdSql()
        {
            var view = ((ODACmd)_Cmd).GetSelectSql(SelectCols);
            view.SqlScript.Insert(0, "(").Append(")");
            return view;
        }
        public ODAColumns CreateColumn(string ColName, ODAdbType ColType = ODAdbType.OVarchar, int size = 2000)
        {
            return new ODAColumns(this, ColName, ColType, size);
        }

        internal ODAView(ODACmd Cmd, params IODAColumns[] Cols)
        {
            _Cmd = Cmd;
            Alias = Cmd.GetAlias();
            SelectCols = Cols; 
        }
    }
}