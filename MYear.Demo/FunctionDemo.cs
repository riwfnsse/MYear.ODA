using MYear.ODA;
using MYear.ODA.Cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MYear.Demo
{
    public class FunctionDemo
    {
        [Demo(Demo = FuncType.Function, MethodName = "Function", MethodDescript = " 数据库函数")]
        public static object Function()
        {
            ///ODA提供数据库常用的通用系统函数：MAX, MIN,  COUNT, SUM, AVG, LENGTH, LTRIM, RTRIM, TRIM, ASCII, UPPER,  LOWER 
            ///这些函数由字段直接带出，如：ColUserAccount.Count
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            object data = U.Where(U.ColStatus == "O", U.ColIsLocked == "N")
                .Groupby(U.ColUserAccount)
                 .Select(U.Function.Count.As("CountAll"), U.ColUserAccount.Count.As("CountOne"), U.ColUserAccount.Upper.As("UPPER_ACC"), U.ColUserAccount.Trim.Ltrim.As("TRIM_ACC"));
            return data;
        }
        [Demo(Demo = FuncType.Function, MethodName = "Express ", MethodDescript = "表达式")]
        public static object Express()
        {
            ////Express方法, 用户可在 SELECT 字段中注入自定义的一段SQL脚本。
            ////因为ODA 的表达式,是应用者注入的一段SQL语句，所以SQL注入的风险及是否可以跨数据库，就用开发者掌握了。
            ODAParameter p1 = new ODAParameter() { ColumnName = "Params1", DBDataType = ODAdbType.OVarchar, Direction = System.Data.ParameterDirection.Input, ParamsName = ODAParameter.ODAParamsMark + "Params1", ParamsValue = "我是第一个参数的值", Size = 2000 };
            ODAParameter p2 = new ODAParameter() { ColumnName = "Params2", DBDataType = ODAdbType.OVarchar, Direction = System.Data.ParameterDirection.Input, ParamsName = ODAParameter.ODAParamsMark + "Params2", ParamsValue = "这是SQL语句注入", Size = 2000 };
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            object data = U.Where(U.ColStatus == "O", U.ColIsLocked == "N")
                 .Select(U.Function.Express("1+1").As("COMPUTED"),
                 U.Function.Express(" null ").As("NULL_COLUMN"), 
                 U.Function.Express(" 'Function( + " + ODAParameter.ODAParamsMark + "Params1, " + ODAParameter.ODAParamsMark + "Params2)' ", p1, p2).As("SQL_Injection"));
            return data;
        }

        [Demo(Demo = FuncType.Function, MethodName = "VisualColumn", MethodDescript = "虚拟字段、临时字段")]
        public static object VisualColumn()
        {
            ///VisualColumn 方法是对 Express 方法的再次封装，为应用提供方便，免出数据转换麻烦、避免SQL注入风险、保证数据库通用。
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            object data = U.Where(U.ColStatus == "O", U.ColIsLocked == "N")
                 .Select(U.Function.VisualColumn("HELLO , I am NYear software").As("STRING_COLUMN"), U.Function.VisualColumn(DateTime.Now).As("APPLICATION_DATETIME"), U.Function.VisualColumn(0).As("DIGIT_COLUMN"));
            return data;
        }
        [Demo(Demo = FuncType.Function, MethodName = "UserDefined ", MethodDescript = "用户自定义的函数")]
        public static object UserDefined()
        {
            ///CreateFunc 方法,用可在 SELECT 字段中加入自定义的数据库函数，但不同的数据库对调用自定义函数的方法差异太大，ODA无法将其统一。
            ///所以若要ODA Function.CreateFunc方法也要以跨数据库，则需要在创建数据库时，特殊处理数据库的schema,user,dbowner,database等对象的名称。 
            ODAContext ctx = new ODAContext();
            var RS = ctx.GetCmd<CmdSysResource>();
            object data = RS.Where(RS.ColStatus == "O",RS.ColResourceType =="MENU") 
                 .Select(RS.AllColumn,RS.Function.CreateFunc("dbo.GET_RESOURCE_PATH", RS.ColId).As("RESOURCE_PATH"));
            return data;
        }

        [Demo(Demo = FuncType.Function, MethodName = "CaseWhen", MethodDescript = "数据转内容转换CaseWhen")]
        public static object CaseWhen()
        {
            ////SQL 语句： case when  条件 then  值 when 条件 then 值 else 默认值 end 
            ODAContext ctx = new ODAContext();  
            var U = ctx.GetCmd<CmdSysUser>();

            Dictionary<ODAColumns, object> Addr = new Dictionary<ODAColumns, object>();
            Addr.Add(U.ColAddress.IsNull, "无用户地址数据...");
            Addr.Add(U.ColAddress.Like("%公安局%"), "被抓了?");

            Dictionary<ODAColumns, object> phone = new Dictionary<ODAColumns, object>();
            phone.Add(U.ColPhoneNo.IsNull, "这个家伙很懒什么都没有留下");
            phone.Add(U.ColPhoneNo == "110", "小贼快跑");
            phone.Add(U.ColAddress.NotLike("%公安局%"), "被抓了?");

            object data = U.Where(U.ColStatus == "O", U.ColIsLocked == "N")
                 .Select(U.Function.CaseWhen(Addr, U.ColAddress).As("ADDRESS"), U.Function.CaseWhen(phone, "110").As("PHONE_NO"));
            return data;
        }
        [Demo(Demo = FuncType.Function, MethodName = "NullDefault", MethodDescript = "空值转换")]
        public static object NullDefault()
        {
            ///NullDefault 是对CaseWhen方法的再次封装，以方便应用
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            object data = U.Where(U.ColStatus == "O", U.ColIsLocked == "N")
                 .Select(U.Function.NullDefault(U.ColAddress, "无用户地址数据...").As("ADDRESS"), U.Function.NullDefault(U.ColPhoneNo,110).As("PHONE_NO"));
            return data;
        }
        [Demo(Demo = FuncType.Function, MethodName = "Case", MethodDescript = "数据转内容转换Case")]
        public static object Case()
        {
            ////SQL 语句： case 字段 when  对比值 then 值 when 对比值 then 值 else 默认值 end 
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();

            Dictionary<object, object> Addr = new Dictionary<object, object>();
            Addr.Add(U.Function.Express(" NULL "), "无用户地址数据...");
            Addr.Add("天堂", "人生最终的去处");

            Dictionary<object, object> phone = new Dictionary<object, object>();
            phone.Add(U.Function.Express(" NULL "), "这个家伙很懒什么都没有留下");
            phone.Add( "110", "小贼快跑");
            phone.Add(U.ColAddress, "资料有误，电话与地址相同");

            object data = U.Where(U.ColStatus == "O", U.ColIsLocked == "N")
                 .Select(U.Function.Case(U.ColAddress,Addr, U.ColAddress).As("ADDRESS"), U.Function.Case(U.ColPhoneNo,phone, U.ColPhoneNo).As("PHONE_NO"));
            return data;
        }
        [Demo(Demo = FuncType.Function, MethodName = "Decode", MethodDescript = "数据转内容转换Decode")]
        public static object Decode()
        {
            ///ODA Decode方法 模拟Oracle内置Decode函数,对Case方法的再次封装，以方便应用
            ODAContext ctx = new ODAContext();
            var RS = ctx.GetCmd<CmdSysResource>();
            object data = RS.Where(RS.ColStatus == "O", RS.ColResourceType == "MENU")
                 .Select(RS.Function.Decode(RS.ColResourceType, "未知类型", "WEB", "网页资源", "WFP_PAGE", "WPF页面资源", "WPF_WIN", "WPF程序窗口", "WIN_FORM", "FORM窗口").As("RESOURCE_TYPE")
                 , RS.AllColumn); 
            return data;
        }


    }
}
