using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using MYear.ODA;
using MYear.ODA.Model;

namespace MYear.ODA.Cmd
{
	 
	internal partial class CmdSysUser:ORMCmd<SYS_USER>
	{
		 public ODAColumns ColStatus{ get { return new ODAColumns(this, "STATUS", ODAdbType.OChar, 1,true ); } }
		 public ODAColumns ColCreatedBy{ get { return new ODAColumns(this, "CREATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColCreatedDate{ get { return new ODAColumns(this, "CREATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColLastUpdatedBy{ get { return new ODAColumns(this, "LAST_UPDATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColLastUpdatedDate{ get { return new ODAColumns(this, "LAST_UPDATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColUserAccount{ get { return new ODAColumns(this, "USER_ACCOUNT", ODAdbType.OChar, 36,true ); } }
		 public ODAColumns ColUserName{ get { return new ODAColumns(this, "USER_NAME", ODAdbType.OVarchar, 80,true ); } }
		 public ODAColumns ColUserPassword{ get { return new ODAColumns(this, "USER_PASSWORD", ODAdbType.OVarchar, 200,false ); } }
		 public ODAColumns ColEmailAddr{ get { return new ODAColumns(this, "EMAIL_ADDR", ODAdbType.OVarchar, 200,false ); } }
		 public ODAColumns ColPhoneNo{ get { return new ODAColumns(this, "PHONE_NO", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColAddress{ get { return new ODAColumns(this, "ADDRESS", ODAdbType.OVarchar, 200,false ); } }
		 public ODAColumns ColFeMale{ get { return new ODAColumns(this, "FE_MALE", ODAdbType.OChar, 1,false ); } }
		 public ODAColumns ColFailTimes{ get { return new ODAColumns(this, "FAIL_TIMES", ODAdbType.ODecimal, 22,true ); } }
		 public ODAColumns ColIsLocked{ get { return new ODAColumns(this, "IS_LOCKED", ODAdbType.OChar, 1,true ); } }
		 public override string CmdName { get { return "SYS_USER"; }}
		 public override List<ODAColumns> GetColumnList() 
		 { 
			 return new List<ODAColumns>() { ColStatus,ColCreatedBy,ColCreatedDate,ColLastUpdatedBy,ColLastUpdatedDate,ColUserAccount,ColUserName,ColUserPassword,ColEmailAddr,ColPhoneNo,ColAddress,ColFeMale,ColFailTimes,ColIsLocked};
		 }
	} 
	internal partial class CmdSysUserRole:ORMCmd<SYS_USER_ROLE>
	{
		 public ODAColumns ColStatus{ get { return new ODAColumns(this, "STATUS", ODAdbType.OChar, 1,true ); } }
		 public ODAColumns ColCreatedBy{ get { return new ODAColumns(this, "CREATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColCreatedDate{ get { return new ODAColumns(this, "CREATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColLastUpdatedBy{ get { return new ODAColumns(this, "LAST_UPDATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColLastUpdatedDate{ get { return new ODAColumns(this, "LAST_UPDATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColUserAccount{ get { return new ODAColumns(this, "USER_ACCOUNT", ODAdbType.OChar, 36,true ); } }
		 public ODAColumns ColRoleCode{ get { return new ODAColumns(this, "ROLE_CODE", ODAdbType.OChar, 36,true ); } }
		 public override string CmdName { get { return "SYS_USER_ROLE"; }}
		 public override List<ODAColumns> GetColumnList() 
		 { 
			 return new List<ODAColumns>() { ColStatus,ColCreatedBy,ColCreatedDate,ColLastUpdatedBy,ColLastUpdatedDate,ColUserAccount,ColRoleCode};
		 }
	}

    internal partial class CmdTest : ORMCmd<Test>
    {
        public ODAColumns ColId { get { return new ODAColumns(this, "Id", ODAdbType.OInt, 4, true); } }
        public ODAColumns ColFByte { get { return new ODAColumns(this, "F_Byte", ODAdbType.OInt, 1, false); } }
        public ODAColumns ColFInt16 { get { return new ODAColumns(this, "F_Int16", ODAdbType.OInt, 2, false); } }
        public ODAColumns ColFInt32 { get { return new ODAColumns(this, "F_Int32", ODAdbType.OInt, 4, false); } }
        public ODAColumns ColFInt64 { get { return new ODAColumns(this, "F_Int64", ODAdbType.ODecimal, 8, false); } }
        public ODAColumns ColFDouble { get { return new ODAColumns(this, "F_Double", ODAdbType.ODecimal, 8, false); } }
        public ODAColumns ColFFloat { get { return new ODAColumns(this, "F_Float", ODAdbType.ODecimal, 4, false); } }
        public ODAColumns ColFDecimal { get { return new ODAColumns(this, "F_Decimal", ODAdbType.ODecimal, 9, false); } }
        public ODAColumns ColFBool { get { return new ODAColumns(this, "F_Bool", ODAdbType.OInt, 1, false); } }
        public ODAColumns ColFDatetime { get { return new ODAColumns(this, "F_DateTime", ODAdbType.ODatetime, 8, false); } }
        public ODAColumns ColFGuid { get { return new ODAColumns(this, "F_Guid", ODAdbType.OVarchar, 16, false); } }
        public ODAColumns ColFString { get { return new ODAColumns(this, "F_String", ODAdbType.OVarchar, 200, false); } }
        //public ODAColumns ColFBytes { get { return new ODAColumns(this, "F_Bytes", ODAdbType.OBinary, 16, false); } }
        public override string CmdName { get { return "TEST"; } }
        public override List<ODAColumns> GetColumnList()
        {
            return new List<ODAColumns>() { ColId, ColFByte, ColFInt16, ColFInt32, ColFInt64, ColFDouble, ColFFloat, ColFDecimal, ColFBool, ColFDatetime, ColFGuid, ColFString };
        }
    }
}
