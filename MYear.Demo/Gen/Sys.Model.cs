using System;

namespace MYear.ODA.Model
{
    public partial class SYS_DATA_DICTIONARY
	{
		 public string STATUS {get; set;}
		 public string CREATED_BY {get; set;}
		 public DateTime? CREATED_DATE {get; set;}
		 public string LAST_UPDATED_BY {get; set;}
		 public DateTime? LAST_UPDATED_DATE {get; set;}
		 public string TABLE_CODE {get; set;}
		 public string COLUMN_CODE {get; set;}
		 public string OBJ_NAME {get; set;}
		 public string OBJ_DESC {get; set;}
		 public string ZHCN_NAME {get; set;}
		 public string ENUS_NAME {get; set;}
	}
	public partial class SYS_RESOURCE
	{
		 public string STATUS {get; set;}
		 public string CREATED_BY {get; set;}
		 public DateTime? CREATED_DATE {get; set;}
		 public string LAST_UPDATED_BY {get; set;}
		 public DateTime? LAST_UPDATED_DATE {get; set;}
		 public string ID {get; set;}
		 public string RESOURCE_NAME {get; set;}
		 public string RESOURCE_DESC {get; set;}
		 public string RESOURCE_TYPE {get; set;}
		 public string RESOURCE_SCOPE {get; set;}
		 public string RESOURCE_LOCATION {get; set;}
		 public string PARENT_ID {get; set;}
		 public string PARENT_ID1 {get; set;}
		 public string PARENT_ID2 {get; set;}
		 public string PARENT_ID3 {get; set;}
		 public string PARENT_ID4 {get; set;}
		 public string PARENT_ID5 {get; set;}
		 public string PARENT_ID6 {get; set;}
		 public decimal? RESOURCE_INDEX {get; set;}
	}
	public partial class SYS_ROLE
	{
		 public string STATUS {get; set;}
		 public string CREATED_BY {get; set;}
		 public DateTime? CREATED_DATE {get; set;}
		 public string LAST_UPDATED_BY {get; set;}
		 public DateTime? LAST_UPDATED_DATE {get; set;}
		 public string ROLE_CODE {get; set;}
		 public string ROLE_NAME {get; set;}
		 public string ROLE_DESC {get; set;}
	}
	public partial class SYS_ROLE_AUTHORIZATION
	{
		 public string STATUS {get; set;}
		 public string CREATED_BY {get; set;}
		 public DateTime? CREATED_DATE {get; set;}
		 public string LAST_UPDATED_BY {get; set;}
		 public DateTime? LAST_UPDATED_DATE {get; set;}
		 public string ROLE_CODE {get; set;}
		 public string RESOURCE_ID {get; set;}
		 public string IS_FORBIDDEN {get; set;}
	}
    public partial class SYS_USER
	{
		 public string STATUS {get; set;}
		 public string CREATED_BY {get; set;}

 
         public DateTime? CREATED_DATE { get; set; }
      
		 public string LAST_UPDATED_BY {get; set;}
		 public DateTime? LAST_UPDATED_DATE {get; set;}
		 public string USER_ACCOUNT {get; set;}
		 public string USER_NAME {get; set;}
		 public string USER_PASSWORD {get; set;}
		 public string EMAIL_ADDR {get; set;}
		 public string PHONE_NO {get; set;}
		 public string ADDRESS {get; set;}
		 public string FE_MALE {get; set;}
 
         public decimal? FAIL_TIMES { get; set; }
    
		 public string IS_LOCKED {get; set;}
        public string IS_LOCKED2 { get; set; }
    }
	public partial class SYS_USER_AUTHORIZATION
	{
		 public string STATUS {get; set;}
		 public string CREATED_BY {get; set;}
		 public DateTime? CREATED_DATE {get; set;}
		 public string LAST_UPDATED_BY {get; set;}
		 public DateTime? LAST_UPDATED_DATE {get; set;}
		 public string USER_ACCOUNT {get; set;}
		 public string RESOURCE_ID {get; set;}
		 public string IS_FORBIDDEN {get; set;}
	}
	public partial class SYS_USER_ROLE
	{
		 public string STATUS {get; set;}
		 public string CREATED_BY {get; set;}
		 public DateTime? CREATED_DATE {get; set;}
		 public string LAST_UPDATED_BY {get; set;}
		 public DateTime? LAST_UPDATED_DATE {get; set;}
		 public string USER_ACCOUNT {get; set;}
		 public string ROLE_CODE {get; set;}
	}
    public partial class SYS_FILES
    {
        public string ID { get; set; }
        public DateTime? DATETIME_CREATED { get; set; }
        public string USER_CREATED { get; set; }
        public string USER_MODIFIED { get; set; }
        public DateTime? DATETIME_MODIFIED { get; set; }
        public string CLIENT_TYPE { get; set; }
        public byte[] FILE_BODY { get; set; }
        public string FILE_NAME { get; set; }
        public string FILE_PATH { get; set; }
        public string FILE_VERSION { get; set; }
        public string MD5 { get; set; }
        public int? REVISION { get; set; }
        public string STATE { get; set; }
        public DateTime? DATETIME_FILE_MODIFIED { get; set; }
        public DateTime? DATETIME_FILE_CREATED { get; set; }
    }
}
