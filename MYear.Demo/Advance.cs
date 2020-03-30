using MYear.ODA;
using MYear.ODA.Cmd;
using MYear.ODA.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MYear.Demo
{
    public class Advance
    {

        [Demo(Demo = FuncType.Advance, MethodName = "Transaction", MethodDescript = "事务")]
        public static object Transaction()
        {
            ODAContext ctx = new ODAContext();
            var U1 = ctx.GetCmd<CmdSysUser>();
            ctx.BeginTransaction();
            try
            {
                var U = ctx.GetCmd<CmdSysUser>();
                U.Where(U.ColUserAccount == "User1", U.ColIsLocked == "N", U.ColStatus == "O", U.ColEmailAddr.IsNotNull)
                 .Update(
                    U.ColUserName == "新的名字", U.ColIsLocked == "Y"
                    ); 
                U1.Insert(U.ColStatus == "O", U1.ColCreatedBy == "User1", U1.ColLastUpdatedBy == "User1", U1.ColLastUpdatedDate == DateTime.Now, U1.ColCreatedDate == DateTime.Now,
                    U1.ColUserAccount == "Nyear", U1.ColUserName == "多年", U1.ColUserPassword == "123", U1.ColFeMale == "M", U1.ColFailTimes == 0, U1.ColIsLocked == "N");
                 
                ctx.Commit();
            }
            catch
            {
                ctx.RollBack();
            }
            return null;

        }

        [Demo(Demo = FuncType.Advance, MethodName = "ColumnJoin", MethodDescript = "字段连接")]
        public static object ColumnJoin()
        {
            ////字符串的连接，不同数据库的处理差异太大，ODA没有提供字符串连接的方法.
            ////但可以用户DataTable方法或通能过实体属性实现
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("COL_ID", typeof(string)));
            dt.Columns.Add(new DataColumn("COL_NUM", typeof(int)));
            dt.Columns.Add(new DataColumn("COL_TEST", typeof(string)));
            dt.Columns.Add(new DataColumn("COL_NUM2", typeof(int)));

            for (int i = 0; i < 100; i++) 
            {
                if(i%3==1)
                dt.Rows.Add(Guid.NewGuid().ToString("N").ToUpper(), i + 1, string.Format("this is {0} Rows", i + 1), null);
                else
                    dt.Rows.Add(Guid.NewGuid().ToString("N").ToUpper(), i + 1, string.Format("this is {0} Rows", i + 1), 1000);
            } 

            dt.Columns.Add("CONNECT_COL", typeof(string), "COL_ID+'  +  '+COL_TEST");
            dt.Columns.Add("ADD_COL", typeof(decimal), "COL_NUM+COL_NUM2");
            return dt;
        }

        [Demo(Demo = FuncType.Advance, MethodName = "ConvertModel", MethodDescript = "转List")]
        public static object ConvertModel()
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ADDRESS"));
            data.Columns.Add(new DataColumn("CREATED_BY"));
            data.Columns.Add(new DataColumn("CREATED_DATE", typeof(DateTime)));
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
                    "自由国度",
                    "User1" ,
                    DateTime.Now,
                    "riwfnsse@163.com",
                    "User1" ,
                    DateTime.Now,
                    0,
                    "O",
                     "Dummy",
                    "ImportUser" + i.ToString(),
                    "导入的用户" + i.ToString(),
                    "123",
                    "N"
                };
                data.Rows.Add(dr);
            }
            List<SYS_USER> DataList = ODA.DBAccess.ConvertToList<SYS_USER>(data); 
            return DataList;
        }

        /// <summary>
        /// 自定义SQL
        /// </summary>
        /// <returns></returns>
        [Demo(Demo = FuncType.Advance, MethodName = "UserSQL", MethodDescript = "自定义SQL")]
        public static object UserSQL()
        {
            ///如果SQL语可以重复使用，或者为有程序更规范，推荐派生 ODACmd 类 重写SQL生成方法
            ODAContext ctx = new ODAContext();
            var sql = ctx.GetCmd<SQLCmd>();
            var data = sql.Select("SELECT * FROM SYS_USER WHERE USER_ACCOUNT = @T1", ODAParameter.CreateParam("@T1","User1"));

            var data1 = sql.Select<DataColumn>(0, 100, "SELECT * FROM SYS_USER WHERE USER_ACCOUNT = @T1","", ODAParameter.CreateParam("@T1", "User1"));
            return data;
        }

        /// <summary>
        /// 自定义SQL
        /// </summary>
        /// <returns></returns>
        [Demo(Demo = FuncType.Advance, MethodName = "Procedure", MethodDescript = "自定义存储过程")]
        public static object Procedure()
        {
            ///如果SQL语可以重复使用，或者为有程序更规范，推荐派生 ODACmd 类 重写SQL生成方法
            ODAContext ctx = new ODAContext();
            var sql = ctx.GetCmd<SQLCmd>();
            var data = sql.Procedure("");
            return data;
        }

       
        [Demo(Demo = FuncType.Advance, MethodName = "RecommendProcedure", MethodDescript = "自定义存储过程")]
        public static object RecommendProcedure()
        {
            /// 推荐派生 ODACmd 类 重写SQL生成方法
            ODAContext ctx = new ODAContext();
            var sql = ctx.GetCmd<SQLCmd>();
            var data = sql.Procedure("");
            return data;
        }

        [Demo(Demo = FuncType.Advance, MethodName = "SQLDebug", MethodDescript = "SQLDebug")]
        public static object SQLDebug()
        {
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            var data = U.Where(U.ColUserAccount == "User1", U.ColIsLocked == "N", U.ColEmailAddr.IsNotNull)
            .SelectDynamicFirst(U.ColUserAccount, U.ColUserName, U.ColPhoneNo, U.ColEmailAddr); 

            ////ODAContext.LastODASQL;属性是ODA最近生成的SQL语句块,包含了ODA 控制SQL语句(并非真正发送给数据库的SQL)、参数、语句类型、操作对象等； 
            var ODASQL = ODAContext.LastODASQL;
            ///ctx.LastSQL属性是最近发送给数据库的SQL；由于分页的方法是两条SQL，所以此处的SQL是最后读取数据库的SQL；
            string sql = ctx.LastSQL;
            ///ctx.SQLParams属性是最近发送给数据库的SQL的参数；
            object[] param = ctx.SQLParams;  
            return data;
        }
         
        [Demo(Demo = FuncType.Advance, MethodName = "Hook", MethodDescript = "ODA钩子")] 
        public static object Hook()
        {
            ///开发者可以通过ODA钩子自定义SQL路由,在SQL执行前对SQL进行修改； 
            ODAContext.CurrentExecutingODASql += ODASqlExecutingEvent; 
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            var data = U.Where(U.ColUserAccount == "User1", U.ColIsLocked == "N", U.ColEmailAddr.IsNotNull)
            .Select(U.ColUserAccount, U.ColUserName, U.ColPhoneNo, U.ColEmailAddr); 
            ODAContext.CurrentExecutingODASql -= ODASqlExecutingEvent;
            return data;
        }

        private static void ODASqlExecutingEvent(object source, ExecuteEventArgs args)
        {
            if (args.SqlParams.ScriptType == SQLType.Select
                && args.SqlParams.TableList.Contains("SYS_USER"))
            {
                args.DBA = new ODA.Adapter.DbASQLite("Data Source=./sqlite.db"); ///改变ODA预设的数据库执行实例，重新实例化一个SQL语句执行实例。
                args.SqlParams.ParamList.Clear();
                args.SqlParams.SqlScript.Clear();
                args.SqlParams.SqlScript.AppendLine(" SELECT * FROM SYS_ROLE"); ///修改将要执行的SQL语句
            }
        } 
        [Demo(Demo = FuncType.Advance, MethodName = "Monitor", MethodDescript = "SQL语句监控钩子")]
        public static object Monitor()
        {
            ///开发者可能通过此钩子，可以监控所有发送给数据库SQL语句及其参数。 
            ODAContext.CurrentExecutingSql += SqlExecutingEvent;
            ODAContext ctx = new ODAContext();
            int total = 0;
            var U = ctx.GetCmd<CmdSysUser>();
            var data = U.Where(U.ColUserAccount == "User1", U.ColIsLocked == "N", U.ColEmailAddr.IsNotNull)
            .Select(0, 20, out total, U.ColUserAccount, U.ColUserName, U.ColPhoneNo, U.ColEmailAddr);

            ODAContext.CurrentExecutingSql -= SqlExecutingEvent;
            return data;
        }
        private static void SqlExecutingEvent(string Sql, object[] prms)
        {
            ///记录将要被执行的SQL语句及其参数
            string LogSql = Sql + prms.ToString();  
        }

    } 
}
