using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MYear.ODA.Cmd;
using MYear.ODA.Model;
using MYear.ODA;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Globalization;
using MySql.Data.Types;

namespace MYear.Demo
{
    public class SelectDemo
    {
        [Demo(Demo = FuncType.Select, MethodName = "Select", MethodDescript = "简单查询")]
        public static object Select()
        { 
            ///ODA语法就SQL语法的直接映射,并尽量简法
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            object data = U.Where(U.ColUserAccount == "User1")
                  .And(U.ColIsLocked == "N")
                  .And(U.ColStatus == "O")
                  .And(U.ColEmailAddr.IsNotNull)  
                 .Select(U.ColUserAccount, U.ColUserPassword.As("PWD"), U.ColUserName, U.ColPhoneNo, U.ColEmailAddr); 
            return data;
        }
        [Demo(Demo = FuncType.Select, MethodName = "SelectM", MethodDescript = "查询默认实体")]
        public static object SelectM()
        {
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            List<SYS_USER> data = U.Where(U.ColUserAccount == "User1", U.ColIsLocked == "N", U.ColStatus == "O", U.ColEmailAddr.IsNotNull)
                .SelectM(U.ColUserAccount, U.ColUserName, U.ColPhoneNo, U.ColEmailAddr);
            return data;
        }
        [Demo(Demo = FuncType.Select, MethodName = "Select<>", MethodDescript = "查询并返回实体类型")]
        public static object SelectDefine()
        {
            ////返回的实体类型可以是任意自定义类型，并不一定是对应数据库的实体
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            List<SYS_USER> data = U.Where(U.ColUserAccount == "User1")
                  .And(U.ColIsLocked == "N")
                  .And(U.ColStatus == "O")
                  .And(U.ColEmailAddr.IsNotNull)
                 .Select<SYS_USER>(U.ColUserAccount, U.ColUserName, U.ColPhoneNo, U.ColEmailAddr);
            return data;
        }

        [Demo(Demo = FuncType.Select, MethodName = "SelectPaging", MethodDescript = "查询分页")]
        public static object SelectPaging()
        {
            ODAContext ctx = new ODAContext(); 
            int total = 0; 
            var U = ctx.GetCmd<CmdSysUser>();
            var data = U.Where(U.ColUserAccount == "User1", U.ColIsLocked == "N", U.ColEmailAddr.IsNotNull)
            .SelectM(0,20,out total, U.ColUserAccount, U.ColUserName, U.ColPhoneNo, U.ColEmailAddr); 
            return data;
        }
        [Demo(Demo = FuncType.Select, MethodName = "SelectFirst", MethodDescript = "查询第一行")]
        public static object SelectFirst()
        {
            ODAContext ctx = new ODAContext(); 
            var U = ctx.GetCmd<CmdSysUser>();
            var data = U.Where(U.ColUserAccount == "User1", U.ColIsLocked == "N", U.ColEmailAddr.IsNotNull)
            .SelectDynamicFirst(U.ColUserAccount, U.ColUserName, U.ColPhoneNo, U.ColEmailAddr);

            string UserName = data.USER_NAME; ///属性 USER_NAME 与 ColUserName 的ColumnName一致，如果没有数据则返回null
            return data; 
        }

         
        [Demo(Demo = FuncType.Select, MethodName = "SelectDynamic", MethodDescript = "返回动态数据模型")]
        public static object SelectDynamic()
        {
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            var data = U.Where(U.ColUserAccount == "User1", U.ColIsLocked == "N", U.ColEmailAddr.IsNotNull)
            .SelectDynamic(U.ColUserAccount, U.ColUserName, U.ColPhoneNo, U.ColEmailAddr);

            string UserName = "";
            if (data.Count > 0)
                UserName =  data[0].USER_NAME; ///与 ColUserName  的 ColumnName一致.
            return data;
        }

        [Demo(Demo = FuncType.Select, MethodName = "Distinct", MethodDescript = "去重复")]
        public static object Distinct()
        {
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            var data = U.Where( U.ColIsLocked == "N", U.ColEmailAddr.IsNotNull)
            .Distinct.Select(U.ColUserAccount, U.ColUserName, U.ColPhoneNo, U.ColEmailAddr);
            return data;
        }

        [Demo(Demo = FuncType.Select, MethodName = "Join", MethodDescript = "连接查询")]
        public static object Join()
        {
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            var R = ctx.GetCmd<CmdSysRole>();
            var UR = ctx.GetCmd<CmdSysUserRole>();

