using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYear.ODA.Model
{

    public partial class SYS_USER
    {
        public string STATUS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string LAST_UPDATED_BY { get; set; }
        public DateTime? LAST_UPDATED_DATE { get; set; }
        [Key]
        public string USER_ACCOUNT { get; set; }
        public string USER_NAME { get; set; }
        public string USER_PASSWORD { get; set; }
        public string EMAIL_ADDR { get; set; }
        public string PHONE_NO { get; set; }
        public string ADDRESS { get; set; }
        public string FE_MALE { get; set; }

        public decimal? FAIL_TIMES { get; set; }
        public string IS_LOCKED { get; set; }
    }

    public partial class SYS_USER_ROLE
    {
        public string STATUS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string LAST_UPDATED_BY { get; set; }
        public DateTime? LAST_UPDATED_DATE { get; set; }
         
        [Key]
        [Column(Order = 2)]
        public string USER_ACCOUNT { get; set; }
        [Key]
        [Column(Order = 1)]
        public string ROLE_CODE { get; set; }
    }



    [Dapper.Contrib.Extensions.Table("Test")]
    public partial class Test
    {
        [Dapper.Contrib.Extensions.Key]
        public int? Id { get; set; }
        public byte? F_Byte { get; set; }
        public Int16? F_Int16 { get; set; }
        public int? F_Int32 { get; set; }
        public long? F_Int64 { get; set; }
        public double? F_Double { get; set; }
        public float? F_Float { get; set; }
        public decimal? F_Decimal { get; set; }
        public bool? F_Bool { get; set; }
        public DateTime? F_DateTime { get; set; }
        public Guid? F_Guid { get; set; }
        public string F_String { get; set; }
       // public byte[] F_Bytes { get; set; }
    }


}
