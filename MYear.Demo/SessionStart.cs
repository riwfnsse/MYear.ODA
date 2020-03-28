using MYear.ODA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace MYear.Demo
{
    public partial class SessionStart : Form
    {
        private const string ConnPath = "ConnectString.json";

        List<string> _ConnectString = new List<string>();

        public SessionStart()
        {
            InitializeComponent();
            try
            {
                if (System.IO.File.Exists(ConnPath))
                {
                    string js = System.IO.File.ReadAllText(ConnPath, Encoding.UTF8);
                    JavaScriptSerializer JsConvert = new JavaScriptSerializer();
                    _ConnectString = JsConvert.Deserialize<List<string>>(js);
                }
            }
            catch { }

            this.cbbxConnectstring.DataSource = _ConnectString;
        }

        private void cbbx_database_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.cbbx_database.Text == "DbAReflection")
            {
                this.lblExecuteRlt.Text = "使用反射建立DbAccess,dll 文件必需有一個類繼承ODA.DataSource.DbAccess, 而且程序集必需標注ODA.AssemblyOdaAttribute屬性";
            }
            else
            {
                switch (this.cbbx_database.Text)
                {
                    case "MsSQL":
                        cbbxConnectstring.Text = "server=localhost;database=master;uid=sa;pwd=sa;";
                        this.lblExecuteRlt.Text = "server=localhost;database=master;uid=sa;pwd=sa;";
                        break;
                    case "MySql":
                        cbbxConnectstring.Text = "Server=localhost;Database=; User=root;Password=;Use Procedure Bodies=false;Charset=utf8;Allow Zero Datetime=True; Pooling=false; Max Pool Size=50;Port=3306;";
                        this.lblExecuteRlt.Text = "Server=localhost;Database=; User=root;Password=;Use Procedure Bodies=false;Charset=utf8;Allow Zero Datetime=True; Pooling=false; Max Pool Size=50;Port=3306;";
                        break;
                    case "OdbcInformix":
                        cbbxConnectstring.Text = "DSN=;User ID=;PWD=";
                        this.lblExecuteRlt.Text = "DSN=;User ID=;PWD=";
                        break;
                    case "OledbAccess":
                        cbbxConnectstring.Text = " Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\directory\\demo.mdb;User Id=admin;Password=; ";
                        this.lblExecuteRlt.Text = " Access建立时自动添加了系统表。但Access默认是不显示的，要想看到这些表，得手动设置一下：选择菜单“工具”－“选项”－“视图”，在“系统对象”前面打勾，就能看到如下七个表了： "
                            + " MSysAccessObjects、MSysAccessXML、MSysAces、MSysImexColumns、MSysObjects、MSysQueries、MSysRelationShips"
                            + " 看是看到了，但还不能读取表里的数据，还需要设置权限：选择菜单“工具”－“安全”－“用户与组的权限”，把这些表的读写权限都勾上，OK！一切尽在掌握了，想怎么用就怎么用。"
                            //+ " 遗憾的是，微软并没给出这些表的文档说明，具体功能也只好望文生义了。较常用的MSysObjects表，很显然储存的是一些对象，里面包含了两个字段Name和Type，可以依靠它们来判断数某个表或某个查询是否存在。"
                            //+ " 例：SELECT [Name] FROM [MSysObjects] WHERE (Left([Name],1)<>'~') AND ([Type]=1)  ORDER BY [Name] "
                            //+ " 其中已知的Type值和对应的类型分别是：1－表；5－查询；-32768－窗体；-32764－报表；-32761－模块；-32766－宏。"
                            + " Connect String: Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\directory\\demo.mdb;User Id=admin;Password=;  ";
                        break;
                    case "Oracle":
                        cbbxConnectstring.Text = "password=123;user id=xxxx;data source=127.0.0.1:1501/orca;";
                        this.lblExecuteRlt.Text = "password=123;user id=xxxx;data source=127.0.0.1:1501/orca";
                        break;
                    case "Sybase":
                        cbbxConnectstring.Text = "Data Source='myASEserver'; Port=5000; Database='myDBname'; UID='username'; PWD='password'; ";
                        this.lblExecuteRlt.Text = "Data Source='myASEserver'; Port=5000; Database='myDBname'; UID='username'; PWD='password'; ";
                        break;
                    case "DbAOledb":
                        this.lblExecuteRlt.Text = " Access:Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\directory\\demo.mdb;User Id=admin;Password=; \r\n"
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
                        break;
                    case "SQLite":
                        cbbxConnectstring.Text = "Data Source=./sqlite.db";
                        this.lblExecuteRlt.Text = "Data Source=./sqlite.db;Version=3;UseUTF16Encoding=True;Password=myPassword;Legacy Format=True;"
                        + " Pooling=False;Max Pool Size=100;Read Only=True;DateTimeFormat=Ticks;BinaryGUID=False;Cache Size=2000;Page Size=1024;\r\n"
                            //+ "在FrameWork4.0上运行须要在<configuration>节点下加入以下配置： <startup useLegacyV2RuntimeActivationPolicy=\"true\" > "
                            //+ " <supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.0\"/> "
                            //+ " <requiredRuntime Version=\"v4.0.20506\"/>"
                            //+ " </startup>";
                            ;
                        break;
                    case "DB2":
                        cbbxConnectstring.Text = "Server=localhost:50000;DataBase=;UID=db2admin;PWD=123;";
                        this.lblExecuteRlt.Text = "Server=localhost:50000;DataBase=;UID=db2admin;PWD=123;";
                        break;
                }
            }
        }

        private void btn_connect_Click(object sender, System.EventArgs e)
        {
            try
            {
                var DBConnectString = this.cbbxConnectstring.Text; 
                switch (this.cbbx_database.Text)
                {
                    case "MsSQL":
                        ODAContext.SetODAConfig(ODA.DbAType.MsSQL, this.cbbxConnectstring.Text); 
                        break;
                    case "MySql":
                        ODAContext.SetODAConfig(ODA.DbAType.MySql, this.cbbxConnectstring.Text); 
                        break;
                    case "OdbcInformix":
                        ODAContext.SetODAConfig(ODA.DbAType.OdbcInformix, this.cbbxConnectstring.Text);
                        break;
                    case "OledbAccess":
                        ODAContext.SetODAConfig(ODA.DbAType.OledbAccess, this.cbbxConnectstring.Text);
                        break;
                    case "Oracle":
                        ODAContext.SetODAConfig(ODA.DbAType.Oracle, this.cbbxConnectstring.Text);
                        break;
                    case "Sybase":
                        ODAContext.SetODAConfig(ODA.DbAType.Sybase, this.cbbxConnectstring.Text);
                        break;
                    case "SQLite":
                        ODAContext.SetODAConfig(ODA.DbAType.SQLite, this.cbbxConnectstring.Text);
                        break;
                    case "DB2":
                        ODAContext.SetODAConfig(ODA.DbAType.DB2, this.cbbxConnectstring.Text);
                        break;
                    default:
                        break;
                }

                ODAContext cxt = new ODAContext();
                var DateNow =  cxt.DBDatetime; 

                if (!_ConnectString.Contains(this.cbbxConnectstring.Text))
                {
                    _ConnectString.Add(this.cbbxConnectstring.Text);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsStr = js.Serialize(_ConnectString);
                    System.IO.File.WriteAllText(ConnPath, jsStr, Encoding.UTF8);
                } 
                this.Close();
            }
            catch (Exception ex)
            {
                this.lblExecuteRlt.Text = "Create DataSource Error : " + ex.Message;
            }
        }
    }
}
