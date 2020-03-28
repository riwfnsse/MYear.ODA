using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MYear.ODA.DevTool
{
    public class MdiParentForm : Form
    {
        public virtual event EventHandler ExecuteSQL;
        public virtual event EventHandler DbRefresh;
        public virtual event EventHandler DBCopy;
        public virtual event EventHandler DBConnectTest;
        public virtual event EventHandler ORMCodeCreate;
        public virtual event EventHandler ORMCodeSave; 
    }
}
