namespace MYear.ODA.DevTool
{
    partial class ORMCode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tpgModel = new System.Windows.Forms.TabPage();
            this.rtbxModel = new ICSharpCode.TextEditor.TextEditorControl();
            this.tpgCommand = new System.Windows.Forms.TabPage();
            this.rtbx_code = new ICSharpCode.TextEditor.TextEditorControl();
            this.tpgTableView = new System.Windows.Forms.TabPage();
            this.ckbx_databaseobject = new System.Windows.Forms.CheckedListBox();
            this.btn_gena = new System.Windows.Forms.Button();
            this.cbx_selectall = new System.Windows.Forms.CheckBox();
            this.cbx_veiw = new System.Windows.Forms.CheckBox();
            this.cbx_table = new System.Windows.Forms.CheckBox();
            this.tbc_databaseinfo = new System.Windows.Forms.TabControl();
            this.tpgCtx = new System.Windows.Forms.TabPage();
            this.rtbxCtx = new ICSharpCode.TextEditor.TextEditorControl();
            this.tpgModel.SuspendLayout();
            this.tpgCommand.SuspendLayout();
            this.tpgTableView.SuspendLayout();
            this.tbc_databaseinfo.SuspendLayout();
            this.tpgCtx.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpgModel
            // 
            this.tpgModel.Controls.Add(this.rtbxModel);
            this.tpgModel.Location = new System.Drawing.Point(4, 22);
            this.tpgModel.Name = "tpgModel";
            this.tpgModel.Size = new System.Drawing.Size(877, 450);
            this.tpgModel.TabIndex = 2;
            this.tpgModel.Text = "Model";
            this.tpgModel.UseVisualStyleBackColor = true;
            // 
            // rtbxModel
            // 
            this.rtbxModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbxModel.IsReadOnly = false;
            this.rtbxModel.Location = new System.Drawing.Point(0, 0);
            this.rtbxModel.Name = "rtbxModel";
            this.rtbxModel.Size = new System.Drawing.Size(877, 450);
            this.rtbxModel.TabIndex = 6;
            // 
            // tpgCommand
            // 
            this.tpgCommand.Controls.Add(this.rtbx_code);
            this.tpgCommand.Location = new System.Drawing.Point(4, 22);
            this.tpgCommand.Name = "tpgCommand";
            this.tpgCommand.Size = new System.Drawing.Size(877, 450);
            this.tpgCommand.TabIndex = 1;
            this.tpgCommand.Text = "Command";
            this.tpgCommand.UseVisualStyleBackColor = true;
            // 
            // rtbx_code
            // 
            this.rtbx_code.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbx_code.IsReadOnly = false;
            this.rtbx_code.Location = new System.Drawing.Point(0, 0);
            this.rtbx_code.Name = "rtbx_code";
            this.rtbx_code.Size = new System.Drawing.Size(877, 450);
            this.rtbx_code.TabIndex = 4;
            // 
            // tpgTableView
            // 
            this.tpgTableView.Controls.Add(this.ckbx_databaseobject);
            this.tpgTableView.Controls.Add(this.btn_gena);
            this.tpgTableView.Controls.Add(this.cbx_selectall);
            this.tpgTableView.Controls.Add(this.cbx_veiw);
            this.tpgTableView.Controls.Add(this.cbx_table);
            this.tpgTableView.Location = new System.Drawing.Point(4, 22);
            this.tpgTableView.Name = "tpgTableView";
            this.tpgTableView.Size = new System.Drawing.Size(877, 450);
            this.tpgTableView.TabIndex = 0;
            this.tpgTableView.Text = "Table View";
            this.tpgTableView.UseVisualStyleBackColor = true;
            // 
            // ckbx_databaseobject
            // 
            this.ckbx_databaseobject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbx_databaseobject.ColumnWidth = 240;
            this.ckbx_databaseobject.Location = new System.Drawing.Point(0, 40);
            this.ckbx_databaseobject.MultiColumn = true;
            this.ckbx_databaseobject.Name = "ckbx_databaseobject";
            this.ckbx_databaseobject.Size = new System.Drawing.Size(875, 404);
            this.ckbx_databaseobject.TabIndex = 14;
            // 
            // btn_gena
            // 
            this.btn_gena.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_gena.Location = new System.Drawing.Point(771, 8);
            this.btn_gena.Name = "btn_gena";
            this.btn_gena.Size = new System.Drawing.Size(96, 23);
            this.btn_gena.TabIndex = 13;
            this.btn_gena.Text = "Generate  Code";
            this.btn_gena.Click += new System.EventHandler(this.btn_gena_Click);
            // 
            // cbx_selectall
            // 
            this.cbx_selectall.Location = new System.Drawing.Point(262, 8);
            this.cbx_selectall.Name = "cbx_selectall";
            this.cbx_selectall.Size = new System.Drawing.Size(104, 24);
            this.cbx_selectall.TabIndex = 12;
            this.cbx_selectall.Text = "Select All";
            this.cbx_selectall.CheckedChanged += new System.EventHandler(this.cbx_selectall_CheckStateChanged);
            // 
            // cbx_veiw
            // 
            this.cbx_veiw.Checked = true;
            this.cbx_veiw.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbx_veiw.Location = new System.Drawing.Point(152, 8);
            this.cbx_veiw.Name = "cbx_veiw";
            this.cbx_veiw.Size = new System.Drawing.Size(104, 24);
            this.cbx_veiw.TabIndex = 10;
            this.cbx_veiw.Text = "System View";
            this.cbx_veiw.CheckedChanged += new System.EventHandler(this.cbx_table_CheckStateChanged);
            // 
            // cbx_table
            // 
            this.cbx_table.Checked = true;
            this.cbx_table.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbx_table.Location = new System.Drawing.Point(24, 8);
            this.cbx_table.Name = "cbx_table";
            this.cbx_table.Size = new System.Drawing.Size(104, 24);
            this.cbx_table.TabIndex = 9;
            this.cbx_table.Text = "System Table";
            this.cbx_table.CheckedChanged += new System.EventHandler(this.cbx_table_CheckStateChanged);
            // 
            // tbc_databaseinfo
            // 
            this.tbc_databaseinfo.Controls.Add(this.tpgTableView);
            this.tbc_databaseinfo.Controls.Add(this.tpgCtx);
            this.tbc_databaseinfo.Controls.Add(this.tpgCommand);
            this.tbc_databaseinfo.Controls.Add(this.tpgModel);
            this.tbc_databaseinfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbc_databaseinfo.Location = new System.Drawing.Point(0, 0);
            this.tbc_databaseinfo.Name = "tbc_databaseinfo";
            this.tbc_databaseinfo.SelectedIndex = 0;
            this.tbc_databaseinfo.Size = new System.Drawing.Size(885, 476);
            this.tbc_databaseinfo.TabIndex = 20;
            // 
            // tpgCtx
            // 
            this.tpgCtx.Controls.Add(this.rtbxCtx);
            this.tpgCtx.Location = new System.Drawing.Point(4, 22);
            this.tpgCtx.Name = "tpgCtx";
            this.tpgCtx.Size = new System.Drawing.Size(877, 450);
            this.tpgCtx.TabIndex = 3;
            this.tpgCtx.Text = "Context";
            this.tpgCtx.UseVisualStyleBackColor = true;
            // 
            // rtbxCtx
            // 
            this.rtbxCtx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbxCtx.IsReadOnly = false;
            this.rtbxCtx.Location = new System.Drawing.Point(0, 0);
            this.rtbxCtx.Name = "rtbxCtx";
            this.rtbxCtx.Size = new System.Drawing.Size(877, 450);
            this.rtbxCtx.TabIndex = 5;
            // 
            // ORMCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 476);
            this.Controls.Add(this.tbc_databaseinfo);
            this.Name = "ORMCode";
            this.Text = "ORMCodeGen";
            this.Load += new System.EventHandler(this.ORMCode_Load);
            this.tpgModel.ResumeLayout(false);
            this.tpgCommand.ResumeLayout(false);
            this.tpgTableView.ResumeLayout(false);
            this.tbc_databaseinfo.ResumeLayout(false);
            this.tpgCtx.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tpgModel;
        private ICSharpCode.TextEditor.TextEditorControl rtbxModel;
        private System.Windows.Forms.TabPage tpgCommand;
        private ICSharpCode.TextEditor.TextEditorControl rtbx_code;
        private System.Windows.Forms.TabPage tpgTableView;
        private System.Windows.Forms.CheckedListBox ckbx_databaseobject;
        private System.Windows.Forms.Button btn_gena;
        private System.Windows.Forms.CheckBox cbx_selectall;
        private System.Windows.Forms.CheckBox cbx_veiw;
        private System.Windows.Forms.CheckBox cbx_table;
        private System.Windows.Forms.TabControl tbc_databaseinfo;
        private System.Windows.Forms.TabPage tpgCtx;
        private ICSharpCode.TextEditor.TextEditorControl rtbxCtx;
    }
}