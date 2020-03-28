using System;
using System.Collections.Generic;

namespace MYear.ODA
{
    public interface IODAFunction
    {
        ODAColumns Count { get; }
        ODAColumns Case(ODAColumns CaseColumn, Dictionary<object, object> WhenThen, object ElseVal);
        ODAColumns CaseWhen(Dictionary<ODAColumns, object> WhenThen, object ElseVal);
        ODAColumns CreateFunc(string Function, params object[] ParamsList);
        ODAColumns Exists(ODACmd Cmd, params ODAColumns[] Cols);
        ODAColumns NotExists(ODACmd Cmd, params ODAColumns[] Cols); 
        ODAColumns VisualColumn(string Val);
        ODAColumns VisualColumn(DateTime Val);
        ODAColumns VisualColumn(Decimal Val);
        ODAColumns Express(string Expression, params ODAParameter[] ODAParameters);
        ODAColumns NullDefault(ODAColumns Col, object DefVal);
        ODAColumns Decode(ODAColumns Col, object DefVal, params object[] KeyValue);
    }
}