namespace MYear.ODA.DevTool
{
    partial class SQLDevlop
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
            this.lbxTableView = new System.Windows.Forms.ListBox();
            this.CantainerSQLExecuting = new System.Windows.Forms.SplitContainer();
            this.tbc_ExecuteSqlResult = new System.Windows.Forms.TabControl();
            this.tpgGrid = new System.Windows.Forms.TabPage();
            this.dgvExecuteSql = new System.Windows.Forms.DataGridView();
            this.tpgMsg = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblExecuteRlt = new System.Windows.Forms.Label();
            this.rtbxSql = new ICSharpCode.TextEditor.TextEditorControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CantainerSQLExecuting)).BeginInit();
            this.CantainerSQLExecuting.Panel1.SuspendLayout();
            this.CantainerSQLExecuting.Panel2.SuspendLayout();
            this.CantainerSQLExecuting.SuspendLayout();
            this.tbc_ExecuteSqlResult.SuspendLayout();
            this.tpgGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExecuteSql)).BeginInit();
            this.tpgMsg.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.lbxTableView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.CantainerSQLExecuting);
            this.splitContainer1.Size = new System.Drawing.Size(815, 493);
            this.splitContainer1.SplitterDistance = 214;
            this.splitContainer1.TabIndex = 0;
            // 
            // lbxTableView
            // 
            this.lbxTableView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxTableView.FormattingEnabled = true;
            this.lbxTableView.ItemHeight = 12;
            this.lbxTableView.Location = new System.Drawing.Point(0, 0);
            this.lbxTableView.Name = "lbxTableView";
            this.lbxTableView.Size = new System.Drawing.Size(214, 493);
            this.lbxTableView.TabIndex = 0;
            this.lbxTableView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbxTableView_MouseDoubleClick);
            // 
            // CantainerSQLExecuting
            // 
            this.CantainerSQLExecuting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CantainerSQLExecuting.Location = new System.Drawing.Point(0, 0);
            this.CantainerSQLExecuting.Name = "CantainerSQLExecuting";
            this.CantainerSQLExecuting.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // CantainerSQLExecuting.Panel1
            // 
            this.CantainerSQLExecuting.Panel1.Controls.Add(this.rtbxSql);
            // 
            // CantainerSQLExecuting.Panel2
            // 
            this.CantainerSQLExecuting.Panel2.Controls.Add(this.tbc_ExecuteSqlResult);
            this.CantainerSQLExecuting.Size = new System.Drawing.Size(597, 493);
            this.CantainerSQLExecuting.SplitterDistance = 199;
            this.CantainerSQLExecuting.SplitterIncrement = 5;
            this.CantainerSQLExecuting.SplitterWidth = 6;
            this.CantainerSQLExecuting.TabIndex = 0;
            // 
            // tbc_ExecuteSqlResult
            // 
            this.tbc_ExecuteSqlResult.Controls.Add(this.tpgGrid);
            this.tbc_ExecuteSqlResult.Controls.Add(this.tpgMsg);
            this.tbc_ExecuteSqlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbc_ExecuteSqlResult.Location = new System.Drawing.Point(0, 0);
            this.tbc_ExecuteSqlResult.Name = "tbc_ExecuteSqlResult";
            this.tbc_ExecuteSqlResult.SelectedIndex = 0;
            this.tbc_ExecuteSqlResult.Size = new System.Drawing.Size(597, 288);
            this.tbc_ExecuteSqlResult.TabIndex = 2;
            // 
            // tpgGrid
            // 
            this.tpgGrid.Controls.Add(this.dgvExecuteSql);
            this.tpgGrid.Location = new System.Drawing.Point(4, 22);
            this.tpgGrid.Name = "tpgGrid";
            this.tpgGrid.Padding = new System.Windows.Forms.Padding(3);
            this.tpgGrid.Size = new System.Drawing.Size(589, 262);
            this.tpgGrid.TabIndex = 0;
            this.tpgGrid.Text = "Grid";
            this.tpgGrid.UseVisualStyleBackColor = true;
            // 
            // dgvExecuteSql
            // 
            this.dgvExecuteSql.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExecuteSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvExecuteSql.Location = new System.Drawing.Point(3, 3);
            this.dgvExecuteSql.Name = "dgvExecuteSql";
            this.dgvExecuteSql.RowTemplate.Height = 24;
            this.dgvExecuteSql.ShowCellErrors = false;
            this.dgvExecuteSql.Size = new System.Drawing.Size(583, 256);
            this.dgvExecuteSql.TabIndex = 0;
            this.dgvExecuteSql.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvExecuteSql_DataError);
            // 
            // tpgMsg
            // 
            this.tpgMsg.Controls.Add(this.panel1);
            this.tpgMsg.Location = new System.Drawing.Point(4, 22);
            this.tpgMsg.Name = "tpgMsg";
            this.tpgMsg.Padding = new System.Windows.Forms.Padding(3);
            this.tpgMsg.Size = new System.Drawing.Size(589, 262);
            this.tpgMsg.TabIndex = 1;
            this.tpgMsg.Text = "Message";
            this.tpgMsg.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.lblExecuteRlt);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(586, 257);
            this.panel1.TabIndex = 4;
            // 
            // lblExecuteRlt
            // 
            this.lblExecuteRlt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExecuteRlt.ForeColor = System.Drawing.Color.Red;
            this.lblExecuteRlt.Location = new System.Drawing.Point(0, 0);
            this.lblExecuteRlt.Name = "lblExecuteRlt";
            this.lblExecuteRlt.Size = new System.Drawing.Size(586, 257);
            this.lblExecuteRlt.TabIndex = 2;
            this.lblExecuteRlt.Text = "Message";
            // 
            // rtbxSql
            // 
            this.rtbxSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbxSql.IsReadOnly = false;
            this.rtbxSql.Location = new System.Drawing.Point(0, 0);
            this.rtbxSql.Name = "rtbxSql";
            this.rtbxSql.Size = new System.Drawing.Size(597, 199);
            this.rtbxSql.TabIndex = 3;
            this.rtbxSql.Text = "SELECT";
            // 
            // SQLDevlop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 493);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SQLDevlop";
            this.Text = "SQLDev";
            this.Load += new System.EventHandler(this.SQLDevlop_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.CantainerSQLExecuting.Panel1.ResumeLayout(false);
            this.CantainerSQLExecuting.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CantainerSQLExecuting)).EndInit();
            this.CantainerSQLExecuting.ResumeLayout(false);
            this.tbc_ExecuteSqlResult.ResumeLayout(false);
            this.tpgGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExecuteSql)).EndInit();
            this.tpgMsg.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer CantainerSQLExecuting;
        private System.Windows.Forms.TabControl tbc_ExecuteSqlResult;
        private System.Windows.Forms.TabPage tpgGrid;
        private System.Windows.Forms.DataGridView dgvExecuteSql;
        private System.Windows.Forms.TabPage tpgMsg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblExecuteRlt;
        private System.Windows.Forms.ListBox lbxTableView;
        private ICSharpCode.TextEditor.TextEditorControl rtbxSql;
    }
}