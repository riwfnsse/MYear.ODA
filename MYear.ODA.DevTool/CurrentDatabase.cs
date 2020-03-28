using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MYear.ODA.DevTool
{
    internal class CurrentDatabase
    {
        private static ODA.DBAccess _DataSource = null;
        public static ODA.DBAccess DataSource
        {
            get { return _DataSource; }
            set
            {
                _DataSource = value;
                if (DBConnected != null)
                    DBConnected(_DataSource, EventArgs.Empty);
            }
        }
        public static string DBConnectString { get; set; }
        public static string[] UserTables { get; set; }
        public static string[] UserViews { get; set; }
        public static string[] UserProcedures { get; set; }
        public static event EventHandler DBConnected;


        private static Dictionary<string, object> _ODATypeMap = null;
        public static Dictionary<string, object> ODATypeMap
        {
            get
            {
                if (_ODATypeMap == null)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
                    if (System.IO.File.Exists("DbType.json"))
                    {
                        string dbType = System.IO.File.ReadAllText("DbType.json", Encoding.UTF8);
                        _ODATypeMap = json.Deserialize<Dictionary<string, object>>(dbType);
                    }
                }
                return _ODATypeMap;
            }
        }
        public static void GetTargetsType(string From, string Target, ref DBColumnInfo Column)
        {
            if (ODATypeMap != null && ODATypeMap.ContainsKey(From))
            {
                var FromDict = ODATypeMap[From] as Dictionary<string, object>; 
                if (FromDict != null)
                {
                    string trg = "Default";
                    if (FromDict.ContainsKey(Target))
                    {
                        trg = Target;
                    }

                    var TargetDict = FromDict[trg] as Dictionary<string, object>;
                    if (TargetDict != null && TargetDict.ContainsKey(Column.ColumnType))
                    {
                        string colType = TargetDict[Column.ColumnType].ToString();
                        Column.IsBigData = false;
                        string[] typeInfo = colType.Split(',');
                        if (typeInfo.Length > 0)
                        {
                            Column.ColumnType = typeInfo[0];
                        }
                        if (typeInfo.Length > 1)
                        {
                            if (typeInfo[1] == "0")
                            {
                                Column.NoLength = true;
                                Column.Length = 0;
                            }
                            else
                            {
                                try
                                {
                                    int setLength = 0;
                                    int.TryParse(typeInfo[1],out setLength);
                                    if (Column.Length == 0 && setLength != 0)
                                    {
                                        Column.Length = setLength;
                                    }
                                    else if(setLength != 0)
                                    {
                                        Column.Length = Column.Length * setLength;
                                    }
                                }
                                catch { }
                            }
                        }
                        if (typeInfo.Length > 2 && typeInfo[1].Trim().ToLower() == "y")
                        {
                            Column.IsBigData = true;
                        } 
                    }
                }
            }

        }
    }

    public class DBColumnInfo
    {
        public string ColumnName { get; set; }
        public int Length { get; set; }
        public int Scale { get; set; }
        public string ColumnType { get; set; }
        public bool NoLength { get; set; }
        public bool NotNull { get; set; }
        public bool IsBigData { get; set; }
    }
}
