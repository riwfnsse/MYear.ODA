using MYear.PerformanceTest.ORMTest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYear.PerformanceTest
{
    class Program
    {
        const int readData = 1;
        const int paging = 100;
        const int sql = 100;
        const int byId = 100;
        static void Main(string[] args)
        {
            Test(OrmType.EF, TestType.GetById, 10);
            Test(OrmType.ODA, TestType.GetById, 10);
            Test(OrmType.SqlSugar, TestType.GetById, 10);
            Test(OrmType.Dapper, TestType.GetById, 10);
            Console.ReadKey();
        }
         
        static long ODAPerformanceTest(TestType Ttype)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            switch (Ttype)
            {
                case TestType.ReadData:
                    for (int i = 0; i < readData; i++)
                        ODATest.ReadData();
                    break;
                case TestType.Paging:
                    for (int i = 0; i < paging; i++)
                        ODATest.Paging();
                    break;
                case TestType.Sql:
                    for (int i = 0; i < sql; i++)
                        ODATest.Sql();
                    break;
                case TestType.GetById:
                    for (int i = 0; i < byId; i++)
                        ODATest.GetById();
                    break;
            }
            sw.Stop();
            Console.WriteLine(string.Format(" ODA Performance Test {0} cost : {1} ", Enum.GetName(typeof(TestType), Ttype), sw.ElapsedMilliseconds));
            return sw.ElapsedMilliseconds;

        }

        static long EFPerformanceTest(TestType Ttype)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            switch (Ttype)
            {
                case TestType.ReadData:
                    for (int i = 0; i < readData; i++)
                        EFTest.ReadData();
                    break;
                case TestType.Paging:
                    for (int i = 0; i < paging; i++)
                        EFTest.Paging();
                    break;
                case TestType.Sql:
                    for (int i = 0; i < sql; i++)
                        EFTest.Sql();
                    break;
                case TestType.GetById:
                    for (int i = 0; i < byId; i++)
                        EFTest.GetById();
                    break;
            }
            sw.Stop();
            Console.WriteLine(string.Format(" EF Performance Test {0} cost : {1} ", Enum.GetName(typeof(TestType), Ttype), sw.ElapsedMilliseconds));
            return sw.ElapsedMilliseconds;
        }

        static long SqlSugarPerformanceTest(TestType Ttype)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            switch (Ttype)
            {
                case TestType.ReadData:
                    for (int i = 0; i < readData; i++)
                        SqlSugarTest.ReadData();
                    break;
                case TestType.Paging:
                    for (int i = 0; i < paging; i++)
                        SqlSugarTest.Paging();
                    break;
                case TestType.Sql:
                    for (int i = 0; i < sql; i++)
                        SqlSugarTest.Sql();
                    break;
                case TestType.GetById:
                    for (int i = 0; i < byId; i++)
                        SqlSugarTest.GetById();
                    break;
            }
            sw.Stop();
            Console.WriteLine(string.Format(" SqlSugar Performance Test {0} cost : {1} ", Enum.GetName(typeof(TestType), Ttype), sw.ElapsedMilliseconds));
            return sw.ElapsedMilliseconds;
        }

        static long DapperPerformanceTest(TestType Ttype)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            switch (Ttype)
            {
                case TestType.ReadData:
                    for (int i = 0; i < readData; i++)
                        DapperTest.ReadData();
                    break;
                case TestType.Paging:
                    for (int i = 0; i < paging; i++)
                        DapperTest.Paging();
                    break;
                case TestType.Sql:
                    for (int i = 0; i < sql; i++)
                        DapperTest.Sql();
                    break;
                case TestType.GetById:
                    for (int i = 0; i < byId; i++)
                        DapperTest.GetById();
                    break;
            }
            sw.Stop();
            Console.WriteLine(string.Format(" Dapper Performance Test {0} cost : {1} ", Enum.GetName(typeof(TestType), Ttype), sw.ElapsedMilliseconds));
            return sw.ElapsedMilliseconds;
        } 

        /// <summary>
        /// 单线程测试
        /// </summary>
        /// <param name="Orm"></param>
        /// <param name="Ttype"></param>
        /// <param name="Times"></param>
        static void Test(OrmType Orm, TestType Ttype, int Times)
        {
            Console.WriteLine(string.Format("Single Thread Test ORM {0} TestType {1} ", Enum.GetName(typeof(OrmType), Orm), Enum.GetName(typeof(TestType), Ttype))); 
            System.Threading.Thread.Sleep(50);
            GC.Collect();
            System.Threading.Thread.Sleep(50);
            long total = 0;
            switch (Orm)
            {
                case OrmType.Dapper:
                    for (int i = 0; i < Times; i++)
                    {
                        total += DapperPerformanceTest(Ttype);
                    }
                    break;
                case OrmType.EF:
                    for (int i = 0; i < Times; i++)
                    {
                        total += EFPerformanceTest(Ttype); 
                    }
                    break;
                case OrmType.ODA:
                    for (int i = 0; i < Times; i++)
                    {
                        total += ODAPerformanceTest(Ttype);
                    }
                    break;
                case OrmType.SqlSugar:
                    for (int i = 0; i < Times; i++)
                    {
                        total += SqlSugarPerformanceTest(Ttype);

                    }
                    break;
            }
            Console.WriteLine(string.Format("{0} Test {1} Times Total cost : {2} ", Enum.GetName(typeof(TestType), Ttype), Times, total));
        }

        
    }

    public enum OrmType
    {
        SqlSugar,
        Dapper,
        EF,
        ODA,
    }

    public enum TestType
    {
        ReadData,
        Paging,
        Sql,
        GetById
    }
}