            ///LeftJoin\InnerJoin\RightJoin 可以无限连接，但实际上每种数据库都有对SQL语句长度作限制。
            var data = U.InnerJoin(UR, U.ColUserAccount == UR.ColUserAccount, UR.ColStatus == "O")
                .LeftJoin(R, UR.ColRoleCode == R.ColRoleCode, R.ColStatus == "O")
                .Where(U.ColStatus == "O",R.ColRoleCode == "Administrator")
                 .Select<UserDefineModel>(U.ColUserAccount.As("UserAccount"), U.ColUserName.As("UserName"),R.ColRoleCode.As("Role"), R.ColRoleName.As("RoleName"));
            return data;
        }
        [Demo(Demo = FuncType.Select, MethodName = "List", MethodDescript = "简单内连接")]
        public static object List()
        {
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            var R = ctx.GetCmd<CmdSysRole>();
            var UR = ctx.GetCmd<CmdSysUserRole>();
            var data =  U.ListCmd(UR,R)
                .Where(U.ColUserAccount == UR.ColUserAccount, 
                 UR.ColStatus == "O",
                 UR.ColRoleCode == R.ColRoleCode,
                 R.ColStatus == "O",
                 U.ColStatus == "O",
                 R.ColRoleCode == "Administrator")
                 .Select< UserDefineModel>(U.ColUserAccount.As("UserAccount"), U.ColUserName.As("UserName"),U.ColEmailAddr.As("Email"), R.ColRoleCode.As("Role"), R.ColRoleName.As("RoleName"));

            return data;
        }

        [Demo(Demo = FuncType.Select, MethodName = "SubQuery", MethodDescript = "嵌套子查询")]
        public static object SubQuery()
        {
            ////嵌套子查询需要把一个查询子句转换成视图(ToView方法)，转换成视图之后可以把它视作普通的Cmd使用。
            ///视图里ViewColumns是视图字段的集合。
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            var R = ctx.GetCmd<CmdSysRole>();
            var UR = ctx.GetCmd<CmdSysUserRole>();

            var UA = ctx.GetCmd<CmdSysUserAuthorization>();
            var RA = ctx.GetCmd<CmdSysRoleAuthorization>();

            var Admin = U.InnerJoin(UR, U.ColUserAccount == UR.ColUserAccount, UR.ColStatus == "O")
                .InnerJoin(R, UR.ColRoleCode == R.ColRoleCode, R.ColStatus == "O")
                .Where(U.ColStatus == "O")
                .ToView(U.ColUserAccount.As("SYS_USER"), U.ColUserName, R.ColRoleCode.As("SYS_ROLE"), R.ColRoleName); ////子查询

           var data =  Admin.InnerJoin(UA, UA.ColUserAccount == Admin.ViewColumns[1],UA.ColIsForbidden == "N")
                .InnerJoin(RA,RA.ColRoleCode == Admin.ViewColumns[2],RA.ColIsForbidden =="N") 
                .Where(Admin.ViewColumns[1] == "张三",
                Admin.ViewColumns[2] == "Administrator")
                .Select(); 
            return data;
        }
        [Demo(Demo = FuncType.Select, MethodName = "Union", MethodDescript = "Union")]
        public static object Union()
        { 
            ///被Union或UnionAll的是视图。要求视图与查询的字段的数据库类型及顺序及数据一致（数据库本身的要求，非ODA要求)。
            ODAContext ctx = new ODAContext(); 
            var U = ctx.GetCmd<CmdSysUser>(); 
            var UR = ctx.GetCmd<CmdSysUserRole>();  
            var RA = ctx.GetCmd<CmdSysRoleAuthorization>();
            var RS = ctx.GetCmd<CmdSysResource>();

            var U1 = ctx.GetCmd<CmdSysUser>();
            var UA = ctx.GetCmd<CmdSysUserAuthorization>();
            var RS1 = ctx.GetCmd<CmdSysResource>();

            U.InnerJoin(UR, U.ColUserAccount == UR.ColUserAccount, UR.ColStatus == "O")
                .InnerJoin(RA, RA.ColRoleCode == UR.ColRoleCode, RA.ColStatus == "O")
                .InnerJoin(RS, RS.ColId == RA.ColResourceId, RS.ColStatus == "O")
                .Where(U.ColUserAccount == "User1");

            U1.InnerJoin(UA, U1.ColUserAccount == UA.ColUserAccount, UA.ColStatus == "O")
                .InnerJoin(RS1, RS1.ColId == UA.ColResourceId , RS1.ColStatus=="O")
                .Where(U1.ColUserAccount == "User1");
 
