namespace MYear.ODA.DevTool
{
    partial class DBCopy
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cbx_selectall = new System.Windows.Forms.CheckBox();
            this.ckbxDatabaseobject = new System.Windows.Forms.CheckedListBox();
            this.Target = new System.Windows.Forms.GroupBox();
            this.cbxTableScript = new System.Windows.Forms.CheckBox();
            this.gbxInformation = new System.Windows.Forms.GroupBox();
            this.pnlTranStatus = new System.Windows.Forms.Panel();
            this.lblTransData = new System.Windows.Forms.Label();
            this.lblTransTable = new System.Windows.Forms.Label();
            this.pgbData = new System.Windows.Forms.ProgressBar();
            this.pgbTable = new System.Windows.Forms.ProgressBar();
            this.ckbxTarDB = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxCreateTable = new System.Windows.Forms.CheckBox();
            this.cbxTransData = new System.Windows.Forms.CheckBox();
            this.tbx_connectstring = new System.Windows.Forms.TextBox();
            this.cbbx_database = new System.Windows.Forms.ComboBox();
            this.lbl_database = new System.Windows.Forms.Label();
            this.lbl_connect_string = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.Target.SuspendLayout();
            this.gbxInformation.SuspendLayout();
            this.pnlTranStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cbx_selectall);
            this.splitContainer1.Panel1.Controls.Add(this.ckbxDatabaseobject);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Target);
            this.splitContainer1.Size = new System.Drawing.Size(839, 404);
            this.splitContainer1.SplitterDistance = 228;
            this.splitContainer1.TabIndex = 0;
            // 
            // cbx_selectall
            // 
            this.cbx_selectall.Location = new System.Drawing.Point(6, 4);
            this.cbx_selectall.Name = "cbx_selectall";
            this.cbx_selectall.Size = new System.Drawing.Size(104, 24);
            this.cbx_selectall.TabIndex = 16;
            this.cbx_selectall.Text = "Select All";
            this.cbx_selectall.CheckedChanged += new System.EventHandler(this.cbx_selectall_CheckStateChanged);
            // 
            // ckbxDatabaseobject
            // 
            this.ckbxDatabaseobject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbxDatabaseobject.ColumnWidth = 240;
            this.ckbxDatabaseobject.Location = new System.Drawing.Point(3, 29);
            this.ckbxDatabaseobject.Name = "ckbxDatabaseobject";
            this.ckbxDatabaseobject.Size = new System.Drawing.Size(222, 372);
            this.ckbxDatabaseobject.TabIndex = 15;
            // 
            // Target
            // 
            this.Target.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Target.Controls.Add(this.cbxTableScript);
            this.Target.Controls.Add(this.gbxInformation);
            this.Target.Controls.Add(this.label1);
            this.Target.Controls.Add(this.cbxCreateTable);
            this.Target.Controls.Add(this.cbxTransData);
            this.Target.Controls.Add(this.tbx_connectstring);
            this.Target.Controls.Add(this.cbbx_database);
            this.Target.Controls.Add(this.lbl_database);
            this.Target.Controls.Add(this.lbl_connect_string);
            this.Target.Location = new System.Drawing.Point(3, 3);
            this.Target.Name = "Target";
            this.Target.Size = new System.Drawing.Size(601, 398);
            this.Target.TabIndex = 35;
            this.Target.TabStop = false;
            this.Target.Text = "Target DataBase";
            // 
            // cbxTableScript
            // 
            this.cbxTableScript.Location = new System.Drawing.Point(86, 15);
            this.cbxTableScript.Name = "cbxTableScript";
            this.cbxTableScript.Size = new System.Drawing.Size(104, 24);
            this.cbxTableScript.TabIndex = 36;
            this.cbxTableScript.Text = "Table Script";
            // 
            // gbxInformation
            // 
            this.gbxInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxInformation.Controls.Add(this.pnlTranStatus);
            this.gbxInformation.Controls.Add(this.ckbxTarDB);
            this.gbxInformation.Location = new System.Drawing.Point(6, 106);
            this.gbxInformation.Name = "gbxInformation";
            this.gbxInformation.Size = new System.Drawing.Size(589, 292);
            this.gbxInformation.TabIndex = 18;
            this.gbxInformation.TabStop = false;
            this.gbxInformation.Text = "Information";
            // 
            // pnlTranStatus
            // 
            this.pnlTranStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTranStatus.Controls.Add(this.lblTransData);
            this.pnlTranStatus.Controls.Add(this.lblTransTable);
            this.pnlTranStatus.Controls.Add(this.pgbData);
            this.pnlTranStatus.Controls.Add(this.pgbTable);
            this.pnlTranStatus.Location = new System.Drawing.Point(6, 34);
            this.pnlTranStatus.Name = "pnlTranStatus";
            this.pnlTranStatus.Size = new System.Drawing.Size(577, 242);
            this.pnlTranStatus.TabIndex = 19;
            this.pnlTranStatus.Visible = false;
            // 
            // lblTransData
            // 
            this.lblTransData.AutoSize = true;
            this.lblTransData.Location = new System.Drawing.Point(4, 72);
            this.lblTransData.Name = "lblTransData";
            this.lblTransData.Size = new System.Drawing.Size(59, 12);
            this.lblTransData.TabIndex = 6;
            this.lblTransData.Text = "TransData";
            // 
            // lblTransTable
            // 
            this.lblTransTable.AutoSize = true;
            this.lblTransTable.Location = new System.Drawing.Point(4, 20);
            this.lblTransTable.Name = "lblTransTable";
            this.lblTransTable.Size = new System.Drawing.Size(65, 12);
            this.lblTransTable.TabIndex = 5;
            this.lblTransTable.Text = "TransTable";
            // 
            // pgbData
            // 
            this.pgbData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgbData.Location = new System.Drawing.Point(0, 87);
            this.pgbData.Name = "pgbData";
            this.pgbData.Size = new System.Drawing.Size(574, 23);
            this.pgbData.TabIndex = 4;
            // 
            // pgbTable
            // 
            this.pgbTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgbTable.Location = new System.Drawing.Point(0, 38);
            this.pgbTable.Name = "pgbTable";
            this.pgbTable.Size = new System.Drawing.Size(574, 23);
            this.pgbTable.TabIndex = 3;
            // 
            // ckbxTarDB
            // 
            this.ckbxTarDB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbxTarDB.ColumnWidth = 240;
            this.ckbxTarDB.Location = new System.Drawing.Point(6, 16);
            this.ckbxTarDB.MultiColumn = true;
            this.ckbxTarDB.Name = "ckbxTarDB";
            this.ckbxTarDB.Size = new System.Drawing.Size(577, 260);
            this.ckbxTarDB.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 20);
            this.label1.TabIndex = 35;
            this.label1.Text = "Copy Object";
            // 
            // cbxCreateTable
            // 
            this.cbxCreateTable.Location = new System.Drawing.Point(196, 15);
            this.cbxCreateTable.Name = "cbxCreateTable";
            this.cbxCreateTable.Size = new System.Drawing.Size(104, 24);
            this.cbxCreateTable.TabIndex = 32;
            this.cbxCreateTable.Text = "Table";
            // 
            // cbxTransData
            // 
            this.cbxTransData.Location = new System.Drawing.Point(305, 15);
            this.cbxTransData.Name = "cbxTransData";
            this.cbxTransData.Size = new System.Drawing.Size(104, 24);
            this.cbxTransData.TabIndex = 33;
            this.cbxTransData.Text = "Data";
            // 
            // tbx_connectstring
            // 
            this.tbx_connectstring.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbx_connectstring.Location = new System.Drawing.Point(86, 69);
            this.tbx_connectstring.Name = "tbx_connectstring";
            this.tbx_connectstring.Size = new System.Drawing.Size(515, 21);
            this.tbx_connectstring.TabIndex = 30;
            // 
            // cbbx_database
            // 
            this.cbbx_database.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbx_database.Items.AddRange(new object[] {
            "MsSQL",
            "MySql",
            "OdbcInformix",
            "OledbAccess",
            "Oracle",
            "Sybase",
            "SQLite",
            "DB2"});
            this.cbbx_database.Location = new System.Drawing.Point(86, 45);
            this.cbbx_database.Name = "cbbx_database";
            this.cbbx_database.Size = new System.Drawing.Size(121, 20);
            this.cbbx_database.TabIndex = 29;
            this.cbbx_database.SelectedIndexChanged += new System.EventHandler(this.cbbx_database_SelectedIndexChanged);
            // 
            // lbl_database
            // 
            this.lbl_database.Location = new System.Drawing.Point(10, 48);
            this.lbl_database.Name = "lbl_database";
            this.lbl_database.Size = new System.Drawing.Size(63, 20);
            this.lbl_database.TabIndex = 28;
            this.lbl_database.Text = "DataBase";
            // 
            // lbl_connect_string
            // 
            this.lbl_connect_string.Location = new System.Drawing.Point(10, 72);
            this.lbl_connect_string.Name = "lbl_connect_string";
            this.lbl_connect_string.Size = new System.Drawing.Size(59, 23);
            this.lbl_connect_string.TabIndex = 31;
            this.lbl_connect_string.Text = "Connect String";
            // 
            // DBCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 404);
            this.Controls.Add(this.splitContainer1);
            this.Name = "DBCopy";
            this.Text = "DB Copy";
            this.Load += new System.EventHandler(this.DBCopy_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.Target.ResumeLayout(false);
            this.Target.PerformLayout();
            this.gbxInformation.ResumeLayout(false);
            this.pnlTranStatus.ResumeLayout(false);
            this.pnlTranStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox ckbxDatabaseobject;
        private System.Windows.Forms.CheckBox cbx_selectall;
        private System.Windows.Forms.Label lbl_database;
        private System.Windows.Forms.Label lbl_connect_string;
        private System.Windows.Forms.ComboBox cbbx_database;
        private System.Windows.Forms.TextBox tbx_connectstring;
        private System.Windows.Forms.CheckBox cbxTransData;
        private System.Windows.Forms.CheckBox cbxCreateTable;
        private System.Windows.Forms.GroupBox Target;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbxInformation;
        private System.Windows.Forms.Panel pnlTranStatus;
        private System.Windows.Forms.Label lblTransData;
        private System.Windows.Forms.Label lblTransTable;
        private System.Windows.Forms.ProgressBar pgbData;
        private System.Windows.Forms.ProgressBar pgbTable;
        private System.Windows.Forms.CheckedListBox ckbxTarDB;
        private System.Windows.Forms.CheckBox cbxTableScript;
    }
}