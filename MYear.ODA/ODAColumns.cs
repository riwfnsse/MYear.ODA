using System;
using System.Collections.Generic;
using System.Data;

namespace MYear.ODA
{

    /// <summary>
    /// ODA字段信息
    /// </summary>
    public class ODAColumns : IODAColumns
    { 
        private CmdFuntion _Fun = CmdFuntion.NONE;
        private List<SqlColumnScript> _SqlColumnList = new List<SqlColumnScript>();
        private object _CompareValue = null;
        private int _Size = 0;
        private string _ColumnComment = "";
        private bool _IsRequired = false;
        private string _AliasColumnName = null;
        private ParameterDirection _PDirection = ParameterDirection.Input;
        private ODACmd _InCmd = null;
        private ODAColumns _InColumn = null;
        protected CmdConditionSymbol _Symbol = CmdConditionSymbol.NONE;
        protected string _ColumnName = "*";
        protected IODACmd _Cmd = null;
        protected ODAdbType _DBDataType = ODAdbType.OChar; 
        public object CompareValue
        {
            get
            {
                return _CompareValue;
            }
        }
        public string AliasName
        {
            get
            {
                return _AliasColumnName;
            }
        }
        public ODAdbType DBDataType { get { return _DBDataType; } }
        public int Size { get { return _Size; } }
        
        /// <summary>
        /// 创建ODA字段，默认 OVarchar（2000）,
        /// </summary>
        /// <param name="Cmd"></param>
        /// <param name="ColumnName"></param>
        public ODAColumns(ODACmd Cmd, string ColumnName)
        {
            this.ODAColumnsInit(Cmd, ColumnName, ODAdbType.OVarchar, 2000, null, false, ParameterDirection.Input);
        }
        public ODAColumns(ODACmd Cmd, string ColumnName, ODAdbType DBDataType)
        {
            this.ODAColumnsInit(Cmd, ColumnName, DBDataType, 2000, null, false, ParameterDirection.Input);
        }
        public ODAColumns(ODACmd Cmd, string ColumnName, ODAdbType DBDataType, int Size)
        {
            this.ODAColumnsInit(Cmd, ColumnName, DBDataType, Size, null, false, ParameterDirection.Input);
        }
        public ODAColumns(ODACmd Cmd, string ColumnName, ODAdbType DBDataType, int Size, bool IsRequired)
        {
            this.ODAColumnsInit(Cmd, ColumnName, DBDataType, Size, null, IsRequired, ParameterDirection.Input);
        }
        public ODAColumns(ODACmd Cmd, string ColumnName, ODAdbType DBDataType, int Size, string ColumnComment, bool IsRequired, ParameterDirection Direction)
        {
            this.ODAColumnsInit(Cmd, ColumnName, DBDataType, Size, ColumnComment, IsRequired, Direction);
        }
        private void ODAColumnsInit(ODACmd Cmd, string ColumnName, ODAdbType DBDataType, int Size, string ColumnComment, bool IsRequired, ParameterDirection Direction)
        {
            if (Cmd == null)
                throw new ODAException(20003, string.IsNullOrEmpty(_ColumnComment) ? _ColumnName : _ColumnComment + "Cmd can't be null");
            _Cmd = Cmd;
            if (!String.IsNullOrEmpty(ColumnName))
                _ColumnName = ColumnName;
            _DBDataType = DBDataType;
            _Size = Size;
            _ColumnComment = ColumnComment;
            _PDirection = Direction;
            _IsRequired = IsRequired;
        }
       
        #region SQL 脚本生成
        string IODAColumns.GetColumnName()
        {
            return this.GetColumnName();
        }
        ODAParameter[] IODAColumns.GetSelectColumn(out string SubSql)
        {
            return this.GetSelectColumn(out SubSql);
        }
        ODAParameter[] IODAColumns.GetInsertSubstring(out string SubSql, out string SubSqlParams)
        {
            return GetInsertSubstring(out SubSql, out SubSqlParams);
        }
        ODAParameter[] IODAColumns.GetUpdateSubstring(out string SubSql)
        {
            return this.GetUpdateSubstring(out SubSql);
        }
        ODAScript IODAColumns.GetWhereSubstring()
        {
            return this.GetWhereSubstring();
        }
        ODAParameter IODAColumns.GetProcedureParams()
        {
            return this.GetProcedureParams();
        } 
        ODAColumns IODAColumns.SetParamDataType(ODAdbType ParamType)
        {
            _DBDataType = ParamType;
            return this;
        }
        bool IODAColumns.IsRequired { get { return _IsRequired; } }
        string IODAColumns.ColumnComment { get { return _ColumnComment; } }
        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
        }
        public virtual string ODAColumnName
        {
            get
            {
                return string.IsNullOrEmpty(_Cmd.Alias) ? _ColumnName : _Cmd.Alias + "." + _ColumnName;
            }
        }