            var data = U.Union(U1.ToView(U1.ColUserAccount, U1.ColUserName, UA.ColIsForbidden,
                RS1.ColId, RS1.ColResourceType, RS1.ColResourceScope, RS1.ColResourceLocation
                ))
                .Select(U.ColUserAccount, U.ColUserName, RA.ColIsForbidden,
                RS.ColId, RS.ColResourceType, RS.ColResourceScope, RS.ColResourceLocation
                ); 
            return data;
        }
        [Demo(Demo = FuncType.Select, MethodName = "OrderBy", MethodDescript = "查询排序")]
        public static object OrderBy()
        {
            ///OrderbyAsc 或OrderbyDesc 对数据按顺序或倒序排列，先给出的排序条件优先排。
            ///OrderbyAsc 或OrderbyDesc 参数可以是多个字段。
            ODAContext ctx = new ODAContext();
            var RS = ctx.GetCmd<CmdSysResource>();
            var datra = RS.Where(RS.ColResourceType == "WEB", RS.ColStatus == "O")
                 .OrderbyDesc(RS.ColResourceIndex)
                 .SelectM();
            return datra;
        }
        [Demo(Demo = FuncType.Select, MethodName = "Where", MethodDescript = "数据库查询条件语法")]
        public static object Where()
        {
            ///Join、Where、Having 查询参数：条件之间可用运算符 “|”(Or方法）或“&”(And方法)表明条件与条件之间的关系；
            ///如同时列出多个条件，则说明SQL语句要同时满足所有条件（即“And”关系）；
            ///Where和Having方法可以多次调用，每调一次SQL语句累加一个条件（And、Or、Groupby、OrderbyAsc、OrderbyDesc方法类同)；
            ///与 Where 方法同等级的 And 方法是等效的
            ///也就是说，数据筛选条件可以根据业务情况动态增加；
            /// IS NULL/ IS NOT NULL 条件可由字段直接带出，如：ColEmailAddr.IsNotNull 
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            var UR = ctx.GetCmd<CmdSysUserRole>();

            var data = U.InnerJoin(UR, U.ColUserAccount == UR.ColUserAccount, UR.ColStatus == "O")
              .Where(U.ColStatus == "O", U.ColEmailAddr.IsNotNull.Or(U.ColEmailAddr == "riwfnsse@163.com"), U.ColIsLocked == "N")
              .Where(UR.ColRoleCode.In("Administrator", "Admin", "PowerUser", "User", "Guest")) 
              .Groupby(UR.ColRoleCode)
              .Having(U.ColUserAccount.Count > 2)
              .OrderbyAsc(U.ColUserAccount.Count)
              .Select(U.ColUserAccount.Count.As("USER_COUNT"), UR.ColRoleCode);

            //以下写法是等效的
            U.InnerJoin(UR, U.ColUserAccount == UR.ColUserAccount, UR.ColStatus == "O");
            U.Where(U.ColStatus == "O");
            U.Where(U.ColEmailAddr.IsNotNull.Or(U.ColEmailAddr == "riwfnsse@163.com"));
            U.And(U.ColIsLocked == "N");
            U.Where(UR.ColRoleCode.In("Administrator", "Admin", "PowerUser", "User", "Guest"));
            U.Groupby(UR.ColRoleCode);
            U.Having(U.ColUserAccount.Count > 2);
            U.OrderbyAsc(U.ColUserAccount.Count);
            data = U.Select(U.ColUserAccount.Count.As("USER_COUNT"), UR.ColRoleCode);
            return data;

        }
        [Demo(Demo = FuncType.Select, MethodName = "GroupByHaving", MethodDescript = "分组统计")]
        public static object GroupByHaving()
        {
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            var UR = ctx.GetCmd<CmdSysUserRole>();
            var data = U.InnerJoin(UR, U.ColUserAccount == UR.ColUserAccount, UR.ColStatus == "O")
               .Where(U.ColStatus == "O", UR.ColRoleCode.In("Administrator", "Admin", "PowerUser", "User", "Guest"))
               .Groupby(UR.ColRoleCode)
               .Having(U.ColUserAccount.Count > 2)
               .OrderbyAsc(UR.ColRoleCode,U.ColUserAccount.Count)
               .Select(U.ColUserAccount.Count.As("USER_COUNT"), UR.ColRoleCode);
            return data;
        }

       
        [Demo(Demo = FuncType.Select, MethodName = "IN/NOT IN", MethodDescript = "IN 条件")]
        public static object In()
        {
            ODAContext ctx = new ODAContext(); 
            var RA = ctx.GetCmd<CmdSysRoleAuthorization>();
            var RS = ctx.GetCmd<CmdSysResource>(); 
            ///IN 数组
            RA.Where(RA.ColIsForbidden == "N", RA.ColStatus == "O", RA.ColRoleCode.In("Administrator", "Admin", "PowerUser"));

            ///IN 子查询
            var data = RS.Where(RS.ColStatus == "O", RS.ColId.In(RA, RA.ColResourceId)) 
                .SelectM(); 
            return data;  
        }

