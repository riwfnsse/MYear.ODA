namespace MYear.Demo
{
    partial class Demo
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fplDemoFunc = new System.Windows.Forms.FlowLayoutPanel();
            this.plFuncType = new System.Windows.Forms.FlowLayoutPanel();
            this.tbpOutput = new System.Windows.Forms.TabControl();
            this.tbpSql = new System.Windows.Forms.TabPage();
            this.rtbxSql = new System.Windows.Forms.RichTextBox();
            this.tbpData = new System.Windows.Forms.TabPage();
            this.dgvData = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tbpOutput.SuspendLayout();
            this.tbpSql.SuspendLayout();
            this.tbpData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fplDemoFunc);
            this.splitContainer1.Panel1.Controls.Add(this.plFuncType);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbpOutput);
            this.splitContainer1.Size = new System.Drawing.Size(772, 471);
            this.splitContainer1.SplitterDistance = 235;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 17;
            // 
            // fplDemoFunc
            // 
            this.fplDemoFunc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fplDemoFunc.BackColor = System.Drawing.SystemColors.Info;
            this.fplDemoFunc.Location = new System.Drawing.Point(4, 45);
            this.fplDemoFunc.Name = "fplDemoFunc";
            this.fplDemoFunc.Size = new System.Drawing.Size(761, 187);
            this.fplDemoFunc.TabIndex = 20;
            // 
            // plFuncType
            // 
            this.plFuncType.Location = new System.Drawing.Point(3, 3);
            this.plFuncType.Name = "plFuncType";
            this.plFuncType.Size = new System.Drawing.Size(762, 36);
            this.plFuncType.TabIndex = 19;
            // 
            // tbpOutput
            // 
            this.tbpOutput.Controls.Add(this.tbpSql);
            this.tbpOutput.Controls.Add(this.tbpData);
            this.tbpOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbpOutput.Location = new System.Drawing.Point(0, 0);
            this.tbpOutput.Name = "tbpOutput";
            this.tbpOutput.SelectedIndex = 0;
            this.tbpOutput.Size = new System.Drawing.Size(772, 226);
            this.tbpOutput.TabIndex = 0;
            // 
            // tbpSql
            // 
            this.tbpSql.Controls.Add(this.rtbxSql);
            this.tbpSql.Location = new System.Drawing.Point(4, 22);
            this.tbpSql.Name = "tbpSql";
            this.tbpSql.Padding = new System.Windows.Forms.Padding(3);
            this.tbpSql.Size = new System.Drawing.Size(764, 200);
            this.tbpSql.TabIndex = 0;
            this.tbpSql.Text = "SQL Output";
            this.tbpSql.UseVisualStyleBackColor = true;
            // 
            // rtbxSql
            // 
            this.rtbxSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbxSql.Location = new System.Drawing.Point(3, 3);
            this.rtbxSql.Name = "rtbxSql";
            this.rtbxSql.Size = new System.Drawing.Size(758, 194);
            this.rtbxSql.TabIndex = 0;
            this.rtbxSql.Text = "";
            // 
            // tbpData
            // 
            this.tbpData.Controls.Add(this.dgvData);
            this.tbpData.Location = new System.Drawing.Point(4, 22);
            this.tbpData.Name = "tbpData";
            this.tbpData.Padding = new System.Windows.Forms.Padding(3);
            this.tbpData.Size = new System.Drawing.Size(764, 200);
            this.tbpData.TabIndex = 1;
            this.tbpData.Text = "Data";
            this.tbpData.UseVisualStyleBackColor = true;
            // 
            // dgvData
            // 
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Location = new System.Drawing.Point(3, 3);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(758, 194);
            this.dgvData.TabIndex = 2;
            // 
            // Demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 471);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Demo";
            this.Text = "Demo";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tbpOutput.ResumeLayout(false);
            this.tbpSql.ResumeLayout(false);
            this.tbpData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tbpOutput;
        private System.Windows.Forms.TabPage tbpSql;
        private System.Windows.Forms.TabPage tbpData;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.RichTextBox rtbxSql;
        private System.Windows.Forms.FlowLayoutPanel plFuncType;
        private System.Windows.Forms.FlowLayoutPanel fplDemoFunc;
    }
}

