using MYear.ODA;
using MYear.ODA.Cmd;
using MYear.ODA.Model;
using System;
using System.Data;

namespace MYear.Demo
{
    public class InsertDemo
    {
        [Demo(Demo = FuncType.Insert, MethodName = "Insert", MethodDescript = "插入指定字段的数据")]
        public static void Insert()
        {
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();

            U.Insert(U.ColStatus == "O", U.ColCreatedBy == "User1", U.ColLastUpdatedBy == "User1", U.ColLastUpdatedDate == DateTime.Now, U.ColCreatedDate == DateTime.Now,
                U.ColUserAccount == "Nyear", U.ColUserName == "多年", U.ColUserPassword == "123", U.ColFeMale == "M", U.ColFailTimes ==0,U.ColIsLocked =="N");

        }

  

        [Demo(Demo = FuncType.Insert, MethodName = "InsertModel", MethodDescript = "插入模型的数据")]
        public static void InsertModel()
        {
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>(); 
            U.Insert(new SYS_USER()
            {
                ADDRESS = "自由国度",
                CREATED_BY = "InsertModel",
                CREATED_DATE = DateTime.Now, 
                FAIL_TIMES = 0,
                STATUS = "O",
                USER_ACCOUNT = "NYear1",
                USER_NAME = "多年1",
                USER_PASSWORD = "123",
                IS_LOCKED ="N",
            }); 
        }

        [Demo(Demo = FuncType.Insert, MethodName = "Import", MethodDescript = "大批量导入数据")]
        public static string Import()
        {
            DateTime prepare = DateTime.Now;
            DataTable data = new DataTable();

            data.Columns.Add(new DataColumn("ADDRESS"));
            data.Columns.Add(new DataColumn("CREATED_BY"));
            data.Columns.Add(new DataColumn("CREATED_DATE",typeof(DateTime)));
            data.Columns.Add(new DataColumn("EMAIL_ADDR"));
            data.Columns.Add(new DataColumn("LAST_UPDATED_BY"));
            data.Columns.Add(new DataColumn("LAST_UPDATED_DATE", typeof(DateTime))); 
            data.Columns.Add(new DataColumn("FAIL_TIMES", typeof(decimal)));
            data.Columns.Add(new DataColumn("STATUS"));
            data.Columns.Add(new DataColumn("DUMMY"));
            data.Columns.Add(new DataColumn("USER_ACCOUNT"));
            data.Columns.Add(new DataColumn("USER_NAME"));
            data.Columns.Add(new DataColumn("USER_PASSWORD"));
            data.Columns.Add(new DataColumn("IS_LOCKED"));
             
            for (int i = 0; i < 10000; i++)
            {
                object[] dr = new object[]
                {
                    "第二次批量导入",
                    "User2" + i.ToString() ,
                    DateTime.Now,
                    "riwfnsse@163.com",
                    "User1" ,
                    DateTime.Now,
                    0,
                    "O",
                     "Dummy",
                    "User3" + DateTime.Now.GetHashCode().ToString() + i.ToString(),
                    "导入的用户" + i.ToString(),
                    "123",
                    "N"                   
                };
                data.Rows.Add(dr); 
            }
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();

            DateTime Begin = DateTime.Now;
            U.Import(data);
            DateTime end1 = DateTime.Now;
            return "导入完成,prepare " + prepare.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " begin " + Begin.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " end " + end1.ToString("yyyy-MM-dd HH:mm:ss.fffff");
        }
    }
}
