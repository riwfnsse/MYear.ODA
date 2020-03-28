using MYear.ODA;
using MYear.ODA.Cmd;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MYear.Demo
{
    public partial class Demo : Form
    {
        List<DemoMethodInfo> _DemoMethods = null;

        StringBuilder _ExeSql = new StringBuilder();

        public Demo()
        {
            InitializeComponent();
            InitFuncType();
            _DemoMethods = GetDemoMethods();
            InitNYearODA();

            ODAContext.CurrentExecutingODASql += (src, args) => {

                 
                _ExeSql.AppendLine(args.SqlParams.SqlScript.ToString() + ";");
                _ExeSql.AppendLine("");
            }; 
        }
        private void InitNYearODA()
        {
            SessionStart ss = new SessionStart();
            ss.ShowDialog(); 
        }
        private void InitFuncType()
        {
            string[] fts = Enum.GetNames(typeof(FuncType));
            foreach (var ft in fts)
            {
                CheckBox cbx = new CheckBox();
                cbx.Text = ft;
                cbx.CheckedChanged += Cbx_CheckedChanged;
                plFuncType.Controls.Add(cbx);
            }
        }
        private void Cbx_CheckedChanged(object sender, EventArgs e)
        {
            List<string> funcTypes = new List<string>();
            foreach (Control ctl in plFuncType.Controls)
            {
                if (ctl is CheckBox)
                {
                    if (((CheckBox)ctl).Checked)
                        funcTypes.Add(((CheckBox)ctl).Text);
                }
            }

            ShowDemoFunc(funcTypes);
        }

        private List<DemoMethodInfo> GetDemoMethods()
        {
            Type[] types = this.GetType().Assembly.GetTypes();
            List<DemoMethodInfo> DemoMethods = new List<DemoMethodInfo>();
            foreach (var tp in types)
            {
                MethodInfo[] mds = tp.GetMethods();
                foreach (var md in mds)
                {
                    if (md.IsDefined(typeof(DemoAttribute),false))
                    {
                         var att = md.GetCustomAttributes(typeof(DemoAttribute), false);
                        if (att != null && att.Count() > 0)
                        {
                            DemoAttribute dmAttr = (DemoAttribute)att[0];
                            DemoMethodInfo mi = new DemoMethodInfo()
                            {
                                MethodName = dmAttr.MethodName,
                                DemoFunc = dmAttr.Demo,
                                MethodDescript = dmAttr.MethodDescript,
                                DemoMethod = md
                            };
                            DemoMethods.Add(mi);
                        }
                    }
                }
            }
            return DemoMethods;
        }

        private void ShowDemoFunc(List<string> funcTypes)
        {
            var mthds = (from mthd in _DemoMethods
                         where funcTypes.Contains(mthd.DemoFunc.ToString())
                         select mthd).ToArray();

            fplDemoFunc.Controls.Clear();
            foreach (var md in mthds)
            {
                Button btn = new Button();
                btn.Text = md.MethodName;
                btn.Click += Btn_Click;
                btn.Tag = md;
                btn.Size = new Size(120, 23);
                ToolTip ti = new ToolTip();
                ti.AutoPopDelay = 5000;
                ti.InitialDelay = 1000;
                ti.ReshowDelay = 500;
                ti.ShowAlways = true;
                
                ti.SetToolTip(btn, md.MethodDescript);
                fplDemoFunc.Controls.Add(btn);
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            try
            {
                _ExeSql.Clear();
                rtbxSql.Clear();
                var md = (DemoMethodInfo)((Button)sender).Tag;
                object rlt = md.DemoMethod.Invoke(null, null);
                if (rlt is DataTable ||rlt is Array || rlt.GetType().IsGenericType)
                {
                    dgvData.DataSource = rlt;
                }
                else if (rlt != null)
                {
                    rtbxSql.AppendText(rlt.ToString());
                    rtbxSql.AppendText(new StringBuilder().AppendLine("").ToString());
                }
               
                rtbxSql.AppendText(_ExeSql.ToString());
            }
            catch (Exception ex)
            {
                rtbxSql.AppendText(_ExeSql.ToString());
                rtbxSql.AppendText("\n\r");
                rtbxSql.AppendText( GetInnerException(ex).Message);
            }
        }

        private Exception GetInnerException(Exception ex)
        {
            if (ex.InnerException == null)
                return ex;
            return GetInnerException(ex.InnerException);
        } 
    }
}
