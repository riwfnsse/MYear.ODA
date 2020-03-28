using MYear.ODA;
using MYear.ODA.Cmd;
using MYear.PerformanceTest.ORMTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYear.PerformanceTest
{
    public static class EFTest
    {
        public static void ReadData()
        {
            using (EFContext conn = new EFContext(DBConfig.ConnectionString))
            {
                var list = conn.TestList.AsNoTracking().ToList();
            }
        } 
        public static void  Paging()
        {
            using (EFContext db = new EFContext(DBConfig.ConnectionString))
            {
                int total = db.TestList.Count();
                var expr = from u in db.TestList
                           orderby 0
                           select u;
                var list = expr.Skip(20 * 50).Take(50).ToList();
            }
        }
        public static void Sql()
        {
            using (EFContext db = new EFContext(DBConfig.ConnectionString))
            { 
                string[] roles = new string[] { "Administrator", "Admin", "PowerUser", "User", "Guest" };

                var data = (from u in db.SysUser
                            join ur in db.SysUserRole.Where(a => a.STATUS == "O") on u.USER_ACCOUNT equals ur.USER_ACCOUNT into urt
                            from ur in urt.DefaultIfEmpty()
                            where u.STATUS == "O"
                             && (u.EMAIL_ADDR == null || u.EMAIL_ADDR == "riwfnsse@163.com")
                             && u.IS_LOCKED == "N"
                             && roles.Contains(ur.ROLE_CODE)
                            group ur by ur.ROLE_CODE into urg
                            select new
                            {
                                urg.Key,
                                USER_COUNT = urg.Count()
                            }).ToList(); 
            } 
        }
        public static void GetById()
        {
            using (EFContext db = new EFContext(DBConfig.ConnectionString))
            {
                 var list = db.TestList.AsNoTracking().Single(it => it.Id == 1);
            }
        }
    }
}