        protected virtual string GetColumnName()
        {
            string SColumn = "";
            switch (_Fun)
            {
                case CmdFuntion.AVG:
                    SColumn = "AVG(" + this.ODAColumnName + ")";
                    break;
                case CmdFuntion.COUNT:
                    SColumn = "COUNT(" + this.ODAColumnName + ")";
                    break;
                case CmdFuntion.MAX:
                    SColumn = "MAX(" + this.ODAColumnName + ")";
                    break;
                case CmdFuntion.MIN:
                    SColumn = "MIN(" + this.ODAColumnName + ")";
                    break;
                case CmdFuntion.SUM:
                    SColumn = "SUM(" + this.ODAColumnName + ")";
                    break;
                case CmdFuntion.ASCII:
                    SColumn = "ASCII(" + this.ODAColumnName + ")";
                    break;
                case CmdFuntion.LENGTH:
                    SColumn = "LENGTH(" + this.ODAColumnName + ")";
                    break;
                case CmdFuntion.LTRIM:
                    SColumn = "LTRIM(" + this.ODAColumnName + ")";
                    break;
                case CmdFuntion.RTRIM:
                    SColumn = "RTRIM(" + this.ODAColumnName + ")";
                    break;
                case CmdFuntion.TRIM:
                    SColumn = "TRIM(" + this.ODAColumnName + ")";
                    break;
                case CmdFuntion.UPPER:
                    SColumn = "UPPER(" + this.ODAColumnName + ")";
                    break;
                case CmdFuntion.LOWER:
                    SColumn = "LOWER(" + this.ODAColumnName + ")";
                    break;
                default:
                    SColumn = this.ODAColumnName;
                    break;
            }
            return SColumn;
        }
        protected virtual ODAParameter[] GetSelectColumn(out string SubSql)
        {
            string SColumn = GetColumnName();
            if (!string.IsNullOrEmpty(_AliasColumnName))
                SubSql = SColumn + " AS " + _AliasColumnName;
            else
                SubSql = SColumn;
            return new ODAParameter[0];
        }
        protected virtual ODAParameter[] GetInsertSubstring(out string SubSql, out string SubSqlParams)
        {
            ODAParameter P = new ODAParameter();
            string PName = ODAParameter.ODAParamsMark + _Cmd.GetAlias();
            P.ColumnName = _ColumnName;
            P.ParamsName = PName;
            P.ParamsValue = _CompareValue == null ? System.DBNull.Value : _CompareValue;
            P.DBDataType = _DBDataType;
            P.Direction = ParameterDirection.Input;
            P.Size = _Size;
            SubSql = _ColumnName;
            SubSqlParams = P.ParamsName;
            return new ODAParameter[] { P };
        }
        protected virtual ODAParameter[] GetUpdateSubstring(out string SubSql)
        {
            SubSql = "";
            List<ODAParameter> PList = new List<ODAParameter>();
            switch (_Symbol)
            {
                case CmdConditionSymbol.EQUAL:
                    SubSql = _ColumnName + "=";
                    break;
                case CmdConditionSymbol.ADD:
                    SubSql = _ColumnName + " + ";
                    break;  
                case CmdConditionSymbol.REDUCE:
                    SubSql = _ColumnName + " - ";
                    break;
                case CmdConditionSymbol.TAKE:
                    SubSql = _ColumnName + " * ";
                    break;
                case CmdConditionSymbol.REMOVE:
                    SubSql = _ColumnName + " / ";
                    break;
                case CmdConditionSymbol.STAY:
                    SubSql = _ColumnName + " % ";
                    break;
                default:
                    throw new ODAException(20004, (string.IsNullOrEmpty(_ColumnComment) ? _ColumnName : _ColumnComment) + "CmdConditionSymbol Errror");
            }
            if (_CompareValue is ODAColumns)
            {
                string SubSqltmp = "";
                if (((ODAColumns)_CompareValue)._Symbol == CmdConditionSymbol.NONE)
                {
                    SubSql += ((ODAColumns)_CompareValue).ODAColumnName;
                }
                else
                {
                    ODAParameter[] p = ((IODAColumns)_CompareValue).GetUpdateSubstring(out SubSqltmp);
                    PList.AddRange(p);
                    SubSql += SubSqltmp;
                }
            }
            else
            {
                ODAParameter P = new ODAParameter();
                string PName = ODAParameter.ODAParamsMark + _Cmd.GetAlias();

                P.ColumnName = _ColumnName;
                P.ParamsName = PName;
                P.ParamsValue = _CompareValue == null ? System.DBNull.Value : _CompareValue;
                P.DBDataType = _DBDataType;
                P.Direction = _PDirection;
                P.Size = _Size;

                SubSql += P.ParamsName;
                PList.Add(P);
            }
            return PList.ToArray();
        }
        protected virtual ODAScript GetWhereSubstring()
        {
            ODAScript sql = new ODAScript();
            sql.SqlScript.Append(GetColumnName());
            if (_CompareValue is ODAColumns)
            {
                switch (_Symbol)
                {
                    case CmdConditionSymbol.BIGGER:
                        sql.SqlScript.Append(" > ");
                        break;
                    case CmdConditionSymbol.EQUAL:
                        sql.SqlScript.Append(" = ");
                        break;
                    case CmdConditionSymbol.LIKE:
                        sql.SqlScript.Append(" LIKE ");
                        break;
                    case CmdConditionSymbol.NOTLIKE:
                        sql.SqlScript.Append(" NOT LIKE ");
                        break;
                    case CmdConditionSymbol.NOTBIGGER:
                        sql.SqlScript.Append(" <= ");
                        break;
                    case CmdConditionSymbol.NOTEQUAL:
                        sql.SqlScript.Append(" <> ");
                        break;
                    case CmdConditionSymbol.NOTSMALLER:
                        sql.SqlScript.Append(" >= ");
                        break;
                    case CmdConditionSymbol.SMALLER:
                        sql.SqlScript.Append(" < ");
                        break;
                    case CmdConditionSymbol.ADD:
                        sql.SqlScript.Append(" + ");
                        break; 
                    case CmdConditionSymbol.REDUCE:
                        sql.SqlScript.Append(" - ");
                        break;
                    case CmdConditionSymbol.TAKE:
                        sql.SqlScript.Append(" * ");
                        break;
                    case CmdConditionSymbol.REMOVE:
                        sql.SqlScript.Append(" / ");
                        break;
                    case CmdConditionSymbol.STAY:
                        sql.SqlScript.Append(" % ");
                        break;
                    default:
                        throw new ODAException(20005, string.IsNullOrEmpty(_ColumnComment) ? _ColumnName + " not assign" : _ColumnComment + "CmdConditionSymbol Errror");
                }
                if (((ODAColumns)_CompareValue)._Symbol == CmdConditionSymbol.NONE)
                {
                    sql.SqlScript.Append(((ODAColumns)_CompareValue).ODAColumnName);
                }
                else
                {
                    var sub = ((IODAColumns)_CompareValue).GetWhereSubstring();
                    sql.Merge(sub);
                }
            }
            else
            {
                ODAParameter param = new ODAParameter();
                string PName = ODAParameter.ODAParamsMark + _Cmd.GetAlias();
                param.ColumnName = this.ODAColumnName;
                param.ParamsName = PName;
                param.ParamsValue = _CompareValue == null ? System.DBNull.Value : _CompareValue;
                param.DBDataType = _DBDataType;
                param.Direction = _PDirection;
                param.Size = _Size;
                switch (_Symbol)
                {
                    case CmdConditionSymbol.BIGGER:
                        sql.SqlScript.Append(" > ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break;
                    case CmdConditionSymbol.EQUAL:
                        sql.SqlScript.Append(" = ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break;
                    case CmdConditionSymbol.IN:
                        if (_InCmd == null && _CompareValue != System.DBNull.Value)
                        {
                            object[] ValueList = (object[])_CompareValue;
                            string paramName = _Cmd.GetAlias();
                            sql.SqlScript.Append(" IN (");
                            for (int k = 0; k < ValueList.Length; k++)
                            {
                                ODAParameter paramSub = new ODAParameter();
                                paramSub.ColumnName = this.ODAColumnName;
                                paramSub.ParamsName = ODAParameter.ODAParamsMark + paramName + "_" + k.ToString();
                                paramSub.ParamsValue = ValueList[k];
                                paramSub.DBDataType = _DBDataType;
                                paramSub.Direction = _PDirection;
                                paramSub.Size = _Size;
                                sql.SqlScript.Append(paramSub.ParamsName).Append(",");
                                sql.ParamList.Add(paramSub);
                            }
                            sql.SqlScript.Remove(sql.SqlScript.Length - 1, 1).Append(")");
                        }
                        else
                        {
                            var subSql = ((ODACmd)_InCmd).GetSelectSql(_InColumn);
                            string inSql = " IN ( " + subSql.SqlScript.ToString() + ")";
                            subSql.SqlScript.Clear();
                            subSql.SqlScript.Append(inSql);
                            sql.Merge(subSql);
                        }
                        break;
                    case CmdConditionSymbol.NOTIN:
                        if (_InCmd == null && _CompareValue != System.DBNull.Value)
                        {
                            object[] ValueList = (object[])_CompareValue;
                            sql.SqlScript.Append(" NOT IN (");
                            string paramName = _Cmd.GetAlias();
                            for (int k = 0; k < ValueList.Length; k++)
                            {
                                ODAParameter paramSub = new ODAParameter();
                                paramSub.ColumnName = this.ODAColumnName;
                                paramSub.ParamsName = ODAParameter.ODAParamsMark + paramName + "_" + k.ToString();
                                paramSub.ParamsValue = ValueList[k];
                                paramSub.DBDataType = _DBDataType;
                                paramSub.Direction = _PDirection;
                                paramSub.Size = _Size;

                                sql.SqlScript.Append(paramSub.ParamsName).Append(",");
                                sql.ParamList.Add(paramSub);
                            }
                            sql.SqlScript.Remove(sql.SqlScript.Length - 1, 1).Append(")");
                        }
                        else
                        {
                            var subSql = ((ODACmd)_InCmd).GetSelectSql(_InColumn);
                            string inSql = " NOT IN ( " + subSql.SqlScript.ToString() + ")";
                            subSql.SqlScript.Clear();
                            subSql.SqlScript.Append(inSql);
                            sql.Merge(subSql);
                        }
                        break;
                    case CmdConditionSymbol.LIKE:
                        sql.SqlScript.Append(" LIKE ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break; 
                    case CmdConditionSymbol.NOTLIKE:
                        sql.SqlScript.Append(" NOT LIKE ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break;
                    case CmdConditionSymbol.NOTBIGGER:
                        sql.SqlScript.Append(" <= ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break;
                    case CmdConditionSymbol.NOTEQUAL:
                        sql.SqlScript.Append(" <>  ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break;
                    case CmdConditionSymbol.NOTSMALLER:
                        sql.SqlScript.Append(" >= ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break;
                    case CmdConditionSymbol.SMALLER:
                        sql.SqlScript.Append("  < ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break;
                    case CmdConditionSymbol.ISNOTNULL:
                        sql.SqlScript.Append(" IS NOT NULL ");
                        break;
                    case CmdConditionSymbol.ISNULL:
                        sql.SqlScript.Append(" IS NULL ");
                        break;
                    case CmdConditionSymbol.ADD:
                        sql.SqlScript.Append(" + ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break;
                    case CmdConditionSymbol.REDUCE:
                        sql.SqlScript.Append(" - ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break;
                    case CmdConditionSymbol.TAKE:
                        sql.SqlScript.Append(" * ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break;
                    case CmdConditionSymbol.REMOVE:
                        sql.SqlScript.Append(" / ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break;
                    case CmdConditionSymbol.STAY:
                        sql.SqlScript.Append(" % ").Append(param.ParamsName);
                        sql.ParamList.Add(param);
                        break;
                    default:
                        throw new ODAException(20006, string.IsNullOrWhiteSpace(_ColumnComment) ? _ColumnName + " not assign" : _ColumnComment + "CmdConditionSymbol Errror");
                }
            }

            var ar = new ODAScript();
            for (int i = 0; i < _SqlColumnList.Count; i++)
            {
                ar.SqlScript.Append(_SqlColumnList[i].ConnScript);
                var cc = _SqlColumnList[i].SqlColumn.GetWhereSubstring();
                ar.Merge(cc);
            }
            if (ar.SqlScript.Length > 0)
            {
                sql.Merge(ar);
                sql.SqlScript.Insert(0, "(").Append(")");
            }
            return sql;
        }
        protected virtual ODAParameter GetProcedureParams()
        {
            ODAParameter P = new ODAParameter();
            P.ColumnName = _ColumnName;
            P.ParamsName = _ColumnName;
            P.ParamsValue = _CompareValue == null ? System.DBNull.Value : _CompareValue;
            P.DBDataType = _DBDataType;
            P.Direction = _PDirection;
            P.Size = _Size;
            return P;
        }
        #endregion
        #region ODA 语法
        #region Function
        public ODAColumns Max
        {
            get
            {
                _Fun = CmdFuntion.MAX;
                return this;
            }
        }
        public ODAColumns Min
        {
            get
            {
                _Fun = CmdFuntion.MIN;
                return this;
            }
        }
        public ODAColumns Avg
        {
            get
            {
                _Fun = CmdFuntion.AVG;
                this._DBDataType = ODAdbType.ODecimal;
                return this;
            }
        }
        public virtual ODAColumns Count
        {
            get
            {
                _Fun = CmdFuntion.COUNT;
                this._DBDataType = ODAdbType.ODecimal;
                return this;
            }
        }
        public ODAColumns Sum
        {
            get
            {
                _Fun = CmdFuntion.SUM;
                this._DBDataType = ODAdbType.ODecimal;
                return this;
            }
        }
        public ODAColumns Length
        {
            get
            {
                _Fun = CmdFuntion.LENGTH;
                return this;
            }
        }
        public ODAColumns Ltrim
        {
            get
            {
                _Fun = CmdFuntion.LTRIM;
                return this;
            }
        }
        public ODAColumns Rtrim
        {
            get
            {
                _Fun = CmdFuntion.RTRIM;
                return this;
            }
        }
        public ODAColumns Upper
        {
            get
            {
                _Fun = CmdFuntion.UPPER;
                return this;
            }
        }
        public ODAColumns Lower
        {
            get
            {
                _Fun = CmdFuntion.LOWER;
                return this;
            }
        }
        public ODAColumns Trim
        {
            get
            {
                _Fun = CmdFuntion.TRIM;
                return this;
            }
        }
        public ODAColumns Ascii
        {
            get
            {
                _Fun = CmdFuntion.ASCII;
                return this;
            }
        }
        #endregion
        public ODAColumns As(string AliasColumnName)
        {
            _AliasColumnName = AliasColumnName;
            return this;
        }
    
        public ODAColumns SetCondition(CmdConditionSymbol Symbol, object Val)
        {
            if (Symbol == CmdConditionSymbol.NONE)
                throw new ODAException(20007, (string.IsNullOrEmpty(_ColumnComment) ? _ColumnName : _ColumnComment) + "Condition Symbol Should not be NONE");
            if (_Symbol != CmdConditionSymbol.NONE)
                throw new ODAException(20008, (string.IsNullOrEmpty(_ColumnComment) ? _ColumnName : _ColumnComment) + "Condition Symbol was setted");
            if (this.Equals(Val))
                throw new ODAException(200081, (string.IsNullOrEmpty(_ColumnComment) ? _ColumnName : _ColumnComment) + "Condition can't be  itself");
            _Symbol = Symbol;
            _CompareValue = Val == null ? System.DBNull.Value : Val;
            return this;
        }
        public ODAColumns IsNull
        {
            get
            {
                return SetCondition(CmdConditionSymbol.ISNULL, System.DBNull.Value);
            }
        }
        public ODAColumns IsNotNull
        {
            get
            {
                return SetCondition(CmdConditionSymbol.ISNOTNULL, System.DBNull.Value);
            }
        }
        public ODAColumns Like(object Val)
        {
            return SetCondition(CmdConditionSymbol.LIKE, Val);
        }
        public ODAColumns Contain(string Val)
        {
            return SetCondition(CmdConditionSymbol.LIKE,"%" + Val + "%" );
        }
        public ODAColumns ContainLeft(string Val)
        {
            return SetCondition(CmdConditionSymbol.LIKE, Val + "%");
        }
        public ODAColumns ContainRight(string Val)
        {
            return SetCondition(CmdConditionSymbol.LIKE, "%" + Val );
        }
        public ODAColumns NotLike(object Val)
        {
            return SetCondition(CmdConditionSymbol.NOTLIKE, Val);
        }
        public ODAColumns In(params object[] Val)
        {
            if (Val == null)
                throw new ODAException(20009, (string.IsNullOrEmpty(_ColumnComment) ? _ColumnName : _ColumnComment) + " In statement CompareValue Can't be null");
            if (Val.Length == 0)
                throw new ODAException(20010, (string.IsNullOrEmpty(_ColumnComment) ? _ColumnName : _ColumnComment) + " In statement  CompareValue Can't be empty");
            return SetCondition(CmdConditionSymbol.IN, Val);
        }
        public ODAColumns NotIn(params object[] Val)
        {
            if (Val == null)
                throw new ODAException(20011, (string.IsNullOrEmpty(_ColumnComment) ? _ColumnName : _ColumnComment) + "In statement  CompareValue Can't be null");
            if (Val.Length == 0)
                throw new ODAException(20012, (string.IsNullOrEmpty(_ColumnComment) ? _ColumnName : _ColumnComment) + "In statement CompareValue Can't be empty");
            return SetCondition(CmdConditionSymbol.NOTIN, Val);
        }
        public ODAColumns In(ODACmd Cmd, ODAColumns Col)
        {
            if (Cmd == null || System.Object.ReferenceEquals(Col, null))
                throw new ODAException(20013, "Cmd and Col Args Can't be null");
            _InColumn = Col;
            _InCmd = Cmd;
            return SetCondition(CmdConditionSymbol.IN, null);
        }
        public ODAColumns NotIn(ODACmd Cmd, ODAColumns Col)
        {
            if (Cmd == null || System.Object.ReferenceEquals(Col, null))
                throw new ODAException(20014, "Cmd and Col Args Can't be null");
            _InColumn = Col;
            _InCmd = Cmd;
            return SetCondition(CmdConditionSymbol.NOTIN, null);
        }
        public ODAColumns And(params ODAColumns[] Condition)
        {
            if (Condition != null)
            {
                foreach (ODAColumns cl in Condition)
                    _SqlColumnList.Add(new SqlColumnScript() { ConnScript = " AND ", SqlColumn = cl });
            }
            return this;
        }
        public ODAColumns Or(params ODAColumns[] Condition)
        {
            if (Condition != null)
            {
                foreach (ODAColumns cl in Condition)
                    _SqlColumnList.Add(new SqlColumnScript() { ConnScript = " OR ", SqlColumn = cl });
            }
            return this;
        }
        #region 符号重载
        public static ODAColumns operator ==(ODAColumns left, object CValue)
        {
            return left.SetCondition(CmdConditionSymbol.EQUAL, CValue);
        }
        public static ODAColumns operator !=(ODAColumns left, object CValue)
        {
            return left.SetCondition(CmdConditionSymbol.NOTEQUAL, CValue);
        }
        public static ODAColumns operator >(ODAColumns left, object CValue)
        {
            return left.SetCondition(CmdConditionSymbol.BIGGER, CValue);
        }
        public static ODAColumns operator <(ODAColumns left, object CValue)
        {
            return left.SetCondition(CmdConditionSymbol.SMALLER, CValue);
        }
        public static ODAColumns operator <=(ODAColumns left, object CValue)
        {
            return left.SetCondition(CmdConditionSymbol.NOTBIGGER, CValue);
        }
        public static ODAColumns operator >=(ODAColumns left, object CValue)
        {
            return left.SetCondition(CmdConditionSymbol.NOTSMALLER, CValue);
        }
        public static ODAColumns operator +(ODAColumns left, object CValue)
        {
            return left.SetCondition(CmdConditionSymbol.ADD, CValue);
        }
        public static ODAColumns operator -(ODAColumns left, object CValue)
        {
            return left.SetCondition(CmdConditionSymbol.REDUCE, CValue);
        }
        public static ODAColumns operator *(ODAColumns left, object CValue)
        {
            return left.SetCondition(CmdConditionSymbol.TAKE, CValue);
        }
        public static ODAColumns operator /(ODAColumns left, object CValue)
        {
            return left.SetCondition(CmdConditionSymbol.REMOVE, CValue);
        }
        public static ODAColumns operator %(ODAColumns left, object CValue)
        {
            return left.SetCondition(CmdConditionSymbol.STAY, CValue);
        }
        public static ODAColumns operator |(ODAColumns left, ODAColumns right)
        {
            return left.Or(right);
        }
        public static ODAColumns operator &(ODAColumns left, ODAColumns right)
        {
            return left.And(right);
        }
        #endregion

        #endregion
        private int _HashCode = 0;
        public override int GetHashCode()
        {
            if (_HashCode == 0)
                _HashCode = Guid.NewGuid().GetHashCode();
            return _HashCode;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ODAColumns))
                return false;
            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}