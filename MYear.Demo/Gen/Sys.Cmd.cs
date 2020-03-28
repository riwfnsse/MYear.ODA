using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using MYear.ODA;
using MYear.ODA.Model;

namespace MYear.ODA.Cmd
{
	internal partial class CmdSysDataDictionary:ORMCmd<SYS_DATA_DICTIONARY>
	{
		 public ODAColumns ColStatus{ get { return new ODAColumns(this, "STATUS", ODAdbType.OChar, 1,true ); } }
		 public ODAColumns ColCreatedBy{ get { return new ODAColumns(this, "CREATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColCreatedDate{ get { return new ODAColumns(this, "CREATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColLastUpdatedBy{ get { return new ODAColumns(this, "LAST_UPDATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColLastUpdatedDate{ get { return new ODAColumns(this, "LAST_UPDATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColTableCode{ get { return new ODAColumns(this, "TABLE_CODE", ODAdbType.OChar, 36,true ); } }
		 public ODAColumns ColColumnCode{ get { return new ODAColumns(this, "COLUMN_CODE", ODAdbType.OChar, 36,true ); } }
		 public ODAColumns ColObjName{ get { return new ODAColumns(this, "OBJ_NAME", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColObjDesc{ get { return new ODAColumns(this, "OBJ_DESC", ODAdbType.OVarchar, 2000,false ); } }
		 public ODAColumns ColZhcnName{ get { return new ODAColumns(this, "ZHCN_NAME", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColEnusName{ get { return new ODAColumns(this, "ENUS_NAME", ODAdbType.OVarchar, 80,false ); } }
		 public override string CmdName { get { return "SYS_DATA_DICTIONARY"; }}
		 public override List<ODAColumns> GetColumnList() 
		 { 
			 return new List<ODAColumns>() { ColStatus,ColCreatedBy,ColCreatedDate,ColLastUpdatedBy,ColLastUpdatedDate,ColTableCode,ColColumnCode,ColObjName,ColObjDesc,ColZhcnName,ColEnusName};
		 }
	}
	internal partial class CmdSysResource:ORMCmd<SYS_RESOURCE>
	{
		 public ODAColumns ColStatus{ get { return new ODAColumns(this, "STATUS", ODAdbType.OChar, 1,true ); } }
		 public ODAColumns ColCreatedBy{ get { return new ODAColumns(this, "CREATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColCreatedDate{ get { return new ODAColumns(this, "CREATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColLastUpdatedBy{ get { return new ODAColumns(this, "LAST_UPDATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColLastUpdatedDate{ get { return new ODAColumns(this, "LAST_UPDATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColId{ get { return new ODAColumns(this, "ID", ODAdbType.OChar, 36,true ); } }
		 public ODAColumns ColResourceName{ get { return new ODAColumns(this, "RESOURCE_NAME", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColResourceDesc{ get { return new ODAColumns(this, "RESOURCE_DESC", ODAdbType.OVarchar, 2000,false ); } }
		 public ODAColumns ColResourceType{ get { return new ODAColumns(this, "RESOURCE_TYPE", ODAdbType.OChar, 36,true ); } }
		 public ODAColumns ColResourceScope{ get { return new ODAColumns(this, "RESOURCE_SCOPE", ODAdbType.OChar, 36,true ); } }
		 public ODAColumns ColResourceLocation{ get { return new ODAColumns(this, "RESOURCE_LOCATION", ODAdbType.OVarchar, 600,true ); } }
		 public ODAColumns ColParentId{ get { return new ODAColumns(this, "PARENT_ID", ODAdbType.OVarchar, 36,false ); } }
		 public ODAColumns ColParentId1{ get { return new ODAColumns(this, "PARENT_ID1", ODAdbType.OVarchar, 36,false ); } }
		 public ODAColumns ColParentId2{ get { return new ODAColumns(this, "PARENT_ID2", ODAdbType.OVarchar, 36,false ); } }
		 public ODAColumns ColParentId3{ get { return new ODAColumns(this, "PARENT_ID3", ODAdbType.OVarchar, 36,false ); } }
		 public ODAColumns ColParentId4{ get { return new ODAColumns(this, "PARENT_ID4", ODAdbType.OVarchar, 36,false ); } }
		 public ODAColumns ColParentId5{ get { return new ODAColumns(this, "PARENT_ID5", ODAdbType.OVarchar, 36,false ); } }
		 public ODAColumns ColParentId6{ get { return new ODAColumns(this, "PARENT_ID6", ODAdbType.OVarchar, 36,false ); } }
		 public ODAColumns ColResourceIndex{ get { return new ODAColumns(this, "RESOURCE_INDEX", ODAdbType.ODecimal, 22,false ); } }
		 public override string CmdName { get { return "SYS_RESOURCE"; }}
		 public override List<ODAColumns> GetColumnList() 
		 { 
			 return new List<ODAColumns>() { ColStatus,ColCreatedBy,ColCreatedDate,ColLastUpdatedBy,ColLastUpdatedDate,ColId,ColResourceName,ColResourceDesc,ColResourceType,ColResourceScope,ColResourceLocation,ColParentId,ColParentId1,ColParentId2,ColParentId3,ColParentId4,ColParentId5,ColParentId6,ColResourceIndex};
		 }
	}
	internal partial class CmdSysRole:ORMCmd<SYS_ROLE>
	{
		 public ODAColumns ColStatus{ get { return new ODAColumns(this, "STATUS", ODAdbType.OChar, 1,true ); } }
		 public ODAColumns ColCreatedBy{ get { return new ODAColumns(this, "CREATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColCreatedDate{ get { return new ODAColumns(this, "CREATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColLastUpdatedBy{ get { return new ODAColumns(this, "LAST_UPDATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColLastUpdatedDate{ get { return new ODAColumns(this, "LAST_UPDATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColRoleCode{ get { return new ODAColumns(this, "ROLE_CODE", ODAdbType.OChar, 36,true ); } }
		 public ODAColumns ColRoleName{ get { return new ODAColumns(this, "ROLE_NAME", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColRoleDesc{ get { return new ODAColumns(this, "ROLE_DESC", ODAdbType.OVarchar, 2000,false ); } }
		 public override string CmdName { get { return "SYS_ROLE"; }}
		 public override List<ODAColumns> GetColumnList() 
		 { 
			 return new List<ODAColumns>() { ColStatus,ColCreatedBy,ColCreatedDate,ColLastUpdatedBy,ColLastUpdatedDate,ColRoleCode,ColRoleName,ColRoleDesc};
		 }
	}
	internal partial class CmdSysRoleAuthorization:ORMCmd<SYS_ROLE_AUTHORIZATION>
	{
		 public ODAColumns ColStatus{ get { return new ODAColumns(this, "STATUS", ODAdbType.OChar, 1,true ); } }
		 public ODAColumns ColCreatedBy{ get { return new ODAColumns(this, "CREATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColCreatedDate{ get { return new ODAColumns(this, "CREATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColLastUpdatedBy{ get { return new ODAColumns(this, "LAST_UPDATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColLastUpdatedDate{ get { return new ODAColumns(this, "LAST_UPDATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColRoleCode{ get { return new ODAColumns(this, "ROLE_CODE", ODAdbType.OChar, 36,true ); } }
		 public ODAColumns ColResourceId{ get { return new ODAColumns(this, "RESOURCE_ID", ODAdbType.OChar, 36,true ); } }
		 public ODAColumns ColIsForbidden{ get { return new ODAColumns(this, "IS_FORBIDDEN", ODAdbType.OChar, 1,true ); } }
		 public override string CmdName { get { return "SYS_ROLE_AUTHORIZATION"; }}
		 public override List<ODAColumns> GetColumnList() 
		 { 
			 return new List<ODAColumns>() { ColStatus,ColCreatedBy,ColCreatedDate,ColLastUpdatedBy,ColLastUpdatedDate,ColRoleCode,ColResourceId,ColIsForbidden};
		 }
	}
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
	internal partial class CmdSysUserAuthorization:ORMCmd<SYS_USER_AUTHORIZATION>
	{
		 public ODAColumns ColStatus{ get { return new ODAColumns(this, "STATUS", ODAdbType.OChar, 1,true ); } }
		 public ODAColumns ColCreatedBy{ get { return new ODAColumns(this, "CREATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColCreatedDate{ get { return new ODAColumns(this, "CREATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColLastUpdatedBy{ get { return new ODAColumns(this, "LAST_UPDATED_BY", ODAdbType.OVarchar, 80,false ); } }
		 public ODAColumns ColLastUpdatedDate{ get { return new ODAColumns(this, "LAST_UPDATED_DATE", ODAdbType.ODatetime, 7,false ); } }
		 public ODAColumns ColUserAccount{ get { return new ODAColumns(this, "USER_ACCOUNT", ODAdbType.OChar, 36,true ); } }
		 public ODAColumns ColResourceId{ get { return new ODAColumns(this, "RESOURCE_ID", ODAdbType.OChar, 36,true ); } }
		 public ODAColumns ColIsForbidden{ get { return new ODAColumns(this, "IS_FORBIDDEN", ODAdbType.OChar, 1,true ); } }
		 public override string CmdName { get { return "SYS_USER_AUTHORIZATION"; }}
		 public override List<ODAColumns> GetColumnList() 
		 { 
			 return new List<ODAColumns>() { ColStatus,ColCreatedBy,ColCreatedDate,ColLastUpdatedBy,ColLastUpdatedDate,ColUserAccount,ColResourceId,ColIsForbidden};
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
    internal partial class CmdSysFiles : ORMCmd<SYS_FILES>
    {
        public ODAColumns ColId { get { return new ODAColumns(this, "ID", ODAdbType.OVarchar, 72, true); } }
        public ODAColumns ColDatetimeCreated { get { return new ODAColumns(this, "DATETIME_CREATED", ODAdbType.ODatetime, 8, false); } }
        public ODAColumns ColUserCreated { get { return new ODAColumns(this, "USER_CREATED", ODAdbType.OVarchar, 160, false); } }
        public ODAColumns ColUserModified { get { return new ODAColumns(this, "USER_MODIFIED", ODAdbType.OVarchar, 160, false); } }
        public ODAColumns ColDatetimeModified { get { return new ODAColumns(this, "DATETIME_MODIFIED", ODAdbType.ODatetime, 8, false); } }
        public ODAColumns ColClientType { get { return new ODAColumns(this, "CLIENT_TYPE", ODAdbType.OVarchar, 64, false); } }
        public ODAColumns ColFileBody { get { return new ODAColumns(this, "FILE_BODY", ODAdbType.OBinary, 16, false); } }
        public ODAColumns ColFileName { get { return new ODAColumns(this, "FILE_NAME", ODAdbType.OVarchar, 480, false); } }
        public ODAColumns ColFilePath { get { return new ODAColumns(this, "FILE_PATH", ODAdbType.OVarchar, 480, false); } }
        public ODAColumns ColFileVersion { get { return new ODAColumns(this, "FILE_VERSION", ODAdbType.OVarchar, 160, false); } }
        public ODAColumns ColMd5 { get { return new ODAColumns(this, "MD5", ODAdbType.OVarchar, 480, false); } }
        public ODAColumns ColRevision { get { return new ODAColumns(this, "REVISION", ODAdbType.OInt, 4, false); } }
        public ODAColumns ColState { get { return new ODAColumns(this, "STATE", ODAdbType.OVarchar, 2, false); } }
        public ODAColumns ColDatetimeFileModified { get { return new ODAColumns(this, "DATETIME_FILE_MODIFIED", ODAdbType.ODatetime, 8, false); } }
        public ODAColumns ColDatetimeFileCreated { get { return new ODAColumns(this, "DATETIME_FILE_CREATED", ODAdbType.ODatetime, 8, false); } }
        public override string CmdName { get { return "SYS_FILES"; } }
        public override List<ODAColumns> GetColumnList()
        {
            return new List<ODAColumns>() { ColId, ColDatetimeCreated, ColUserCreated, ColUserModified, ColDatetimeModified, ColClientType, ColFileBody, ColFileName, ColFilePath, ColFileVersion, ColMd5, ColRevision, ColState, ColDatetimeFileModified, ColDatetimeFileCreated };
        }
    }
}
