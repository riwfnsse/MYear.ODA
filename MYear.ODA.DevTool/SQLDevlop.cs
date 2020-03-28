using ICSharpCode.TextEditor.Document;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MYear.ODA.DevTool
{
    public partial class SQLDevlop : Form
    {
        public SQLDevlop()
        {
            InitializeComponent();
        }

        private void SQLDevlop_Load(object sender, EventArgs e)
        { 
            this.rtbxSql.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("TSQL");
            this.rtbxSql.Encoding = Encoding.GetEncoding("UTF-8");

            this.lbxTableView.Items.Clear();
            this.lbxTableView.Items.AddRange(CurrentDatabase.DataSource.GetUserTables());
            this.lbxTableView.Items.AddRange(CurrentDatabase.DataSource.GetUserViews());

            if (this.MdiParent is MdiParentForm)
            {
                this.MdiParent.MdiChildActivate += MdiParent_MdiChildActivate;
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            this.MdiParent.MdiChildActivate -= MdiParent_MdiChildActivate;
            ((MdiParentForm)this.MdiParent).ExecuteSQL -= ExecuteSQL;
            ((MdiParentForm)this.MdiParent).DbRefresh -= DbRefresh;
        }
 
        private void MdiParent_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.MdiParent.ActiveMdiChild == null ||  this.MdiParent.ActiveMdiChild != this)
            {
                ((MdiParentForm)this.MdiParent).ExecuteSQL -= ExecuteSQL;
                ((MdiParentForm)this.MdiParent).DbRefresh -= DbRefresh;
            }
            else
            {
                ((MdiParentForm)this.MdiParent).ExecuteSQL += ExecuteSQL;
                ((MdiParentForm)this.MdiParent).DbRefresh += DbRefresh;
            }
        }
     
        private void ExecuteSQL(object sender, EventArgs e)
        {
            if (this.rtbxSql.ActiveTextAreaControl.SelectionManager.SelectedText.Trim() == "")
                this.ExcuteSql(this.rtbxSql.Text);
            else
                this.ExcuteSql(this.rtbxSql.ActiveTextAreaControl.SelectionManager.SelectedText);
        }

        private void DbRefresh(object sender, EventArgs e)
        {
            this.lbxTableView.Items.Clear();
            this.lbxTableView.Items.AddRange(CurrentDatabase.DataSource.GetUserTables());
            this.lbxTableView.Items.AddRange(CurrentDatabase.DataSource.GetUserViews()); 
        }


        private void lbxTableView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.rtbxSql.ActiveTextAreaControl.TextArea.InsertString(lbxTableView.SelectedItem.ToString());
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.HWnd == this.rtbxSql.Handle)
            {
                if (keyData == (Keys.Control | Keys.Enter))
                {
                    ExecuteSQL(null, null);
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ExcuteSql(string Sql)
        {
            string ExSql = "";
            try
            {
                if (Sql.Trim().ToUpper().StartsWith("SELECT "))
                {
                    var cmd = CurrentDatabase.DataSource.Select(Sql); 
                    DataTable Dt = ReadData(cmd);
                    this.dgvExecuteSql.DataSource = Dt;
                    this.dgvExecuteSql.Update();
                    this.dgvExecuteSql.Refresh();
                    ExSql = Sql;
                    this.tbc_ExecuteSqlResult.SelectedTab = this.tpgGrid;
                }
                else
                {
                    int i = CurrentDatabase.DataSource.ExecuteSQL(Sql, null);
                    ExSql = "執行SQL " + Sql + "\r\t 返回影響行數:  " + i.ToString();
                    this.tbc_ExecuteSqlResult.SelectedTab = this.tpgMsg;
                }
            }
            catch (Exception ex)
            {
                ExSql = ex.Message;
                this.tbc_ExecuteSqlResult.SelectedTab = this.tpgMsg;
            }
            this.lblExecuteRlt.Text = ExSql;
        } 

        private  DataTable ReadData(IDbCommand Cmd)
        {
            IDataReader Dr = null;
            try
            {
                int StartIndex = 0;
                int MaxRecord = 100;
                Dr = Cmd.ExecuteReader();
                DataTable dt = new DataTable("RECORDSET");
                if (Dr.FieldCount > 0)
                {
                    for (int num = 0; num < Dr.FieldCount; num++)
                    {
                        DataColumn column = new DataColumn();
                        if (dt.Columns.Contains(Dr.GetName(num)))
                            column.ColumnName = Dr.GetName(num) + num.ToString();
                        else
                            column.ColumnName = Dr.GetName(num);
                        column.DataType = Dr.GetFieldType(num);
                        dt.Columns.Add(column);
                    }
                    while (StartIndex > 0)
                    {
                        if (!Dr.Read())
                            return dt;
                        StartIndex--;
                    }
                    int ReadRecord = MaxRecord;
                    while (ReadRecord > 0 || MaxRecord == -1)
                    {
                        if (Dr.Read())
                        {
                            object[] rVal = new object[Dr.FieldCount];
                            Dr.GetValues(rVal);
                            dt.Rows.Add(rVal);
                            ReadRecord--;
                        }
                        else
                            break;
                    }
                }
                return dt;
            }
            finally
            {
                if (Dr != null)
                {
                    Cmd.Cancel();
                    Dr.Close();
                    Dr.Dispose();
                }
                Cmd.Dispose(); 
            }
        }

        private void dgvExecuteSql_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }


}
