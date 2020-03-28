using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MYear.ODA
{
    public partial class ODACmd
    {
        public dynamic SelectDynamicFirst(params IODAColumns[] Cols)
        {
            int total = 0;
            var dt = this.Select(0, 1, out total, Cols); 
            ODynamicModel M = new ODynamicModel(); 
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataColumn c in dt.Columns)
                {
                    M.Add(c.ColumnName, dt.Rows[0][c.ColumnName]);
                }
            }
            else
            {
                foreach (IODAColumns c in Cols)
                {
                    M.Add(c.ColumnName, null);
                }
            }
            return M;
        }

        public List<dynamic> SelectDynamic(params IODAColumns[] Cols)
        {
            var dt = this.Select(Cols);
            List<dynamic> Models = new List<dynamic>();
            if (dt != null && dt.Rows.Count > 0)
            {
                string[] CNames = new string[dt.Columns.Count];
                for (int i = 0; i < CNames.Length; i++)
                {
                    CNames[i] = dt.Columns[i].ColumnName;
                }
                foreach (DataRow r in dt.Rows)
                {
                    ODynamicModel M = new ODynamicModel();
                    for (int i = 0; i < CNames.Length; i++)
                    {
                        M.Add(CNames[i], r.ItemArray[i]);
                    }
                    Models.Add(M);
                }
            }
            return Models;
        }
        public List<dynamic> SelectDynamic(int StartIndex, int MaxRecord, out int Total, params IODAColumns[] Cols)
        {
            var dt = this.Select(StartIndex, MaxRecord,out Total,Cols);
            List<dynamic> Models = new List<dynamic>();
            if (dt != null && dt.Rows.Count > 0)
            {
                string[] CNames = new string[dt.Columns.Count];
                for (int i = 0; i < CNames.Length; i++)
                {
                    CNames[i] = dt.Columns[i].ColumnName;
                }
                foreach (DataRow r in dt.Rows)
                {
                    ODynamicModel M = new ODynamicModel();
                    for (int i = 0; i < CNames.Length; i++)
                    {
                        M.Add(CNames[i], r.ItemArray[i]);
                    }
                    Models.Add(M);
                }
            }
            return Models;
        }
    }
}
