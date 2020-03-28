namespace MYear.ODA.DevTool
{
    partial class SessionStart
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
            this.btn_connect = new System.Windows.Forms.Button();
            this.lbl_connect_string = new System.Windows.Forms.Label();
            this.cbbx_database = new System.Windows.Forms.ComboBox();
            this.lbl_database = new System.Windows.Forms.Label();
            this.lblExecuteRlt = new System.Windows.Forms.Label();
            this.cbbxConnectstring = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btn_connect
            // 
            this.btn_connect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_connect.Location = new System.Drawing.Point(543, 2);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(75, 48);
            this.btn_connect.TabIndex = 21;
            this.btn_connect.Text = "Connect";
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // lbl_connect_string
            // 
            this.lbl_connect_string.Location = new System.Drawing.Point(12, 29);
            this.lbl_connect_string.Name = "lbl_connect_string";
            this.lbl_connect_string.Size = new System.Drawing.Size(81, 23);
            this.lbl_connect_string.TabIndex = 25;
            this.lbl_connect_string.Text = "Connect String";
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
            this.cbbx_database.Location = new System.Drawing.Point(114, 6);
            this.cbbx_database.Name = "cbbx_database";
            this.cbbx_database.Size = new System.Drawing.Size(121, 20);
            this.cbbx_database.TabIndex = 23;
            this.cbbx_database.SelectedIndexChanged += new System.EventHandler(this.cbbx_database_SelectedIndexChanged);
            // 
            // lbl_database
            // 
            this.lbl_database.Location = new System.Drawing.Point(12, 9);
            this.lbl_database.Name = "lbl_database";
            this.lbl_database.Size = new System.Drawing.Size(96, 20);
            this.lbl_database.TabIndex = 22;
            this.lbl_database.Text = "DataBase";
            // 
            // lblExecuteRlt
            // 
            this.lblExecuteRlt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExecuteRlt.ForeColor = System.Drawing.Color.Red;
            this.lblExecuteRlt.Location = new System.Drawing.Point(0, 53);
            this.lblExecuteRlt.Name = "lblExecuteRlt";
            this.lblExecuteRlt.Size = new System.Drawing.Size(618, 165);
            this.lblExecuteRlt.TabIndex = 26;
            this.lblExecuteRlt.Text = "Message";
            // 
            // cbbxConnectstring
            // 
            this.cbbxConnectstring.FormattingEnabled = true;
            this.cbbxConnectstring.Location = new System.Drawing.Point(114, 30);
            this.cbbxConnectstring.Name = "cbbxConnectstring";
            this.cbbxConnectstring.Size = new System.Drawing.Size(423, 20);
            this.cbbxConnectstring.TabIndex = 27;
            this.cbbxConnectstring.Text = "Connecting String...";
            // 
            // SessionStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 219);
            this.Controls.Add(this.cbbxConnectstring);
            this.Controls.Add(this.lblExecuteRlt);
            this.Controls.Add(this.btn_connect);
            this.Controls.Add(this.lbl_connect_string);
            this.Controls.Add(this.cbbx_database);
            this.Controls.Add(this.lbl_database);
            this.Name = "SessionStart";
            this.Text = "Session";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Label lbl_connect_string;
        private System.Windows.Forms.ComboBox cbbx_database;
        private System.Windows.Forms.Label lbl_database;
        private System.Windows.Forms.Label lblExecuteRlt;
        private System.Windows.Forms.ComboBox cbbxConnectstring;
    }
}