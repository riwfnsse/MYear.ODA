using Dapper.Contrib.Extensions;
using MYear.ODA.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Dapper.Contrib;
using Dapper;


namespace MYear.PerformanceTest.ORMTest
{
    public static class DapperTest
    {
        public static void ReadData()
        {
            using (SqlConnection db = new SqlConnection(DBConfig.ConnectionString))
            {
                var list = db.GetAll<Test>();
            }
        }
        public static void Paging()
        {
            using (SqlConnection db = new SqlConnection(DBConfig.ConnectionString))
            {
                ///没有分页方法
            }
        }
        public static void Sql()
        {
            using (SqlConnection db = new SqlConnection(DBConfig.ConnectionString))
            {
                ///只能生成最简单的SQL
            }
        }
        public static void GetById()
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                var list = conn.Get<Test>(1);
            }
        }
    }
}
