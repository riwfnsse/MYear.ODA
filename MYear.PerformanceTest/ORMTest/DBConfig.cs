using MYear.ODA.Model;
using MYear.ODA;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYear.PerformanceTest.ORMTest
{
    public class DBConfig
    {
        public const string ConnectionString = "server=localhost;database=NYear;uid=sa;pwd=123;";

        static DBConfig()
        {
            Database.SetInitializer<EFContext>(null); 
        } 
        public static SqlSugarClient GetSugarConn()
        {
             
            return new SqlSugarClient(new ConnectionConfig() { IsAutoCloseConnection = true, InitKeyType = InitKeyType.SystemTable, ConnectionString = DBConfig.ConnectionString, DbType = DbType.SqlServer });
        }
        public static EFContext GetEFConn()
        {
            return new EFContext(DBConfig.ConnectionString);
        }
        public static ODAContext GetODAContext()
        {
            return new ODAContext(DbAType.MsSQL, DBConfig.ConnectionString);
        }

    }
    public class EFContext : DbContext
    {
        public EFContext(string connectionString) : base(connectionString)
        {

        }
        public DbSet<Test> TestList { get; set; }
        public DbSet<SYS_USER> SysUser { get; set; }
        public DbSet<SYS_USER_ROLE> SysUserRole { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>().ToTable("Test");
            modelBuilder.Entity<SYS_USER>().ToTable("SYS_USER");
            modelBuilder.Entity<SYS_USER_ROLE>().ToTable("SYS_USER_ROLE");
        }
    }
}
