using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ICSharpCode.TextEditor.Document;

namespace MYear.ODA.DevTool
{
    public partial class ORMCode : Form
    {
        public ORMCode()
        {
            InitializeComponent();
        }
        private void ORMCode_Load(object sender, EventArgs e)
        {
            this.ckbx_databaseobject.Items.Clear();
            this.ckbx_databaseobject.Items.AddRange(CurrentDatabase.DataSource.GetUserTables());
            this.ckbx_databaseobject.Items.AddRange(CurrentDatabase.DataSource.GetUserViews());

            if (this.MdiParent is MdiParentForm)
            {
                this.MdiParent.MdiChildActivate += MdiParent_MdiChildActivate;
            }
            this.rtbxModel.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            this.rtbxModel.Encoding = Encoding.GetEncoding("UTF-8");

            this.rtbx_code.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            this.rtbx_code.Encoding = Encoding.GetEncoding("UTF-8");
        }
        protected override void OnClosed(EventArgs e)
        {
            this.MdiParent.MdiChildActivate -= MdiParent_MdiChildActivate;
            ((MdiParentForm)this.MdiParent).ORMCodeCreate -= btn_gena_Click;
            ((MdiParentForm)this.MdiParent).ORMCodeSave -= SaveORMCode;
        }
        private void MdiParent_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.MdiParent.ActiveMdiChild == null || this.MdiParent.ActiveMdiChild != this)
            {
                ((MdiParentForm)this.MdiParent).ORMCodeCreate -= btn_gena_Click;
                ((MdiParentForm)this.MdiParent).ORMCodeSave -= SaveORMCode;
            }
            else
            {
                ((MdiParentForm)this.MdiParent).ORMCodeCreate += btn_gena_Click;
                ((MdiParentForm)this.MdiParent).ORMCodeSave += SaveORMCode;
            }
        }

        private void btn_gena_Click(object sender, System.EventArgs e)
        {
            string[] creat = new string[this.ckbx_databaseobject.CheckedItems.Count];
            string tranSeq = "";
            for (int i = 0; i < creat.Length; i++)
            {
                creat[i] = this.ckbx_databaseobject.CheckedItems[i].ToString();
                tranSeq += ",\"" + creat[i] + "\"";
            }
            CodeGenerator codeGen = new CodeGenerator(CurrentDatabase.DataSource); 
            string[] Code = codeGen.Generate_Code(creat);
            this.rtbx_code.Text = Code[0];
            this.rtbxModel.Text = Code[1];
            this.tbc_databaseinfo.SelectedTab = this.tpgCommand;
        }

        private void SaveORMCode(object sender, System.EventArgs e)
        {

            SaveFileDialog  SaveDlg = new SaveFileDialog();
            SaveDlg.DefaultExt = ".cs";
            SaveDlg.Filter = "指令(*.Cmd.cs),实体(*.Model.cs)|*.cs";
            //“文本文件(*.txt) | *.txt | 所有文件(*.*) | *.*””

            SaveDlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (SaveDlg.ShowDialog(this) == DialogResult.OK)
            {

                var sfi = new FileInfo(SaveDlg.FileName);
                var modelFile = sfi.FullName.Replace(sfi.Extension, "") + ".Model.cs";
                var cmdFile = sfi.FullName.Replace(sfi.Extension, "") + ".Cmd.cs";
                if (File.Exists(modelFile) )
                {
                    if(MessageBox.Show("File ["+ sfi.Name.Replace(sfi.Extension, "") + ".Model.cs] is exists,Replace it ?","File exists", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        File.Delete(modelFile);
                    } 
                }
                if (File.Exists(cmdFile))
                {
                    if (MessageBox.Show("File [" + sfi.Name.Replace(sfi.Extension, "") + ".Cmd.cs] is exists,Replace it ?", "File exists", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        File.Delete(cmdFile);
                    }
                }
                if (!sfi.Directory.Exists)
                    sfi.Directory.Create();

                File.WriteAllText(Path.Combine(cmdFile), rtbx_code.Text);
                File.WriteAllText(Path.Combine(modelFile), rtbxModel.Text); 
            }
        }

        private void cbx_table_CheckStateChanged(object sender, System.EventArgs e)
        {
            if (sender.Equals(this.cbx_table))
            {
                if (this.cbx_table.Checked)
                {
                    this.ckbx_databaseobject.Items.AddRange(CurrentDatabase.DataSource.GetUserTables());
                }
                else
                {
                    string[] tables = CurrentDatabase.DataSource.GetUserTables();
                    for (int i = 0; i < tables.Length; i++)
                    {
                        this.ckbx_databaseobject.Items.Remove(tables[i]);
                    }
                }
            }

            if (sender.Equals(this.cbx_veiw))
            {
                if (this.cbx_veiw.Checked)
                {
                    this.ckbx_databaseobject.Items.AddRange(CurrentDatabase.DataSource.GetUserViews());
                }
                else
                {
                    string[] tables = CurrentDatabase.DataSource.GetUserViews();
                    for (int i = 0; i < tables.Length; i++)
                    {
                        this.ckbx_databaseobject.Items.Remove(tables[i]);
                    }
                }
            }
            this.ckbx_databaseobject.Refresh();
        }

        private void cbx_selectall_CheckStateChanged(object sender, System.EventArgs e)
        {
            for (int i = 0; i < this.ckbx_databaseobject.Items.Count; i++)
            {
                this.ckbx_databaseobject.SetItemCheckState(i, this.cbx_selectall.CheckState);
            }
            this.ckbx_databaseobject.Update();
            this.ckbx_databaseobject.Refresh();
        }
    }
}
