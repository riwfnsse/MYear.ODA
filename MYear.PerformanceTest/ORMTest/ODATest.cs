using MYear.ODA;
using MYear.ODA.Cmd;

namespace MYear.PerformanceTest.ORMTest
{
    public static class ODATest
    {

        public static void ReadData()
        {
            ODAContext ctx = DBConfig.GetODAContext();
            var t = ctx.GetCmd<CmdTest>();
            var list = t.SelectM();
        }
        public static void Paging()
        {
            ODAContext ctx = DBConfig.GetODAContext();
            int total = 0;
            var t = ctx.GetCmd<CmdTest>();
            var list = t.SelectM(20 * 50, 50, out total);
        }
        public static void Sql()
        {
            ODAContext ctx = DBConfig.GetODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            var UR = ctx.GetCmd<CmdSysUserRole>();

            var data = U.InnerJoin(UR, U.ColUserAccount == UR.ColUserAccount, UR.ColStatus == "O")
              .Where(U.ColStatus == "O", U.ColEmailAddr.IsNotNull.Or(U.ColEmailAddr == "riwfnsse@163.com"),
              U.ColIsLocked == "N" ,
               UR.ColRoleCode.In("Administrator", "Admin", "PowerUser", "User", "Guest"))
              .Groupby(UR.ColRoleCode)
              .Having(U.ColUserAccount.Count > 2)
              .OrderbyAsc(U.ColUserAccount.Count)
              .Select(U.ColUserAccount.Count.As("USER_COUNT"), UR.ColRoleCode); 
        }
        public static void GetById()
        {
            ODAContext ctx = DBConfig.GetODAContext();
            var t = ctx.GetCmd<CmdTest>();
            var list = t.Where(t.ColId == 1).SelectM();
        }
    }

}
