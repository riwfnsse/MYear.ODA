using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace MYear.ODA
{
    public abstract class ORMCmd<T> : ODACmd where T : class
    {
        /// <summary>
        /// 子类重写,命令可用的变量
        /// </summary>
        /// <returns></returns>
        public abstract List<ODAColumns> GetColumnList();
        protected virtual List<ODAColumns> BindColumnValues(T Model)
        {
            PropertyInfo[] Pis = Model.GetType().GetProperties();
            List<ODAColumns> Cs = GetColumnList();
            List<ODAColumns> CList = new List<ODAColumns>();
            foreach (PropertyInfo Pi in Pis)
            {
                object V = Pi.GetValue(Model, null);
                if (V != DBNull.Value && V != null)
                    foreach (ODAColumns C in Cs)
                        if (C.ColumnName == Pi.Name)
                        {
                            C.SetCondition(CmdConditionSymbol.EQUAL, V);
                            CList.Add(C);
                        }
            }
            return CList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data">导入的数据：要求与数据库表字段字称及数据类型一致</param>
        /// <returns></returns>
        public  bool Import(DataTable Data)
        {
            try
            {
                List<ODAColumns> cols = this.GetColumnList();
                List<ODAParameter> prms = new List<ODAParameter>();
                for (int i = 0; i < Data.Columns.Count; i++)
                {
                    for (int j = 0; j < cols.Count; j++)
                    {
                        if (cols[j].ColumnName == Data.Columns[i].ColumnName)
                        {
                            prms.Add(new ODAParameter()
                            {
                                ColumnName = cols[j].ColumnName,
                                Direction = ParameterDirection.Input,
                                ParamsName = cols[j].ColumnName,
                                DBDataType = cols[j].DBDataType,
                                Size = cols[j].Size,
                            });
                            break; 
                        }
                    }
                }
                return base.Import(Data, prms.ToArray());
            }
            finally
            {
                this.Clear();
            }
        }
        public List<T> SelectM(params ODAColumns[] Cols)
        {
            return this.Select<T>(Cols); 
        }
        public List<T> SelectM(int StartIndex, int MaxRecord, out int TotalRecord, params ODAColumns[] Cols)
        {
           return this.Select<T>(StartIndex, MaxRecord, out TotalRecord, Cols);
        }
        public bool Insert(T Model)
        {
            return Insert(BindColumnValues(Model).ToArray());
        }
        public bool Update(T Model)
        {
            return Update(BindColumnValues(Model).ToArray());
        }
        public new ORMCmd<T> Distinct
        {
            get { _Distinct = true; return this; }
        }
        public new ORMCmd<T> ListCmd(params ODACmd[] Cmds)
        {
            return (ORMCmd<T>)base.ListCmd(Cmds);
        }
        public new ORMCmd<T> RightJoin(ODACmd JoinCmd, params IODAColumns[] On)
        {
            return (ORMCmd<T>)base.RightJoin(JoinCmd, On);
        }
        public new ORMCmd<T> LeftJoin(ODACmd JoinCmd, params IODAColumns[] On)
        {
            return (ORMCmd<T>)base.LeftJoin(JoinCmd, On);
        }
        public new ORMCmd<T> InnerJoin(ODACmd JoinCmd, params IODAColumns[] On)
        {
            return (ORMCmd<T>)base.InnerJoin(JoinCmd, On);
        }
        public new ORMCmd<T> StartWithConnectBy(string StartWithExpress, string ConnectBy, string Prior, string ConnectColumn, string ConnectStr, int MaxLevel)
        {
            return (ORMCmd<T>)base.StartWithConnectBy(StartWithExpress, ConnectBy, Prior, ConnectColumn, ConnectStr, MaxLevel);
        }
        public new ORMCmd<T> OrderbyAsc(params IODAColumns[] Columns)
        {
            return (ORMCmd<T>)base.OrderbyAsc(Columns);
        }
        public new ORMCmd<T> OrderbyDesc(params IODAColumns[] Columns)
        {
            return (ORMCmd<T>)base.OrderbyDesc(Columns);
        }
        public new ORMCmd<T> Groupby(params IODAColumns[] Columns)
        {
            return (ORMCmd<T>)base.Groupby(Columns);
        }
        public new ORMCmd<T> Having(params IODAColumns[] Condition)
        {
            return (ORMCmd<T>)base.Having(Condition);
        }
        public new ORMCmd<T> Where(params IODAColumns[] Condition)
        {
            return (ORMCmd<T>)base.Where(Condition);
        }
        public new ORMCmd<T> And(params IODAColumns[] Condition)
        {
            return (ORMCmd<T>)base.And(Condition);
        }
        public new ORMCmd<T> Or(params IODAColumns[] Condition)
        {
            return (ORMCmd<T>)base.Or(Condition);
        }
    }
}
