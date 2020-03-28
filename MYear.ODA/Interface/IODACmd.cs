using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MYear.ODA
{
    public interface IODACmd
    {
        string Alias { get; set; }
        string Schema { get; set; }
        GetDBAccessHandler GetDBAccess { get; set; }
        Func<string> GetAlias { get; set; }
        string DBObjectMap { get; set; }
        Encoding DBCharSet { get; set; }
    }
}