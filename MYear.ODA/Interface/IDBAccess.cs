using System;
using System.Collections.Generic;
using System.Data;

namespace MYear.ODA
{

    public interface IDBAccess
    { 
        int CommandTimeOut { get; set; }
        char ParamsMark { get; }
        DbAType DBAType { get; }
        string ConnString { get; }
        string[] ObjectFlag { get; }
        IDbTransaction Transaction { get; set; }
        Action<IDbCommand> ExecutingCommand { get; set; }
        void BeginTransaction();
        void Commit();
        void RollBack();
        DataSet ExecuteProcedure(string SQL, ODAParameter[] ParamList);
        int ExecuteSQL(string SQL, ODAParameter[] ParamList);
        DateTime GetDBDateTime();
        object GetExpressResult(string ExpressionString);
        long GetSequenceNextVal(string SequenceName);
        DataTable GetTableColumns();
        string[] GetUserTables();
        string[] GetUserViews();
        string[] GetUserProcedure();
        string[] GetPrimarykey(string TableName);
        Dictionary<string, string[]> GetPrimarykey();
        DataTable GetUniqueIndex(string TableName);
        DataTable GetViewColumns(); 
        List<T> Select<T>(string SQL, ODAParameter[] ParamList) where T : class;
        List<T> Select<T>(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord,string Orderby) where T : class;
        List<T> Select<T>(string SQL, ODAParameter[] ParamList, string StartWithExpress, string ConnectBy, string Prior, string ConnectColumn, string ConnectChar, int MaxLevel) where T : class;
        DataTable Select(string SQL, ODAParameter[] ParamList);
        DataTable Select(string SQL, ODAParameter[] ParamList, int StartIndex, int MaxRecord,string Orderby);
        object[] SelectFirst(string SQL, ODAParameter[] ParamList); 
        DataTable Select(string SQL, ODAParameter[] ParamList, string StartWithExpress, string ConnectBy, string Prior, string ConnectColumn, string ConnectChar, int MaxLevel);
        DataTable GetUserProcedureArguments(string ProcedureName);
        bool Import(DataTable Data, ODAParameter[] Prms); 
    }
}
