namespace MYear.ODA.DevTool
{
    partial class ToolMain
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolMain));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mncNewConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mncORMCreateWm = new System.Windows.Forms.ToolStripMenuItem();
            this.mncDBCopyWm = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mncExcuteSQL = new System.Windows.Forms.ToolStripMenuItem();
            this.mncORMCodeCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.mncORMCodeSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mncDBConnectTest = new System.Windows.Forms.ToolStripMenuItem();
            this.mncDBCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arrangeIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlbrExecuteSQL = new System.Windows.Forms.ToolStripButton();
            this.tlbrORMCodeCreate = new System.Windows.Forms.ToolStripButton();
            this.tlbrORMCodeSave = new System.Windows.Forms.ToolStripButton();
            this.tlbrDBConnectTest = new System.Windows.Forms.ToolStripButton();
            this.tlbrDBCopy = new System.Windows.Forms.ToolStripButton();
            this.tlbrDBRefresh = new System.Windows.Forms.ToolStripButton();
            this.mncQueryWm = new System.Windows.Forms.ToolStripMenuItem();
            this.mncRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.viewMenu,
            this.windowsMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.MdiWindowListItem = this.windowsMenu;
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(632, 25);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mncNewConnect,
            this.toolStripSeparator5,
            this.mncQueryWm,
            this.mncORMCreateWm,
            this.mncDBCopyWm,
            this.toolStripSeparator3,
            this.mncExcuteSQL,
            this.mncRefresh,
            this.mncORMCodeCreate,
            this.mncORMCodeSave,
            this.mncDBConnectTest,
            this.mncDBCopy,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder;
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(58, 21);
            this.fileMenu.Text = "文件(&F)";
            // 
            // mncNewConnect
            // 
            this.mncNewConnect.Name = "mncNewConnect";
            this.mncNewConnect.Size = new System.Drawing.Size(199, 22);
            this.mncNewConnect.Text = "连接数据库";
            this.mncNewConnect.Click += new System.EventHandler(this.mncNewConnect_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(196, 6);
            // 
            // mncORMCreateWm
            // 
            this.mncORMCreateWm.Name = "mncORMCreateWm";
            this.mncORMCreateWm.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mncORMCreateWm.Size = new System.Drawing.Size(199, 22);
            this.mncORMCreateWm.Text = "ORM代码(&N)";
            this.mncORMCreateWm.Click += new System.EventHandler(this.ShowNewForm_ORMCode);
            // 
            // mncDBCopyWm
            // 
            this.mncDBCopyWm.Name = "mncDBCopyWm";
            this.mncDBCopyWm.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.mncDBCopyWm.Size = new System.Drawing.Size(199, 22);
            this.mncDBCopyWm.Text = "数据库复制(&D)";
            this.mncDBCopyWm.Click += new System.EventHandler(this.ShowNewForm_DBCopy);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(196, 6);
            // 
            // mncExcuteSQL
            // 
            this.mncExcuteSQL.Name = "mncExcuteSQL";
            this.mncExcuteSQL.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.mncExcuteSQL.Size = new System.Drawing.Size(199, 22);
            this.mncExcuteSQL.Text = "执行";
            // 
            // mncORMCodeCreate
            // 
            this.mncORMCodeCreate.Name = "mncORMCodeCreate";
            this.mncORMCodeCreate.Size = new System.Drawing.Size(199, 22);
            this.mncORMCodeCreate.Text = "生成";
            // 
            // mncORMCodeSave
            // 
            this.mncORMCodeSave.ImageTransparentColor = System.Drawing.Color.Black;
            this.mncORMCodeSave.Name = "mncORMCodeSave";
            this.mncORMCodeSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mncORMCodeSave.Size = new System.Drawing.Size(199, 22);
            this.mncORMCodeSave.Text = "保存(&S)";
            // 
            // mncDBConnectTest
            // 
            this.mncDBConnectTest.Name = "mncDBConnectTest";
            this.mncDBConnectTest.Size = new System.Drawing.Size(199, 22);
            this.mncDBConnectTest.Text = "连接测试";
            // 
            // mncDBCopy
            // 
            this.mncDBCopy.Name = "mncDBCopy";
            this.mncDBCopy.Size = new System.Drawing.Size(199, 22);
            this.mncDBCopy.Text = "数据库复制";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(196, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.exitToolStripMenuItem.Text = "退出(&X)";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolsStripMenuItem_Click);
            // 
            // viewMenu
            // 
            this.viewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBarToolStripMenuItem,
            this.statusBarToolStripMenuItem});
            this.viewMenu.Name = "viewMenu";
            this.viewMenu.Size = new System.Drawing.Size(60, 21);
            this.viewMenu.Text = "视图(&V)";
            // 
            // toolBarToolStripMenuItem
            // 
            this.toolBarToolStripMenuItem.Checked = true;
            this.toolBarToolStripMenuItem.CheckOnClick = true;
            this.toolBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolBarToolStripMenuItem.Name = "toolBarToolStripMenuItem";
            this.toolBarToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.toolBarToolStripMenuItem.Text = "工具栏(&T)";
            this.toolBarToolStripMenuItem.Click += new System.EventHandler(this.ToolBarToolStripMenuItem_Click);
            // 
            // statusBarToolStripMenuItem
            // 
            this.statusBarToolStripMenuItem.Checked = true;
            this.statusBarToolStripMenuItem.CheckOnClick = true;
            this.statusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusBarToolStripMenuItem.Name = "statusBarToolStripMenuItem";
            this.statusBarToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.statusBarToolStripMenuItem.Text = "状态栏(&S)";
            this.statusBarToolStripMenuItem.Click += new System.EventHandler(this.StatusBarToolStripMenuItem_Click);
            // 
            // windowsMenu
            // 
            this.windowsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newWindowToolStripMenuItem,
            this.cascadeToolStripMenuItem,
            this.tileVerticalToolStripMenuItem,
            this.tileHorizontalToolStripMenuItem,
            this.closeAllToolStripMenuItem,
            this.arrangeIconsToolStripMenuItem});
            this.windowsMenu.Name = "windowsMenu";
            this.windowsMenu.Size = new System.Drawing.Size(64, 21);
            this.windowsMenu.Text = "窗口(&W)";
            // 
            // newWindowToolStripMenuItem
            // 
            this.newWindowToolStripMenuItem.Name = "newWindowToolStripMenuItem";
            this.newWindowToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.newWindowToolStripMenuItem.Text = "新建窗口(&N)";
            this.newWindowToolStripMenuItem.Click += new System.EventHandler(this.ShowNewForm_SQLDevlop);
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.cascadeToolStripMenuItem.Text = "层叠(&C)";
            this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.CascadeToolStripMenuItem_Click);
            // 
            // tileVerticalToolStripMenuItem
            // 
            this.tileVerticalToolStripMenuItem.Name = "tileVerticalToolStripMenuItem";
            this.tileVerticalToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.tileVerticalToolStripMenuItem.Text = "垂直平铺(&V)";
            this.tileVerticalToolStripMenuItem.Click += new System.EventHandler(this.TileVerticalToolStripMenuItem_Click);
            // 
            // tileHorizontalToolStripMenuItem
            // 
            this.tileHorizontalToolStripMenuItem.Name = "tileHorizontalToolStripMenuItem";
            this.tileHorizontalToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.tileHorizontalToolStripMenuItem.Text = "水平平铺(&H)";
            this.tileHorizontalToolStripMenuItem.Click += new System.EventHandler(this.TileHorizontalToolStripMenuItem_Click);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.closeAllToolStripMenuItem.Text = "全部关闭(&L)";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.CloseAllToolStripMenuItem_Click);
            // 
            // arrangeIconsToolStripMenuItem
            // 
            this.arrangeIconsToolStripMenuItem.Name = "arrangeIconsToolStripMenuItem";
            this.arrangeIconsToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.arrangeIconsToolStripMenuItem.Text = "排列图标(&A)";
            this.arrangeIconsToolStripMenuItem.Click += new System.EventHandler(this.ArrangeIconsToolStripMenuItem_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbrExecuteSQL,
            this.tlbrDBRefresh,
            this.toolStripSeparator2,
            this.tlbrORMCodeCreate,
            this.tlbrORMCodeSave,
            this.toolStripSeparator1,
            this.tlbrDBConnectTest,
            this.tlbrDBCopy});
            this.toolStrip.Location = new System.Drawing.Point(0, 25);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(632, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "ToolStrip";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 396);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(632, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel.Text = "状态";
            // 
            // tlbrExecuteSQL
            // 
            this.tlbrExecuteSQL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbrExecuteSQL.Image = global::MYear.ODA.DevTool.Properties.Resources.Properties_16x16;
            this.tlbrExecuteSQL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbrExecuteSQL.Name = "tlbrExecuteSQL";
            this.tlbrExecuteSQL.Size = new System.Drawing.Size(23, 22);
            this.tlbrExecuteSQL.Text = "执行";
            // 
            // tlbrORMCodeCreate
            // 
            this.tlbrORMCodeCreate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbrORMCodeCreate.Image = global::MYear.ODA.DevTool.Properties.Resources.Scripts_16x16;
            this.tlbrORMCodeCreate.ImageTransparentColor = System.Drawing.Color.Black;
            this.tlbrORMCodeCreate.Name = "tlbrORMCodeCreate";
            this.tlbrORMCodeCreate.Size = new System.Drawing.Size(23, 22);
            this.tlbrORMCodeCreate.Text = "生成ORM代码";
            // 
            // tlbrORMCodeSave
            // 
            this.tlbrORMCodeSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbrORMCodeSave.Image = global::MYear.ODA.DevTool.Properties.Resources.SaveTo_16x16;
            this.tlbrORMCodeSave.ImageTransparentColor = System.Drawing.Color.Black;
            this.tlbrORMCodeSave.Name = "tlbrORMCodeSave";
            this.tlbrORMCodeSave.Size = new System.Drawing.Size(23, 22);
            this.tlbrORMCodeSave.Text = "保存代码";
            // 
            // tlbrDBConnectTest
            // 
            this.tlbrDBConnectTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbrDBConnectTest.Image = global::MYear.ODA.DevTool.Properties.Resources.BugReport_16x16;
            this.tlbrDBConnectTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbrDBConnectTest.Name = "tlbrDBConnectTest";
            this.tlbrDBConnectTest.Size = new System.Drawing.Size(23, 22);
            this.tlbrDBConnectTest.Text = "连接测试";
            // 
            // tlbrDBCopy
            // 
            this.tlbrDBCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbrDBCopy.Image = global::MYear.ODA.DevTool.Properties.Resources.SelectDataMember_16x16;
            this.tlbrDBCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbrDBCopy.Name = "tlbrDBCopy";
            this.tlbrDBCopy.Size = new System.Drawing.Size(23, 22);
            this.tlbrDBCopy.Text = "数据库复制";
            // 
            // tlbrDBRefresh
            // 
            this.tlbrDBRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbrDBRefresh.Image = global::MYear.ODA.DevTool.Properties.Resources.database_refresh;
            this.tlbrDBRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbrDBRefresh.Name = "tlbrDBRefresh";
            this.tlbrDBRefresh.Size = new System.Drawing.Size(23, 22);
            this.tlbrDBRefresh.Text = "Refresh";
            // 
            // mncQueryWm
            // 
            this.mncQueryWm.Image = ((System.Drawing.Image)(resources.GetObject("mncQueryWm.Image")));
            this.mncQueryWm.Name = "mncQueryWm";
            this.mncQueryWm.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.mncQueryWm.Size = new System.Drawing.Size(199, 22);
            this.mncQueryWm.Text = "SQL查询(&Q)";
            this.mncQueryWm.Click += new System.EventHandler(this.ShowNewForm_SQLDevlop);
            // 
            // mncRefresh
            // 
            this.mncRefresh.Name = "mncRefresh";
            this.mncRefresh.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mncRefresh.Size = new System.Drawing.Size(199, 22);
            this.mncRefresh.Text = "刷新";
            // 
            // ToolMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 418);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "ToolMain";
            this.Text = "ODA开发工具";
            this.Shown += new System.EventHandler(this.ToolMain_Shown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem tileHorizontalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem mncORMCodeSave;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMenu;
        private System.Windows.Forms.ToolStripMenuItem toolBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsMenu;
        private System.Windows.Forms.ToolStripMenuItem newWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cascadeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileVerticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arrangeIconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tlbrORMCodeSave;
        private System.Windows.Forms.ToolStripButton tlbrORMCodeCreate;
        private System.Windows.Forms.ToolStripMenuItem mncQueryWm;
        private System.Windows.Forms.ToolStripMenuItem mncORMCreateWm;
        private System.Windows.Forms.ToolStripMenuItem mncDBCopyWm;
        private System.Windows.Forms.ToolStripMenuItem mncExcuteSQL;
        private System.Windows.Forms.ToolStripMenuItem mncORMCodeCreate;
        private System.Windows.Forms.ToolStripButton tlbrExecuteSQL;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tlbrDBConnectTest;
        private System.Windows.Forms.ToolStripButton tlbrDBCopy;
        private System.Windows.Forms.ToolStripMenuItem mncDBConnectTest;
        private System.Windows.Forms.ToolStripMenuItem mncDBCopy;
        private System.Windows.Forms.ToolStripMenuItem mncNewConnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tlbrDBRefresh;
        private System.Windows.Forms.ToolStripMenuItem mncRefresh;
    }
}



