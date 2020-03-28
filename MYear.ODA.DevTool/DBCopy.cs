using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MYear.ODA.DevTool
{
    public partial class DBCopy : Form
    {
        private ODA.DBAccess TarDB = null;
        public DBCopy()
        { 
            InitializeComponent();
        }

        private void cbx_selectall_CheckStateChanged(object sender, System.EventArgs e)
        {
            for (int i = 0; i < this.ckbxDatabaseobject.Items.Count; i++)
            {
                this.ckbxDatabaseobject.SetItemCheckState(i, this.cbx_selectall.CheckState);
            }
            this.ckbxDatabaseobject.Update();
            this.ckbxDatabaseobject.Refresh();
        }

        private void DBCopy_Load(object sender, EventArgs e)
        {
            this.ckbxDatabaseobject.Items.Clear();
            this.ckbxDatabaseobject.Items.AddRange(CurrentDatabase.DataSource.GetUserTables());

            if (this.MdiParent is MdiParentForm)
            {
                this.MdiParent.MdiChildActivate += MdiParent_MdiChildActivate;
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            this.MdiParent.MdiChildActivate -= MdiParent_MdiChildActivate;
            ((MdiParentForm)this.MdiParent).DBConnectTest -= btn_connect_Click;
            ((MdiParentForm)this.MdiParent).DBCopy -= button1_Click;
        }
        private void MdiParent_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.MdiParent.ActiveMdiChild == null || this.MdiParent.ActiveMdiChild != this)
            {
                ((MdiParentForm)this.MdiParent).DBConnectTest -= btn_connect_Click;
                ((MdiParentForm)this.MdiParent).DBCopy -= button1_Click;
            }
            else
            {
                ((MdiParentForm)this.MdiParent).DBConnectTest += btn_connect_Click;
                ((MdiParentForm)this.MdiParent).DBCopy += button1_Click;
            }
        }
        private void btn_connect_Click(object sender, EventArgs e)
        {
            try
            {
                TarDBConnect(); 
                TarDB.GetUserTables();
                this.pnlTranStatus.Visible = false;
                this.ckbxTarDB.Visible = true;
                if (TarDB != null)
                {
                    this.ckbxTarDB.Items.Clear();
                    this.ckbxTarDB.Items.AddRange(TarDB.GetUserTables());
                }
                MessageBox.Show("连接测试成功", "成功");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbbx_database_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.cbbx_database.Text == "DbAReflection")
            {
                MessageBox.Show("使用反射建立DbAccess,dll 文件必需有一個類繼承ODA.DataSource.DbAccess, 而且程序集必需標注ODA.AssemblyOdaAttribute屬性");
            }
            else
            {
                string msg = null;
                switch (this.cbbx_database.Text)
                {
                    case "MsSQL":
                        tbx_connectstring.Text = "server=localhost;database=master;uid=sa;pwd=sa;";
                        break;
                    case "MySql":
                        this.tbx_connectstring.Text = "Server=localhost;Database=; User=root;Password=;Use Procedure Bodies=false;Charset=utf8;Allow Zero Datetime=True; Pooling=false; Max Pool Size=50;Port=3306;Old Guids=true;";
                        break;
                    case "OdbcInformix":
                        tbx_connectstring.Text = "DSN=;User ID=;PWD=";
                        break;
                    case "OledbAccess":
                        tbx_connectstring.Text = " Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\directory\\demo.mdb;User Id=admin;Password=; ";
                        msg = " Access建立时自动添加了系统表。但Access默认是不显示的，要想看到这些表，得手动设置一下：选择菜单“工具”－“选项”－“视图”，在“系统对象”前面打勾，就能看到如下七个表了： "
                           + " MSysAccessObjects、MSysAccessXML、MSysAces、MSysImexColumns、MSysObjects、MSysQueries、MSysRelationShips"
                           + " 看是看到了，但还不能读取表里的数据，还需要设置权限：选择菜单“工具”－“安全”－“用户与组的权限”，把这些表的读写权限都勾上，OK！一切尽在掌握了，想怎么用就怎么用。"
                           //+ " 遗憾的是，微软并没给出这些表的文档说明，具体功能也只好望文生义了。较常用的MSysObjects表，很显然储存的是一些对象，里面包含了两个字段Name和Type，可以依靠它们来判断数某个表或某个查询是否存在。"
                           //+ " 例：SELECT [Name] FROM [MSysObjects] WHERE (Left([Name],1)<>'~') AND ([Type]=1)  ORDER BY [Name] "
                           //+ " 其中已知的Type值和对应的类型分别是：1－表；5－查询；-32768－窗体；-32764－报表；-32761－模块；-32766－宏。"
                           + " Connect String: Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\directory\\demo.mdb;User Id=admin;Password=;  ";

                        MessageBox.Show(msg, "提示", MessageBoxButtons.OK);
                        break;
                    case "Oracle":
                        tbx_connectstring.Text = "password=;user id=;data source=;";
                        break;
                    case "Sybase":
                        tbx_connectstring.Text = "Data Source='myASEserver'; Port=5000; Database='myDBname'; UID='username'; PWD='password'; ";
                        break;
                    case "DbAOledb":
                        msg = " Access:Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\directory\\demo.mdb;User Id=admin;Password=; \r\n"
                        + " DB2:Provider=IBMDADB2;Database=demodeb;HOSTNAME=myservername;PROTOCOL=TCPIP;PORT=50000;uid=myusername;pwd=mypasswd;\r\n"
                        + " DBase:Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\\directory;Extended Properties=dBASE IV;User ID=Admin;Password= \r\n"
                        + " Excel:Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\\directory;Extended Properties=dBASE IV;User ID=Admin;Password= \r\n"
                        + " Exchange:oConn.Provider = \"EXOLEDB.DataSource\" oConn.Open = \"http://myServerName/myVirtualRootName\" \r\n"
                        + " Firebird:User=SYSDBA;Password=mypasswd;Database=demo.fdb;DataSource=localhost;Port=3050;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0 \r\n"
                        + " FoxPro:Provider=vfpoledb.1;Data Source=c:\\directory\\demo.dbc;Collating Sequence=machine \r\n"
                        + " Informix:Provider=Ifxoledbc.2;User ID=myusername;password=mypasswd;Data Source=demodb@demoservername;Persist Security Info=true \r\n"
                        + " MySQL:Provider=MySQLProv;Data Source=mydemodb;User Id=myusername;Password=mypasswd;  \r\n"
                        + " Oracle:Provider=msdaora;Data Source=mydemodb;User Id=myusername;Password=mypasswd; \r\n"
                        + " SQL Server:Provider=sqloledb;Data Source=myservername;Initial Catalog=mydemodb;User Id=myusername;Password=mypasswd; \r\n"
                        + " Sybase:Provider=Sybase.ASEOLEDBProvider;Server Name=myservername,5000;Initial Catalog=mydemodb;User Id=myusername;Password=mypassword  "
                        + " 以上信息來自:http://blog.csdn.net/yufei_yxd/archive/2010/04/21/5510593.aspx";

                        MessageBox.Show(msg, "提示", MessageBoxButtons.OK);
                        break;

                    case "SQLite":
                        tbx_connectstring.Text = "Data Source=./sqlite.db";
                        msg = "Data Source=./sqlite.db;Version=3;UseUTF16Encoding=True;Password=myPassword;Legacy Format=True;"
                        + " Pooling=False;Max Pool Size=100;Read Only=True;DateTimeFormat=Ticks;BinaryGUID=False;Cache Size=2000;Page Size=1024;\r\n"
                            //+ "在FrameWork4.0上运行须要在<configuration>节点下加入以下配置： <startup useLegacyV2RuntimeActivationPolicy=\"true\" > "
                            //+ " <supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.0\"/> "
                            //+ " <requiredRuntime Version=\"v4.0.20506\"/>"
                            //+ " </startup>";
                            ;
                        MessageBox.Show(msg, "提示", MessageBoxButtons.OK);
                        break;
                    case "DB2":
                        this.tbx_connectstring.Text = "Server=localhost:50000;DataBase=;UID=db2admin;PWD=123;";
                        break;
                }
            }
        }

        private void TarDBConnect()
        {
            switch (this.cbbx_database.Text)
            {
                case "MsSQL":
                    TarDB = new ODA.Adapter.DbAMsSQL(this.tbx_connectstring.Text);
                    break;
                case "MySql":
                    TarDB = new ODA.Adapter.DbAMySql(this.tbx_connectstring.Text);
                    break;
                case "OdbcInformix":
                    TarDB = new ODA.Adapter.DbAOdbcInformix(this.tbx_connectstring.Text);
                    break;
                case "OledbAccess":
                    TarDB = new ODA.Adapter.DbAOledbAccess(this.tbx_connectstring.Text);
                    break;
                case "Oracle":
                    TarDB = new ODA.Adapter.DbAOracle(this.tbx_connectstring.Text);
                    break;
                case "Sybase":
                    TarDB = new ODA.Adapter.DbASybase(this.tbx_connectstring.Text);
                    break;
                case "SQLite":
                    TarDB = new ODA.Adapter.DbASQLite(this.tbx_connectstring.Text);
                    break;
                case "DB2":
                    TarDB = new ODA.Adapter.DbADB2(this.tbx_connectstring.Text);
                    break;
                default:
                    TarDB = CurrentDatabase.DataSource;
                    break;
            } 

        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (!cbxTransData.Checked && !cbxCreateTable.Checked && !cbxTableScript.Checked)
            {
                MessageBox.Show("请选择复制类型", "提示", MessageBoxButtons.OK);
                return;
            } 
            TarDBConnect(); 
            if (cbxTransData.Checked || cbxCreateTable.Checked)
            {
                try
                {
                    this.TarDB.GetUserTables();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK);
                    return;
                }
            }
            TransferParams prm = new TransferParams();
            prm.SourceDB = CurrentDatabase.DataSource;
            prm.TargetDB = TarDB;
            prm.NeedTransData = cbxTransData.Checked;
            prm.NeedTransTable = cbxCreateTable.Checked;
            prm.TableScript = cbxTableScript.Checked;
            prm.SrcTables = CurrentDatabase.DataSource.GetTableColumns();
            prm.TranTable = new List<string>();

            for (int i = 0; i < this.ckbxDatabaseobject.CheckedItems.Count; i++)
            {
                string TableName = this.ckbxDatabaseobject.CheckedItems[i].ToString();
                prm.TranTable.Add(TableName);
            }

            BackgroundWorker bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;
            bgw.WorkerSupportsCancellation = true;
            bgw.ProgressChanged += bgw_ProgressChanged;
            bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
            bgw.DoWork += bgw_DoWork;
            bgw.RunWorkerAsync(prm);

        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] rtlMsg = new string[2];
            try
            {
                StringBuilder sbrlt = new StringBuilder();
                TransferParams prm = e.Argument as TransferParams;
                BackgroundWorker bgw = sender as BackgroundWorker;

                StringBuilder tblScript = new StringBuilder();
                string TargetDB = prm.TargetDB.DBAType.ToString();

                var TblPkeys = prm.SourceDB.GetPrimarykey();

                for (int i = 0; i < prm.TranTable.Count; i++)
                {
                    ReportStatus RS = new ReportStatus()
                    {
                        Percent = i * 100 / prm.TranTable.Count,
                        TransObject = prm.TranTable[i],
                        TransType = "Table"
                    };
                    bgw.ReportProgress(RS.Percent, RS);

                    DataRow[] drs = prm.SrcTables.Select("TABLE_NAME ='" + prm.TranTable[i] + "'");

                    if (drs == null || drs.Length == 0)
                        continue;
                    DBColumnInfo[] ColumnInfo = new DBColumnInfo[drs.Length];
                    ODAParameter[] Oprms = new ODAParameter[drs.Length];

                    bool isBigData = false; 
                    try
                    {
                        for (int j = 0; j < drs.Length; j++)
                        {
                            int Scale = 0;
                            int.TryParse(drs[j]["SCALE"].ToString().Trim(), out Scale);
                            int length = 2000;
                            int.TryParse(drs[j]["LENGTH"].ToString().Trim(), out length);
                            string ColumnName = drs[j]["COLUMN_NAME"].ToString().Trim();

                            DBColumnInfo DBColInfo = new DBColumnInfo()
                            {
                                ColumnName = ColumnName,
                                ColumnType = drs[j]["DATATYPE"].ToString().Trim(),
                                Length = length,
                                Scale = Scale,
                                NotNull = drs[j]["NOT_NULL"].ToString().Trim().ToUpper() == "Y",
                            };
                            CurrentDatabase.GetTargetsType(prm.SourceDB.DBAType.ToString(), TargetDB, ref DBColInfo);
                            ColumnInfo[j] = DBColInfo;
                            isBigData = isBigData || DBColInfo.IsBigData;

                            DBColumnInfo ODAColInfo = new DBColumnInfo()
                            {
                                ColumnName = ColumnName,
                                ColumnType = drs[j]["DATATYPE"].ToString().Trim(),
                                Length = length,
                                Scale = Scale,
                                NotNull = drs[j]["NOT_NULL"].ToString().Trim().ToUpper() == "Y",
                            };
                            CurrentDatabase.GetTargetsType(prm.SourceDB.DBAType.ToString(), "ODA", ref ODAColInfo);
                            ODAdbType OdaType = ODAdbType.OVarchar;

                            OdaType = (ODAdbType)Enum.Parse(typeof(ODAdbType), ODAColInfo.ColumnType, true);

                            Oprms[j] = new ODAParameter()
                            {
                                ColumnName = drs[j]["COLUMN_NAME"].ToString(),
                                DBDataType = OdaType,
                                Direction = ParameterDirection.Input,
                                ParamsName = drs[j]["COLUMN_NAME"].ToString(),
                                Size = ColumnInfo[j].Length
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        sbrlt.AppendLine(string.Format("表【{0}】字段异常,异常信息：{1} ", prm.TranTable[i], ex.Message));
                    }
                    string sql = "";
                    try
                    {
                        string[] Pkeys = null;
                        if (TblPkeys != null && TblPkeys.ContainsKey(prm.TranTable[i]))
                        {
                            Pkeys = TblPkeys[prm.TranTable[i]];
                        } 
                        sql = this.CreateTable(prm.TargetDB, prm.TranTable[i], ColumnInfo, Pkeys);
                        tblScript.AppendLine(sql);
                    }
                    catch (Exception ex)
                    {
                        sbrlt.AppendLine(string.Format("读取表【{0}】主键并生成建表脚本时生异常,异常信信：{1} ", prm.TranTable[i], ex.Message));
                    }

                    try
                    {
                        if (prm.NeedTransTable)
                        {
                            try
                            {
                                string dropSQL = "DROP TABLE " + prm.TranTable[i];
                                prm.TargetDB.ExecuteSQL(dropSQL, null);
                            }
                            catch { }
                            prm.TargetDB.ExecuteSQL(sql.ToString(), null);
                        }
                        ReportStatus RST = new ReportStatus()
                        {
                            Percent = (i + 1) * 100 / prm.TranTable.Count,
                            TransObject = "Table [" + prm.TranTable[i] + "] Created",
                            TransType = "Table"
                        };
                        bgw.ReportProgress(RS.Percent, RST);

                    }
                    catch (Exception ex)
                    {
                        sbrlt.AppendLine(string.Format("创建表【{0}】时发生异常,建表脚本  {1}   ，异常信信：{2} ", prm.TranTable[i], sql.ToString(), ex.Message));
                    }
                    try
                    {
                        if (prm.NeedTransData)
                        {
                            int total = 0;
                            int maxR = isBigData ? 50 : 10000;
                            int startIndx = 0;
                            DataTable DT_total = CurrentDatabase.DataSource.Select("SELECT COUNT(*) FROM " + prm.TranTable[i], null);
                            int.TryParse(DT_total.Rows[0][0].ToString(), out total);
                            while (startIndx < total)
                            {
                                ReportStatus RSData0 = new ReportStatus()
                                {
                                    Percent = total == 0 ? 0 : startIndx * 100 / total,
                                    TransObject = prm.TranTable[i] + " Preparing " + startIndx.ToString() + " ~ " + (startIndx + maxR).ToString() + "/" + total.ToString() + " record ",
                                    TransType = "Data"
                                };
                                bgw.ReportProgress(RS.Percent, RSData0);
                                DataTable Source = CurrentDatabase.DataSource.Select("SELECT * FROM " + prm.TranTable[i], null, startIndx, maxR, null);
                                Source.TableName = prm.TranTable[i];

                                int endIdx = (startIndx + maxR) > total ? total : startIndx + maxR;
                                ReportStatus RSData1 = new ReportStatus()
                                {
                                    Percent = total == 0 ? 0 : endIdx * 100 / total,
                                    TransObject = prm.TranTable[i] + " Importing " + startIndx.ToString() + " ~ " + endIdx.ToString() + "/" + total.ToString() + " record ",
                                    TransType = "Data"
                                };
                                bgw.ReportProgress(RS.Percent, RSData1);

                                TarDB.Import(Source, Oprms);
                                startIndx = startIndx + maxR;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        sbrlt.AppendLine(string.Format("导入数据到表【{0}】时发生异常：{1} ", prm.TranTable[i], ex.Message));
                    }
                }
                if (sbrlt.Length == 0)
                    sbrlt.Append("数据复制完成！");
                rtlMsg[0] = sbrlt.ToString();
                rtlMsg[1] = tblScript.ToString();
            }
            catch (Exception ex)
            {
                rtlMsg[0] = ex.ToString();
            }
            e.Result = rtlMsg;
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!this.InvokeRequired)
            {
                this.pnlTranStatus.Visible = false;
                this.ckbxTarDB.Visible = true; 
                this.ckbxTarDB.Items.Clear();
                if (cbxTransData.Checked || cbxCreateTable.Checked) 
                    this.ckbxTarDB.Items.AddRange(TarDB.GetUserTables());

                if (cbxTableScript.Checked && !string.IsNullOrWhiteSpace(((string[])e.Result)[1]))
                {
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Filter = "SQL|*.sql";
                    if (saveFile.ShowDialog(this) == DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(saveFile.FileName, ((string[])e.Result)[1], Encoding.UTF8);
                    }
                }
                this.TarDB = null;
                MessageBox.Show(((string[])e.Result)[0], "提示", MessageBoxButtons.OK);
            }
            else
            {
                this.BeginInvoke(new Action(() =>
                {
                    this.pnlTranStatus.Visible = false;
                    this.ckbxTarDB.Visible = true; 
                    this.ckbxTarDB.Items.Clear();
                    if (cbxTransData.Checked || cbxCreateTable.Checked)
                        this.ckbxTarDB.Items.AddRange(TarDB.GetUserTables());

                    if (cbxTableScript.Checked && !string.IsNullOrWhiteSpace(((string[])e.Result)[1]) )
                    {
                        SaveFileDialog saveFile = new SaveFileDialog();
                        saveFile.Filter = "SQL|sql";
                        if (saveFile.ShowDialog(this) == DialogResult.OK)
                        {
                            System.IO.File.WriteAllText(saveFile.FileName, ((string[])e.Result)[1], Encoding.UTF8);
                        }
                    }
                    this.TarDB = null;
                    MessageBox.Show(((string[])e.Result)[0], "提示", MessageBoxButtons.OK);

                }), null);

            }
        }

        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!this.InvokeRequired)
            {
                ReportStatus RS = e.UserState as ReportStatus;
                SetTranStatus(RS);
            }
            else
            {
                this.BeginInvoke(new Action<ReportStatus>(SetTranStatus), e.UserState);
            }
        }
        private void SetTranStatus(ReportStatus RS)
        {
            this.pnlTranStatus.Visible = true;
            this.ckbxTarDB.Visible = false ; 
            if (RS.TransType == "Table")
            {
                this.lblTransTable.Text = RS.TransObject;
                this.lblTransData.Text = "Preparing Data";
                pgbTable.Value = RS.Percent; 
                pgbTable.Update();
            }
            else if (RS.TransType == "Data")
            {
                this.lblTransData.Text = RS.TransObject;
                this.pgbData.Value = RS.Percent;
                this.pgbData.Update();
            }
        }
        private string CreateTable(DBAccess TargetDB, string TableName, DBColumnInfo[] ColumnInfo, string[]  Pkeys )
        {
            StringBuilder creatSQL = new StringBuilder();
            try
            {
                creatSQL.AppendLine("CREATE TABLE " + TableName);
                creatSQL.AppendLine("(");

                for (int i = 0; i < ColumnInfo.Length; i++)
                {
                    if (ColumnInfo[i] != null)
                    {
                        if (ColumnInfo[i].NoLength)
                        {
                            creatSQL.Append(ColumnInfo[i].ColumnName + " " + ColumnInfo[i].ColumnType);
                        }
                        else
                        {
                            if (ColumnInfo[i].Scale == 0)
                            {
                                creatSQL.Append(ColumnInfo[i].ColumnName + " " + ColumnInfo[i].ColumnType + "(" + ColumnInfo[i].Length.ToString() + ")");
                            }
                            else
                            {
                                creatSQL.Append(ColumnInfo[i].ColumnName + " " + ColumnInfo[i].ColumnType + "(" + ColumnInfo[i].Length.ToString() + "," + ColumnInfo[i].Scale.ToString() + ")");
                            }
                        }

                        if (ColumnInfo[i].NotNull)
                            creatSQL.Append(" NOT NULL ");
                        if (i + 1 < ColumnInfo.Length)
                            creatSQL.AppendLine(",");
                    }
                }

                if (Pkeys != null && Pkeys.Length > 0)
                {
                    string p = "";
                    for (int j = 0; j < Pkeys.Length; j++)
                        p += Pkeys[j] + ",";
                    p = p.Remove(p.Length - ",".Length, ",".Length);
                    creatSQL.AppendLine(",");
                    creatSQL.AppendLine("PRIMARY KEY (" + p + ") ");
                }
                else
                {
                    creatSQL.AppendLine("");
                }
                creatSQL.AppendLine(")"); 
                return creatSQL.ToString();
            }
            catch(Exception ex)
            {
                creatSQL.Insert(0, "Create Table Error: " + ex.Message);
                return creatSQL.ToString();
            }
          
        }

    }
     

    public class TransferParams
    {
        public DBAccess SourceDB { get; set; }
        public DBAccess TargetDB { get; set; }
        public bool NeedTransData { get; set; }
        public bool NeedTransTable { get; set; }
        public bool TableScript { get; set; }
        public DataTable SrcTables { get; set; }
        public List<string> TranTable { get; set; }
        public DataTable SrcData { get; set; }
    }

    public class ReportStatus
    {
        public string TransObject { get; set; }
        public string TransType { get; set; }
        public int Percent { get; set; }
    }
   
}