        [Demo(Demo = FuncType.Select, MethodName = "Exists/NOT Exists", MethodDescript = "Exists 子查询")]
        public static object Exists()
        {
            ODAContext ctx = new ODAContext();
            var RA = ctx.GetCmd<CmdSysRoleAuthorization>(); 
            //Exists 子查询的条件
            var RS = ctx.GetCmd<CmdSysResource>();
            RA.Where(RA.ColIsForbidden == "N", RA.ColStatus == "O", RA.ColResourceId == RS.ColId); 

            var data = RS.Where(RS.ColStatus == "O", RS.Function.Exists(RA, RA.AllColumn)) 
                .SelectM();
            return data;
        }

        [Demo(Demo = FuncType.Select, MethodName = "Recursion", MethodDescript = "递归查询")]
        public static object Recursion()
        {
            ///如Oracle的StartWith ConnectBy语句一致。ODA处理：先以 where 条作查出需要递归筛先的数据，然后在内存中递归筛选
            ///由于是在内存递归，所以递归使所用到的所有字段必须包含在 Seclect 字段里。
            ///注：ODA性能比 oracle 数据库的 StartWith ConnectBy 差一个等级，但比 SQLServer 的 with as 好一个级等。
            ///递归有深度限制，数据量多的时候性能下降很快，最好保被递归筛选的数在10W条以内

            ODAContext ctx = new ODAContext();

            ////由根向叶子递归 Prior 参数就是递归方向
            CmdSysResource RS = ctx.GetCmd<CmdSysResource>();  
            var rlt = RS.Where(RS.ColStatus == "O", RS.ColResourceType == "MENU")
                .StartWithConnectBy(RS.ColResourceName.ColumnName + "='根菜单'", RS.ColParentId.ColumnName, RS.ColId.ColumnName, "MENU_PATH", "->", 10)
                .Select(RS.ColResourceName.As("MENU_PATH"), RS.ColId, RS.ColParentId, RS.ColResourceName, RS.ColResourceType, RS.ColResourceScope, RS.ColResourceLocation, RS.ColResourceIndex);
           
            ////由叶子向根递归,Prior 参数就是递归方向
            CmdSysResource RS1 = ctx.GetCmd<CmdSysResource>(); 
            var rlt1 = RS.Where(RS.ColStatus == "O", RS.ColResourceType == "MENU")
                .StartWithConnectBy(RS.ColResourceName.ColumnName + "='菜单1'", RS.ColId.ColumnName, RS.ColParentId.ColumnName, "MENU_PATH", "<-", 10)
                .Select(RS.ColResourceName.As("MENU_PATH"), RS.ColId, RS.ColParentId, RS.ColResourceName, RS.ColResourceType, RS.ColResourceScope, RS.ColResourceLocation, RS.ColResourceIndex);
            rlt1.Merge(rlt);
            return rlt1;
        }
 
        [Demo(Demo = FuncType.Select, MethodName = "Lambda", MethodDescript = "Lambda语法支持")]
        public static object Lambda()
        {
            ///Lambda语法支持最多九个表连接查询
            int total = 0;
            var data = new ODAContext().GetJoinCmd<CmdSysUser>()
                .InnerJoin<CmdSysUserRole>((u, ur) => u.ColUserAccount == ur.ColUserAccount & ur.ColStatus == "O")
                 .InnerJoin<CmdSysRole>((u, ur, r) => ur.ColRoleCode == r.ColRoleCode & r.ColStatus == "O")
                 .InnerJoin<CmdSysRoleAuthorization>((u, ur, r, ra) => r.ColRoleCode == ra.ColRoleCode & ra.ColIsForbidden == "O" & ra.ColStatus == "O")
                 .Where((u, ur, r, ra) => u.ColStatus == "O" & (r.ColRoleCode == "Administrator" | r.ColRoleCode == "Admin") & u.ColIsLocked == "N")
                 .Groupby((u, ur, r, ra) => new IODAColumns[] { r.ColRoleCode, u.ColUserAccount })
                 .Having((u, ur, r, ra) => ra.ColResourceId.Count > 10)
                 .OrderbyAsc((u, ur, r, ra) => new IODAColumns[] { ra.ColResourceId.Count })
                 .Select(0, 20, out total, (u, ur, r, ra) => new IODAColumns[] { r.ColRoleCode, u.ColUserAccount, ra.ColResourceId.Count.As("ResourceCount") });
            return data;
        }
    }

    public class UserDefineModel
    {
        public string UserAccount{get;set;}
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string RoleName { get; set; }
        public decimal? Times { get; set; }
        public DateTime? DateNow { get; set; }
    }

    public enum TestEnum
    {
        e1,
        e2,
        e3,
        e4,
    }
}
