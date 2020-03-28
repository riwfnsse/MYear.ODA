using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace MYear.ODA
{
    public partial class ODACmd : IODACmd //: IDBScriptGenerator
    {

        #region ODA指令寄存器   
        private string _StartWithExpress = null;
        private string _ConnectByParent = null;
        private string _PriorChild = null;
        private string _ConnectStr = "";
        private string _ConnectColumn = null;
        private int _MaxLevel = 32;

        protected bool _Distinct = false;
        protected List<IODAColumns> _WhereList = new List<IODAColumns>();
        protected List<IODAColumns> _OrList = new List<IODAColumns>();
        protected List<SqlOrderbyScript> _Orderby = new List<SqlOrderbyScript>();
        protected List<IODAColumns> _Groupby = new List<IODAColumns>();
        protected List<IODAColumns> _Having = new List<IODAColumns>();
        protected List<ODACmd> _ListCmd = new List<ODACmd>();
        protected List<SqlJoinScript> _JoinCmd = new List<SqlJoinScript>();
        protected List<SqlUnionScript> _UnionCmd = new List<SqlUnionScript>();


        protected virtual ODACmd BaseCmd { get { return null; } }
        protected virtual string DataBaseId { get { return null; } }


        #endregion

        #region 基础信息
        private string _DBObjectMap = string.Empty;
        /// <summary>
        /// 命令名称
        /// </summary>
        public virtual string CmdName
        {
            get;
        }
        /// <summary>
        /// 命令名称
        /// </summary>
        public virtual string Schema
        {
            get;set;
        }
        /// <summary>
        /// 确定输入项长度
        /// </summary>
        Encoding IODACmd.DBCharSet { get; set; }
        GetDBAccessHandler IODACmd.GetDBAccess { get; set; }
        Func<string> IODACmd.GetAlias { get; set; }
        string IODACmd.DBObjectMap
        {
            get { return this.DBObjectMap; }
            set
            {
                this.DBObjectMap = value;
            }
        }
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        /// <summary>
        /// 操作的表名
        /// 用作分表
        ///   CmdName：没有分表的情况下就是表名
        ///   当对表[CmdName]纵向切割出N个分表时，DBObjectMap是根据路由条件临时给出表名 
        /// </summary> 
        protected virtual string DBObjectMap
        {
            get
            {
                string TableName = string.IsNullOrWhiteSpace(_DBObjectMap) ? this.CmdName : _DBObjectMap;
                if (!string.IsNullOrWhiteSpace(Schema))
                {
                    TableName = Schema + "." + TableName;
                }
                return TableName;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _DBObjectMap = value;
            }
        } 
        protected virtual GetDBAccessHandler GetDBAccess
        {
            get
            {
                return ((IODACmd)this).GetDBAccess;
            }
            set
            {
                ((IODACmd)this).GetDBAccess = value;
            }
        }
        /// <summary>
        /// 获取表别名或Function 参数命称
        /// </summary>
        internal Func<string> GetAlias
        {
            get
            {
                return ((IODACmd)this).GetAlias;
            }
            set
            {
                ((IODACmd)this).GetAlias = value;
            }
        }
        #endregion

        #region ODA应用语法定义 
        /// <summary>
        /// 查询去除重复
        /// </summary>
        public virtual ODACmd Distinct
        {
            get
            {
                _Distinct = true;
                return this;
            }
        }

        /// <summary>
        /// 新建查询函数
        /// </summary>
        public IODAFunction Function
        {
            get
            {
               return new ODAFunction(this);
            }
        }

        /// <summary>
        /// 这个表的所有字段,即 "*"
        /// </summary>
        public ODAColumns AllColumn
        {
            get
            {
                return new ODAColumns(this, "*");
            }
        }
         
        /// <summary>
        /// 转换成子查询
        /// </summary>
        /// <param name="Cols">子查询的字段</param>
        /// <returns></returns>
        public virtual ODAView ToView(params IODAColumns[] Cols)
        {
            return new ODAView(this, Cols)
            {
                GetDBAccess = this.GetDBAccess,
                GetAlias = this.GetAlias,
                Alias = this.GetAlias(), 
            };
        }
        /// <summary>
        /// 内连接查询,形如：select * from table1 t1,table2 t2
        /// </summary>
        /// <param name="Cmds">要连接的表</param>
        /// <returns></returns>
        public virtual ODACmd ListCmd(params ODACmd[] Cmds)
        {
            for (int i = 0; i < Cmds.Length; i++)
            {
                if (Cmds[i] == this)
                    throw new ODAException(10000, "ListCmd 对象不能是本身"); 
                _ListCmd.Add(Cmds[i]);
            }
            return this;
        }
        /// <summary>
        ///  左连接查询 
        /// </summary>
        /// <param name="JoinCmd">要连接的表</param>
        /// <param name="On">连接条件</param>
        /// <returns></returns>
        public virtual ODACmd LeftJoin(ODACmd JoinCmd, params IODAColumns[] On)
        {
            return Join(JoinCmd, " LEFT JOIN ", On);
        }
        /// <summary>
        ///  右连接查询 
        /// </summary>
        /// <param name="JoinCmd">要连接的表</param>
        /// <param name="On">连接条件</param>
        /// <returns></returns>
        public virtual ODACmd RightJoin(ODACmd JoinCmd, params IODAColumns[] On)
        {
            return Join(JoinCmd, " RIGHT JOIN ", On);
        }
        /// <summary>
        ///  内连接查询 
        /// </summary>
        /// <param name="JoinCmd">要连接的表</param>
        /// <param name="On">连接条件</param>
        /// <returns></returns>
        public virtual ODACmd InnerJoin(ODACmd JoinCmd, params IODAColumns[] On)
        {
            return Join(JoinCmd, " INNER JOIN ", On);
        }
        /// <summary>
        /// 连接查询
        /// </summary>
        /// <param name="JoinCmd"></param>
        /// <param name="Join"></param>
        /// <param name="On"></param>
        /// <returns></returns>
        protected virtual ODACmd Join(ODACmd JoinCmd, string Join, params IODAColumns[] On)
        {
            if (JoinCmd == this)
                throw new ODAException(10002, "Inner Join Instance Can't be itselft"); 
            JoinCmd.Where(On);
            _JoinCmd.Add(new SqlJoinScript() { JoinCmd = JoinCmd, JoinScript = Join });
            return this;
        }

        /// <summary>
        /// 递归查询
        /// </summary>
        /// <param name="StartWithExpress">递归入口条件表达式,如 ColumnName = 0</param>
        /// <param name="ConnectBy">递归父字段名称,如：ColumnParent </param>
        /// <param name="Prior">递归子字段名称,如ColumnChild</param>
        /// <param name="ConnectColumn">递归路径字段名称，用来返加父子层级关系</param>
        /// <param name="ConnectStr">父子层级之间的连接字符</param>
        /// <param name="MaxLevel">最大递归深度,最大递归深度不超过32层</param>
        /// <returns></returns>
        public virtual ODACmd StartWithConnectBy(string StartWithExpress, string ConnectBy, string Prior, string ConnectColumn, string ConnectStr, int MaxLevel)
        {
            if (MaxLevel > 32)
                throw new ODAException(10003, "MaxLevel should be smaller than  32");
            if (string.IsNullOrWhiteSpace(ConnectBy) || string.IsNullOrWhiteSpace(Prior) || string.IsNullOrWhiteSpace(StartWithExpress))
                throw new ODAException(10004, "StartWithExpress and ConnectByParent and PriorChild Can't be Empty");
            _StartWithExpress = StartWithExpress;
            _ConnectColumn = ConnectColumn;
            _ConnectByParent = ConnectBy;
            _PriorChild = Prior;
            _ConnectStr = ConnectStr;
            _MaxLevel = MaxLevel;
            return this;
        }
        public virtual ODACmd OrderbyAsc(params IODAColumns[] Columns)
        {
            if (Columns != null)
                for (int i = 0; i < Columns.Length; i++)
                    _Orderby.Add(new SqlOrderbyScript() { OrderbyCol = Columns[i], OrderbyScript = " ASC " });
            return this;
        }
        public virtual ODACmd OrderbyDesc(params IODAColumns[] Columns)
        {
            if (Columns != null)
                for (int i = 0; i < Columns.Length; i++)
                    _Orderby.Add(new SqlOrderbyScript() { OrderbyCol = Columns[i], OrderbyScript = " DESC " });
            return this;
        }
        public virtual ODACmd Groupby(params IODAColumns[] Columns)
        {
            _Groupby.AddRange(Columns);
            return this;
        }
        public virtual ODACmd Having(params IODAColumns[] Condition)
        {
            _Having.AddRange(Condition);
            return this;
        }
        public virtual ODACmd Where(params IODAColumns[] Condition)
        {
            _WhereList.AddRange(Condition);
            return this;
        }
        public virtual ODACmd And(params IODAColumns[] Condition)
        {
            _WhereList.AddRange(Condition);
            return this;
        }
        public virtual ODACmd Or(params IODAColumns[] Condition)
        {
            if (_WhereList.Count == 0)
                throw new ODAException(10005, "Where Condition is null,Add Where first");
            _OrList.AddRange(Condition);
            return this;
        }

        public virtual ODACmd Union(ODAView ODAView)
        {
            if (ODAView._Orderby.Count > 0)
            {
                throw new ODAException(10020, "There is [Order by] expression  in Union ODACmd !"); 
            }
            _UnionCmd.Add(new SqlUnionScript() { UnionCmd = ODAView, JoinScript = " UNION " }); 
            return this;
        }
        public virtual ODACmd UnionAll(ODAView ODAView)
        {
            if (ODAView._Orderby.Count > 0)
            {
                throw new ODAException(10020, "There is [Order by] expression  in Union ODACmd !"); 
            }
            _UnionCmd.Add(new SqlUnionScript() { UnionCmd = ODAView, JoinScript = " UNION ALL " });
            return this;
        }
        #endregion

        #region SQL语句生成

        /// <summary>
        /// Cmd命令本身是一个查询语句
        /// </summary>
        /// <param name="DBObject"></param>
        /// <returns></returns>
        protected virtual ODAScript GetCmdSql()
        {
            var sql = new ODAScript();
            sql.SqlScript.Append(this.DBObjectMap);
            sql.TableList.Add(this.DBObjectMap);
            return sql;
        }
        /// <summary>
        /// 获取查询语主中 form 字符串及变量
        /// </summary>
        /// <param name="SubSql"></param>
        /// <returns></returns>
        protected virtual ODAScript GetFromSubString()
        { 
            var sql = new ODAScript();
            sql.SqlScript.Append(" FROM "); 
            var cmdsql = GetCmdSql();
            sql.Merge(cmdsql);
            string AliasSql = String.IsNullOrWhiteSpace(Alias) ? "" : " " + Alias;
            sql.SqlScript.Append(AliasSql);

            for (int i = 0; i < _JoinCmd.Count; i++)
            {
                sql.SqlScript.Append(_JoinCmd[i].JoinScript);
                var join = _JoinCmd[i].JoinCmd.GetCmdSql();
                sql.Merge(join);　
                sql.SqlScript.Append(" ").Append(_JoinCmd[i].JoinCmd.Alias); 
               var wsql = GetWhereSubSql(_JoinCmd[i].JoinCmd._WhereList, " AND ");
                if (wsql.SqlScript.Length > 0)
                {
                    sql.SqlScript.Append(" ON ");
                    sql.Merge(wsql);
                }　
            }
            for (int i = 0; i < _ListCmd.Count; i++)
            {
                var lsql = _ListCmd[i].GetCmdSql();
                sql.SqlScript.Append(",");
                sql.Merge(lsql).SqlScript.Append(" ").Append(_ListCmd[i].Alias);
            } 
            return sql;
        }
        /// <summary>
        /// 获取查询语主中 where 字符串有变量
        /// </summary>
        /// <param name="ConditionList"></param>
        /// <param name="RelationStr"></param>
        /// <param name="SubSql"></param>
        /// <returns></returns>
        protected virtual ODAScript GetWhereSubSql(List<IODAColumns> ConditionList, string RelationStr)
        {
            var sql = new ODAScript();
            if (ConditionList == null || ConditionList.Count == 0)
                return sql; 
            List<ODAParameter> ParamsList = new List<ODAParameter>();
          
            foreach (IODAColumns W in ConditionList)
            {
                var sub = W.GetWhereSubstring();
                sql.Merge(sub);
                sql.SqlScript.Append(RelationStr); 
            }
            if (sql.SqlScript.Length > 0)
                sql.SqlScript.Remove(sql.SqlScript.Length - RelationStr.Length, RelationStr.Length); 
            return sql; 
        }

        /// <summary>
        /// 获取查询语主中 select 字符串有变量
        /// </summary>
        /// <param name="ConnectStr"></param>
        /// <param name="SubSql"></param>
        /// <param name="ColList"></param>
        /// <returns></returns>
        protected virtual ODAParameter[] GetSelectColumns(string ConnectStr, out string SubSql, params IODAColumns[] ColList)
        {
            string Sql = "";
            List<ODAParameter> ParamList = new List<ODAParameter>();
            foreach (IODAColumns Col in ColList)
            {
                string SubSelectSql = "";
                ODAParameter[] prms = Col.GetSelectColumn(out SubSelectSql);
                ParamList.AddRange(prms);
                Sql += SubSelectSql + ConnectStr;
            }
            SubSql = string.IsNullOrEmpty(Sql) ? "" : Sql.Substring(0, Sql.Length - ConnectStr.Length);
            return ParamList.ToArray();
        }

        /// <summary>
        ///  获取查询语主中 GroupBy 字符串有变量
        /// </summary>
        /// <param name="ColList"></param>
        /// <returns></returns>
        protected ODAScript GetGroupByColumns(params IODAColumns[] ColList)
        {
            var sql = new ODAScript();
            if (ColList == null || ColList.Length == 0)
                return sql;

            foreach (IODAColumns Col in ColList)
            {
                string OSql = "";
                var prms = Col.GetSelectColumn(out OSql);
                sql.ParamList.AddRange(prms);
                sql.SqlScript.Append(OSql + ",");
            }
            sql.SqlScript.Remove(sql.SqlScript.Length - 1, 1);
            return sql;
        }

        /// <summary>
        ///  获取查询语主中 Orderby 字符串有变量
        /// </summary>
        /// <param name="OrderbyList"></param>
        /// <returns></returns>
        protected virtual ODAScript GetOrderbyColumns(params SqlOrderbyScript[] OrderbyList)
        {
            var sql = new ODAScript();
            if (OrderbyList == null || OrderbyList.Length == 0)
                return sql; 
            for (int i = 0; i < OrderbyList.Length; i++)
            {
                string oSql = "";
                var prms = OrderbyList[i].OrderbyCol.GetSelectColumn(out oSql);
                sql.OrderBy.Append(oSql); 
                sql.OrderBy.Append(OrderbyList[i].OrderbyScript + ",");
                sql.ParamList.AddRange(prms); 
            }
            sql.OrderBy.Remove(sql.OrderBy.Length - 1, 1);
            return sql;
        }

        /// <summary>
        /// 生成统计数据行数的SQL
        /// </summary>
        /// <param name="CountSql"></param>
        /// <param name="Col"></param>
        /// <returns></returns>
        protected virtual ODAScript GetCountSql(IODAColumns Col)
        {
            if (_Groupby.Count > 0 || _Having.Count > 0)
                throw new ODAException(10006, "Do not count the [Group by] cmd,You should probably use [ ToView(Columns).Count()] instead.");
        
            ODAScript sql = new ODAScript()
            {
                ScriptType = SQLType.Select,
                DataBaseId = this.DataBaseId,
            };
            if (_Distinct)
                sql.SqlScript.Append("SELECT COUNT(DISTINCT ");
            else
                sql.SqlScript.Append("SELECT COUNT(");
            if (System.Object.ReferenceEquals(Col, null))
            {
                sql.SqlScript.Append("*");
            }
            else
            {
                string SubSelectSql = "";
                ODAParameter[] SubSelectPrms = GetSelectColumns(",", out SubSelectSql, Col);
                sql.SqlScript.Append(SubSelectSql); 
                if (SubSelectPrms != null && SubSelectPrms.Length > 0)
                    sql.ParamList.AddRange(SubSelectPrms);
            }
            sql.SqlScript.Append(") AS TOTAL_RECORD");
            var fSql = GetFromSubString();
            sql.Merge(fSql);

            if (_WhereList.Count > 0 || _OrList.Count > 0)
            {
                sql.SqlScript.Append(" WHERE ");
                var asql = GetWhereSubSql(_WhereList, " AND ");
                sql.Merge(asql);
                if (_OrList.Count > 0)
                {
                    if (_WhereList.Count > 0)
                        sql.SqlScript.Append(" OR ");
                    var osql = GetWhereSubSql(_OrList, " OR ");
                    sql.Merge(osql);
                }
            }
          
            return sql;
        }

        /// <summary>
        /// 生成查询语句
        /// </summary>
        /// <param name="SelectSql">sql脚本</param>
        /// <param name="Cols">变量列表及变操作符</param>
        /// <returns>变量列表</returns>
        public virtual ODAScript GetSelectSql( params IODAColumns[] Cols)
        { 
            ODAScript sql = new ODAScript()
            {
                ScriptType = SQLType.Select,
            };
            if (_Distinct)
                sql.SqlScript.Append("SELECT DISTINCT ");
            else
                sql.SqlScript.Append("SELECT "); 

            if (Cols == null || Cols.Length == 0)
            {
                sql.SqlScript.Append(" * ");
            }
            else
            {
                string SubSelectSql = "";
                ODAParameter[] SubSelectPrms = GetSelectColumns(",", out SubSelectSql, Cols);
                sql.SqlScript.Append(SubSelectSql);
                if (SubSelectPrms != null && SubSelectPrms.Length > 0)
                    sql.ParamList.AddRange(SubSelectPrms);
            } 
            var fSql = this.GetFromSubString();
            sql.Merge(fSql);

            if (_WhereList.Count > 0 || _OrList.Count > 0)
            {
                sql.SqlScript.Append(" WHERE ");
                var asql = GetWhereSubSql(_WhereList, " AND ");
                sql.Merge(asql);
                if (_OrList.Count > 0)
                {
                    if (_WhereList.Count > 0)
                        sql.SqlScript.Append(" OR ");
                    var osql = GetWhereSubSql(_OrList, " OR ");
                    sql.Merge(osql);
                }
            }
            if (_Groupby.Count > 0)
            {
                var gy = GetGroupByColumns(_Groupby.ToArray());
                sql.SqlScript.Append(" GROUP BY ");
                sql.Merge(gy);
            }
            if (_Having.Count > 0)
            {
                var hsql = GetWhereSubSql(_Having, " AND ");
                sql.SqlScript.Append(" HAVING ");
                sql.Merge(hsql);
            }

            if (_UnionCmd.Count > 0)
            {
                for (int i = 0; i < _UnionCmd.Count; i++)
                {
                    sql.SqlScript.AppendLine(" ");
                    sql.SqlScript.AppendLine(_UnionCmd[i].JoinScript);
                    var Union = _UnionCmd[i].UnionCmd.GetCmdSql(); 
                    Union.SqlScript.Remove(0, 1);
                    Union.SqlScript.Remove(Union.SqlScript.Length - 1, 1);
                    sql.Merge(Union); 
                }
            }

            if (_Orderby.Count > 0)
            {
                var oy = GetOrderbyColumns(_Orderby.ToArray());
                sql.OrderBy.Clear();
                sql.OrderBy.Append(" ORDER BY ");
                sql.OrderBy.Append(oy.OrderBy.ToString()); 
                sql.SqlScript.Append(sql.OrderBy.ToString()); 
                sql.Merge(oy);
            }
            return sql;  
        }
        /// <summary>
        /// 生成删除语
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        protected virtual ODAScript GetDeleteSql()
        {
            ODAScript sql = new ODAScript()
            {
                ScriptType = SQLType.Delete,
                DataBaseId = this.DataBaseId
            };
            sql.TableList.Add(this.DBObjectMap);
            sql.SqlScript.Append("DELETE FROM ").Append(this.DBObjectMap);


            if (!string.IsNullOrWhiteSpace(Alias))
            {
                Alias = "";//SQLite 不支持delete from 别名
                //sql.SqlScript.Append(" ").Append(Alias);
            }
            if (_WhereList.Count > 0 || _OrList.Count > 0)
            {
                sql.SqlScript.Append(" WHERE ");
                var asql = GetWhereSubSql(_WhereList, " AND ");
                sql.Merge(asql);
                if (_OrList.Count > 0)
                {
                    if (_WhereList.Count > 0)
                        sql.SqlScript.Append(" OR ");
                    var osql = GetWhereSubSql(_OrList, " OR ");
                    sql.Merge(osql);
                }
            }
            return sql; 
        }

        /// <summary>
        /// 生成插入语句
        /// </summary>
        /// <param name="Sql">脚本</param>
        /// <param name="Cols">变量列表及变操作符</param>
        /// <returns>变量列表</returns>
        protected virtual ODAScript GetInsertSql(params IODAColumns[] Cols)
        {
            if(Cols == null || Cols.Length ==0)
                throw new ODAException(10018, "NO Columns for Insert!");
            this.Alias = "";
            ODAScript sql = new ODAScript()
            {
                ScriptType = SQLType.Insert,
                DataBaseId = this.DataBaseId
            };
            sql.TableList.Add(this.DBObjectMap);
            sql.SqlScript.Append("INSERT INTO ").Append(this.DBObjectMap).Append("(");

            List <ODAParameter> ParamList = new List<ODAParameter>();
            string Column = "";
            string ColumnParams = "";
            for (int i = 0; i < Cols.Length; i++)
            {
                string ColumnTmp = "";
                string ColumnParamsTmp = "";
                sql.ParamList.AddRange(((IODAColumns)Cols[i]).GetInsertSubstring(out ColumnTmp, out ColumnParamsTmp));
                Column += ColumnTmp + ",";
                ColumnParams += ColumnParamsTmp + ",";
            }
            sql.SqlScript.Append(Column.Remove(Column.Length - 1, 1)).Append(") VALUES (").Append(ColumnParams.Remove(ColumnParams.Length - 1, 1)).Append(")");
            return sql;
        }

        /// <summary>
        /// 生成update语句
        /// </summary>
        /// <param name="Sql">脚本</param>
        /// <param name="Cols">变量列表及变操作符</param>
        /// <returns>变量列表</returns>
        protected virtual ODAScript GetUpdateSql(params IODAColumns[] Cols)
        {
            if(Cols == null || Cols.Length ==0)
                throw new ODAException(10019, "NO Columns for update!");
            this.Alias = "";
            ODAScript sql = new ODAScript()
            {
                ScriptType = SQLType.Update,
                DataBaseId = this.DataBaseId
            };
            sql.TableList.Add(this.DBObjectMap);
            sql.SqlScript.Append("UPDATE ").Append(this.DBObjectMap).Append(" SET "); 
            string Column = "";
            for (int i = 0; i < Cols.Length; i++)
            {
                string ColumnTmp = "";
                ODAParameter[] P = Cols[i].GetUpdateSubstring(out ColumnTmp);
                if (P != null)
                    sql.ParamList.AddRange(P);
                Column += ColumnTmp + ",";
            }
            sql.SqlScript.Append(Column.Remove(Column.Length - 1, 1));
            if (_WhereList.Count > 0 || _OrList.Count > 0)
            {
                sql.SqlScript.Append(" WHERE ");
                var asql = GetWhereSubSql(_WhereList, " AND ");
                sql.Merge(asql);
                if (_OrList.Count > 0)
                {
                    if (_WhereList.Count > 0)
                        sql.SqlScript.Append(" OR ");
                    var osql = GetWhereSubSql(_OrList, " OR ");
                    sql.Merge(osql);
                }
            }
            return sql; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Cols"></param>
        /// <returns></returns>
        protected virtual ODAParameter[] GetProcedureSql(out string Sql, params IODAColumns[] Params)
        {
            List<ODAParameter> ParamList = new List<ODAParameter>();
            Sql = this.DBObjectMap;
            for (int i = 0; i < Params.Length; i++)
            {
                ParamList.Add(Params[i].GetProcedureParams());
            }
            return ParamList.ToArray();
        }

        #endregion

        #region 执行SQL语句  
        /// <summary>
        /// 在数据库中执行select count 语句,返统计结果
        /// </summary>
        /// <param name="Col"></param>
        /// <returns></returns>
        public virtual int Count(IODAColumns Col = null)
        {
            try
            { 
                return CountRecords(Col);
            }
            finally
            {
                this.Clear();
            }
        }
        /// <summary>
        /// 在数据库中执行select count 语句,返统计结果
        /// </summary>
        /// <param name="Col"></param>
        /// <returns></returns>
        protected virtual int CountRecords(IODAColumns Col = null)
        {
            var sql = this.GetCountSql(Col);
            var db = this.GetDBAccess(sql);
            if (db == null)
                throw new ODAException(10007, "ODACmd Count 没有执行程序"); 
            object[] vl = db.SelectFirst(sql.SqlScript.ToString(), sql.ParamList.ToArray());
            return int.Parse(vl[0].ToString());
        }

        /// <summary>
        /// 在数据库中执行select语句,并返回结果集
        /// </summary>
        /// <param name="Cols">要select的字段</param>
        /// <returns></returns>
        public virtual DataTable Select(params IODAColumns[] Cols)
        {
            try
            {
                var sql = this.GetSelectSql(Cols);
                var db = this.GetDBAccess(sql);
                if (db == null)
                    throw new ODAException(10008, "ODACmd Select 没有执行程序");
                 
                if (string.IsNullOrEmpty(_StartWithExpress) || string.IsNullOrEmpty(_ConnectByParent) || string.IsNullOrEmpty(_PriorChild))
                {
                    return db.Select(sql.SqlScript.ToString(), sql.ParamList.ToArray());
                }
                else
                {
                    return db.Select(sql.SqlScript.ToString(), sql.ParamList.ToArray(), _StartWithExpress, _ConnectByParent, _PriorChild, _ConnectColumn, _ConnectStr, _MaxLevel);
                }
            }
            finally
            {
                this.Clear();
            }
        }

        /// <summary>
        /// 查询数据（取前多少条记录）
        /// </summary>
        /// <param name="StartIndex">起始记录的位置</param>
        /// <param name="MaxRecord">返回最大的记录数</param>
        /// <param name="TotalRecord">查询得到的总记录数</param>
        /// <param name="Cols">查询的字段，可为空，空则返回所有字段</param>
        /// <returns></returns>
        public virtual DataTable Select(int StartIndex, int MaxRecord, out int TotalRecord, params IODAColumns[] Cols)
        {
            try
            {
                var sql = this.GetSelectSql(Cols);
                var db = this.GetDBAccess(sql);
                if (db == null)
                    throw new ODAException(10008, "ODACmd Select 没有执行程序"); 
                if (string.IsNullOrEmpty(_StartWithExpress) || string.IsNullOrEmpty(_ConnectByParent) || string.IsNullOrEmpty(_PriorChild))
                {
                    if ((_Groupby.Count > 0 || _Having.Count > 0) 
                        || (Cols.Length > 0 && _Distinct)
                        || (_UnionCmd.Count > 0)
                        )
                    {
                        if (_Orderby.Count > 0)
                        {
                            SqlOrderbyScript[] orderbys = _Orderby.ToArray();
                            _Orderby.Clear();
                            TotalRecord = this.ToView(Cols).CountRecords();
                            _Orderby.AddRange(orderbys);
                        }
                        else
                        {
                            TotalRecord = this.ToView(Cols).CountRecords();
                        }
                    }
                    else
                    {
                        if (_Orderby.Count > 0)
                        {
                            SqlOrderbyScript[] orderbys = _Orderby.ToArray();
                            _Orderby.Clear();
                            TotalRecord = this.CountRecords();
                            _Orderby.AddRange(orderbys);
                        }
                        else
                        {
                            TotalRecord = this.CountRecords();
                        }
                    }
                    return db.Select(sql.SqlScript.ToString(), sql.ParamList.ToArray(), StartIndex, MaxRecord, sql.OrderBy.ToString());
                }
                else
                {
                    DataTable dt = db.Select(sql.SqlScript.ToString(), sql.ParamList.ToArray(), _StartWithExpress, _ConnectByParent, _PriorChild, _ConnectColumn, _ConnectStr, _MaxLevel);
                    TotalRecord = dt.Rows.Count;
                    DataTable dtrtl = dt.Clone();
                    for (int i = StartIndex; i < StartIndex + MaxRecord; i++)
                    {
                        if (dt.Rows.Count > i)
                            dtrtl.Rows.Add(dt.Rows[i].ItemArray);
                        else
                            break;
                    }
                    return dtrtl;
                }
            }
            finally
            {
                this.Clear();
            }
        }

        /// <summary>
        ///  查询数据并转换为对象列表
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="Cols">查询的字段，可为空，空则返回所有字段</param>
        /// <returns></returns>
        public virtual List<T> Select<T>(params IODAColumns[] Cols) where T : class
        {
            try
            {
                ODAScript sql = this.GetSelectSql(Cols);
                IDBAccess db = this.GetDBAccess(sql);
                if (db == null)
                    throw new ODAException(10009, "ODACmd Select 没有执行程序");
                var prms = sql.ParamList.ToArray();
                if (string.IsNullOrEmpty(_StartWithExpress) || string.IsNullOrEmpty(_ConnectByParent) || string.IsNullOrEmpty(_PriorChild))
                {
                    return db.Select<T>(sql.SqlScript.ToString(), prms);
                }
                else
                {
                    return db.Select<T>(sql.SqlScript.ToString(), prms, _StartWithExpress, _ConnectByParent, _PriorChild, _ConnectColumn, _ConnectStr, _MaxLevel);
                }
            }
            finally
            {
                this.Clear(); 
            }
        }
        /// <summary>
        /// 查询数据并转换为对象列表（取前多少条记录）
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="StartIndex">起始记录的位置</param>
        /// <param name="MaxRecord">返回最大的记录数</param>
        /// <param name="TotalRecord">查询得到的总记录数</param>
        /// <param name="Cols">查询的字段，可为空，空则返回所有字段</param>
        /// <returns></returns>
        public virtual List<T> Select<T>(int StartIndex, int MaxRecord, out int TotalRecord, params IODAColumns[] Cols) where T : class
        {
            try
            {
                var sql = this.GetSelectSql(Cols);
                var db = this.GetDBAccess(sql);
                if (db == null)
                    throw new ODAException(10010, "ODACmd Select 没有执行程序");
                var prms = sql.ParamList.ToArray();
                if (string.IsNullOrEmpty(_StartWithExpress) || string.IsNullOrEmpty(_ConnectByParent) || string.IsNullOrEmpty(_PriorChild))
                {
                    if ((_Groupby.Count > 0 || _Having.Count > 0) || (Cols.Length > 0 && _Distinct))
                    {
                        if (_Orderby.Count > 0)
                        {
                            SqlOrderbyScript[] orderbys = _Orderby.ToArray();
                            _Orderby.Clear();
                            TotalRecord = this.ToView(Cols).CountRecords();
                            _Orderby.AddRange(orderbys);
                        }
                        else
                        {
                            TotalRecord = this.ToView(Cols).CountRecords();
                        }
                    }
                    else
                    {
                        if (_Orderby.Count > 0)
                        {
                            SqlOrderbyScript[] orderbys = _Orderby.ToArray();
                            _Orderby.Clear();
                            TotalRecord = this.CountRecords();
                            _Orderby.AddRange(orderbys);
                        }
                        else
                        {
                            TotalRecord = this.CountRecords();
                        }
                    }
                    return db.Select<T>(sql.SqlScript.ToString(), prms, StartIndex, MaxRecord, sql.OrderBy.ToString());
                }
                else
                {
                    DataTable dt = db.Select(sql.SqlScript.ToString(), prms, _StartWithExpress, _ConnectByParent, _PriorChild, _ConnectColumn, _ConnectStr, _MaxLevel);
                    TotalRecord = dt.Rows.Count;
                    DataTable dtrtl = dt.Clone();
                    for (int i = StartIndex; i < StartIndex + MaxRecord; i++)
                    {
                        if (dt.Rows.Count > i)
                            dtrtl.Rows.Add(dt.Rows[i].ItemArray);
                        else
                            break;
                    }
                    return DBAccess.ConvertToList<T>(dtrtl);
                }
            }
            finally
            {
                this.Clear();
            }
        }
        /// <summary>
        /// 查询数据，接参数参顺序返回第一行的数据
        /// </summary>
        /// <param name="Cols">要查询的字段</param>
        /// <returns>第一行的数据据</returns>
        public object[] SelectFirst(params IODAColumns[] Cols)
        {
            try
            {
                var sql = this.GetSelectSql(Cols);
                var db = this.GetDBAccess(sql);
                if (db == null)
                    throw new ODAException(10011, "ODACmd Select 没有执行程序");
                var prms = sql.ParamList.ToArray();
                return db.SelectFirst(sql.SqlScript.ToString(), prms);
            }
            finally
            {
                this.Clear();
            }
        }
        /// <summary>
        /// 批量导入数据
        /// </summary>
        /// <param name="Data">源数据</param>
        /// <param name="Prms">数据表对应的字段（Data.Row[n][ColumnIndex]与Prms[ColumnIndex]对应）</param>
        /// <returns></returns>
        public virtual bool Import(DataTable Data, ODAParameter[] Prms)
        {
            try
            {
                ODAScript sql = new ODAScript()
                {
                    ScriptType = SQLType.Import,
                };
                sql.TableList.Add(this.CmdName);
                sql.ParamList.AddRange(Prms);
                sql.SqlScript.Append(this.CmdName);
                Data.TableName = this.CmdName;

                foreach (ODAParameter p in Prms)
                {
                    if (!Data.Columns.Contains(p.ColumnName))
                    {
                        throw new ODAException(10020, "Data Should be contain Column [" + p.ColumnName + "]");
                    }
                } 
                var db = this.GetDBAccess(sql);
                if (db == null)
                    throw new ODAException(10012, "ODACmd Import 没有执行程序");
                return db.Import(Data,Prms);
            }
            finally
            {
                this.Clear();
            }
        } 
        /// <summary>
        /// 在数据库中执行Delete 语句
        /// </summary>
        /// <returns></returns>
        public virtual bool Delete()
        {
            try
            {
                var sql = this.GetDeleteSql();
                var db = this.GetDBAccess(sql);
                if (db == null)
                    throw new ODAException(10013, "ODACmd Delete 没有执行程序"); 
                var prms = sql.ParamList.ToArray();
                return db.ExecuteSQL(sql.SqlScript.ToString(), prms) > 0;
            }
            finally
            {
                this.Clear();
            }
        }
        /// <summary>
        /// 在数据库中执行update 语句
        /// </summary>
        /// <param name="Cols">需要更新的字段及其值</param>
        /// <returns></returns>
        public virtual bool Update(params IODAColumns[] ColumnValues)
        {
            try
            {
                var sql = this.GetUpdateSql(ColumnValues);
                var db = this.GetDBAccess(sql);
                if (db == null)
                    throw new ODAException(10014, "ODACmd Update 没有执行程序");
                var prms = sql.ParamList.ToArray();
                return db.ExecuteSQL(sql.SqlScript.ToString(), prms) > 0;
            }
            finally
            {
                this.Clear();
            }
        }

        /// <summary>
        /// 在数据库中执行insert 语句
        /// </summary>
        /// <param name="Cols">插件的字段及其值</param>
        /// <returns></returns>
        public virtual bool Insert(params IODAColumns[] ColumnValues)
        {
            try
            {
                var sql = this.GetInsertSql(ColumnValues);
                var db = this.GetDBAccess(sql);
                if (db == null)
                    throw new ODAException(10015, "ODACmd Update 没有执行程序");
                var prms = sql.ParamList.ToArray();
                return db.ExecuteSQL(sql.SqlScript.ToString(), prms) > 0;
            }
            finally
            {
                this.Clear();
            }
        }
        /// <summary>
        /// insert () select
        /// </summary>
        /// <param name="SelectCmd"></param>
        /// <param name="Columns">select的字段</param>
        /// <returns></returns>
        public virtual bool Insert(ODACmd SelectCmd, params IODAColumns[] Columns)
        {
            try
            {
                var sql = new ODAScript()
                {
                    ScriptType = SQLType.Insert,
                };
                sql.SqlScript.Append("INSERT INTO ").Append(this.CmdName).Append("(");
                string Column = "";
                for (int i = 0; i < Columns.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(Columns[i].AliasName))
                    {
                        Column += Columns[i].ColumnName + ",";
                    }
                    else
                    {
                        Column += Columns[i].AliasName + ",";
                    }
                }
                sql.SqlScript.Append(Column.Remove(Column.Length - 1, 1)).Append(") ");
                var sSql = SelectCmd.GetSelectSql(Columns);
                sql.Merge(sSql);

                var db = this.GetDBAccess(sql);
                if (db == null)
                    throw new ODAException(10016, "ODACmd Insert 没有执行程序");
                var prms = sql.ParamList.ToArray();
                return db.ExecuteSQL(sql.SqlScript.ToString(), prms) > 0;
            }
            finally
            {
                this.Clear();
            }
        }

        /// <summary>
        /// 在数据库中Procedure
        /// </summary>
        /// <param name="Params">存储过程的参数及其值</param>
        /// <returns></returns>
        public virtual DataSet Procedure(params IODAColumns[] Params)
        {
            try
            {
                string sqlScript = "";
                var prms = this.GetProcedureSql(out sqlScript, Params);
                var sql = new ODAScript()
                {
                    ScriptType = SQLType.Procedure,
                };
                sql.ParamList.AddRange(prms);
                sql.TableList.Add(sqlScript);
                var db = this.GetDBAccess(sql);
                if (db == null)
                    throw new ODAException(10017, "ODACmd Procedure 没有执行程序");
                return db.ExecuteProcedure(sqlScript, prms);
            }
            finally
            {
                this.Clear();
            }
        }

        /// <summary>
        /// 验证更新值是否合法
        /// </summary>
        /// <param name="Cols"></param>
        protected virtual void ValidColumn(params IODAColumns[] Cols)
        {
            StringBuilder sbr = new StringBuilder();
            foreach (IODAColumns c in Cols)
            {
                if (c.IsRequired)
                {
                    if (c.CompareValue == null || c.CompareValue == System.DBNull.Value || (c.DBDataType == ODAdbType.OVarchar && String.IsNullOrWhiteSpace(c.CompareValue.ToString())))
                    {
                        sbr.AppendLine((String.IsNullOrWhiteSpace(c.ColumnComment) ? c.ColumnName : c.ColumnComment) + "不能为空;");
                    }
                }
                if (c.DBDataType == ODAdbType.OVarchar && (c.CompareValue.ToString().Length > c.Size))
                    sbr.AppendLine((String.IsNullOrWhiteSpace(c.ColumnComment) ? c.ColumnName : c.ColumnComment) + "超出长度限制");
            }
            if (!string.IsNullOrWhiteSpace(sbr.ToString()))
            {
                sbr.Insert(0, "修改数据错误，");
                throw new ODAException(10018, sbr.ToString());
            }
        }
        public virtual void Clear()
        {
            //Alias = "";
            _StartWithExpress = null;
            _ConnectByParent = null;
            _PriorChild = null;
            _ConnectStr = "";
            _ConnectColumn = null;
            _MaxLevel = 32;
            _Distinct = false;
            _WhereList.Clear();
            _OrList.Clear();
            _Orderby.Clear();
            _Groupby.Clear();
            _Having.Clear();

            foreach (ODACmd cmdl in _ListCmd)
                cmdl.Clear(); 
            _ListCmd.Clear();

            foreach (var cmdj in _JoinCmd)
                cmdj.JoinCmd.Clear();
            _JoinCmd.Clear();
        }
        #endregion
    }
}