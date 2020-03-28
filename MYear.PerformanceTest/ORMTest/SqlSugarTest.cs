
using MYear.ODA.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYear.PerformanceTest.ORMTest
{
    public static class SqlSugarTest
    {
        public static void ReadData()
        {
            using (SqlSugarClient conn = DBConfig.GetSugarConn())
            {
                var list2 = conn.Queryable<Test>().ToList();
            }
        } 
        public static void  Paging()
        {
            using (SqlSugarClient conn = DBConfig.GetSugarConn())
            {
                 
                int total = 0;
                var list2 = conn.Queryable<Test>().ToPageList(20, 50, ref total);
            }
        }
        public static void Sql()
        {
            using (SqlSugarClient db = DBConfig.GetSugarConn())
            {
                string[] roles = new string[] { "Administrator", "Admin", "PowerUser", "User", "Guest" };
                var data = db.Queryable<SYS_USER, SYS_USER_ROLE>((u, ur) => new object[] { JoinType.Inner, u.USER_ACCOUNT == ur.USER_ACCOUNT && ur.STATUS == "O" })
                     .Where((u, ur) => u.STATUS == "O" && (u.EMAIL_ADDR != null || u.EMAIL_ADDR == "riwfnsse@163.com") && u.IS_LOCKED == "N"
                     && roles.Contains(ur.ROLE_CODE))
                     .GroupBy((u, ur) => ur.ROLE_CODE)
                     .Having(u => SqlFunc.AggregateCount(u.USER_ACCOUNT) > 2)
                     .OrderBy(u => SqlFunc.AggregateCount(u.USER_ACCOUNT))
                        .Select((u, ur) => new
                        {
                            USER_COUNT = SqlFunc.AggregateCount(u.USER_ACCOUNT),
                            ROLE_CODE = ur.ROLE_CODE
                        }).ToDataTable();

            }
        }
        public static void GetById()
        {
            using (SqlSugarClient conn = DBConfig.GetSugarConn())
            {
                var list2 = conn.Queryable<Test>().InSingle(1);
            }
        }
    }
}
