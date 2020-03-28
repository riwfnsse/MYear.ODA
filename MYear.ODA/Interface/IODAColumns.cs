namespace MYear.ODA
{
    public interface IODAColumns
    {
        string AliasName { get; } 
        object CompareValue { get; }
        ODAdbType DBDataType { get; }
        int Size { get; } 
        ODAParameter[] GetInsertSubstring(out string SubSql, out string SubSqlParams);
        ODAParameter GetProcedureParams();
        ODAParameter[] GetSelectColumn(out string SubSql);
        ODAParameter[] GetUpdateSubstring(out string SubSql);
        ODAScript GetWhereSubstring(); 
        
        ODAColumns SetParamDataType(ODAdbType ParamType);
        string GetColumnName();
        string ColumnName { get; } 
        string ColumnComment { get; } 
        bool IsRequired { get; }
        string ODAColumnName { get; }



        //ODAColumns SetCondition(CmdConditionSymbol Symbol, object CompareValue);
        //ODAColumns And(params ODAColumns[] Columns);
        //ODAColumns As(string AliasColumnName);
        //ODAColumns In(ODACmd Cmd, ODAColumns Col);
        //ODAColumns In(params object[] CompareValue);
        //ODAColumns Like(object CompareValue);
        //ODAColumns NotIn(ODACmd Cmd, ODAColumns Col);
        //ODAColumns NotIn(params object[] CompareValue);
        //ODAColumns Or(params ODAColumns[] Columns);

        //ODAColumns Count { get; }
        //ODAColumns Ascii { get; }
        //ODAColumns Avg { get; }
        //ODAColumns Sum { get; }
        //ODAColumns Trim { get; }
        //ODAColumns Upper { get; }
        //ODAColumns IsNotNull { get; }
        //ODAColumns IsNull { get; }
        //ODAColumns Length { get; }
        //ODAColumns Lower { get; }
        //ODAColumns Ltrim { get; }
        //ODAColumns Rtrim { get; }
        //ODAColumns Max { get; }
        //ODAColumns Min { get; }
    }
}