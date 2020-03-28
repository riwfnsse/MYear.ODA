using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MYear.ODA.Cmd;
using MYear.ODA.Model;
using MYear.ODA;
using System.Data;
using System;

namespace MYear.Demo
{
    public class UpdateDeleteDemo
    {
        [Demo(Demo = FuncType.UpdateDelete, MethodName = "Update", MethodDescript = "更新数据")]
        public static void Update()
        {
            ///Update 的 where 条件可参考 SELECT 语句 
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            U.Where(U.ColUserAccount == "User1", U.ColIsLocked == "N", U.ColStatus == "O", U.ColEmailAddr.IsNotNull)
             .Update(
                U.ColUserName == "新的名字", U.ColIsLocked == "Y"
                );
        }
        [Demo(Demo = FuncType.UpdateDelete, MethodName = "UpdateModel", MethodDescript = "模型数据Upadte")]
        public static void UpdateModel()
        {
            ///使用实体 Update 数据时，对于属性值为 null 的字段不作更新。
            ///这是由于在ORM组件的实际应用中，多数时候界面回传的是完整的实体对象，
            ///或者收接时使用完整的实体作为反序列化的容器，那些不需要更新的字段也在其中，而且为null.
            ODAContext ctx = new ODAContext(); 
            var U = ctx.GetCmd<CmdSysUser>();
            U.Where(U.ColUserAccount == "User1", U.ColIsLocked == "N", U.ColStatus == "O", U.ColEmailAddr.IsNotNull)
             .Update(new SYS_USER()
            {
                ADDRESS = "自由国度",
                CREATED_BY = "InsertModel",
                CREATED_DATE = DateTime.Now, 
                STATUS = "O",
                USER_ACCOUNT = "NYear1",
                USER_NAME = "多年1",
                USER_PASSWORD = "123",
                IS_LOCKED = "N",
            });

        }

        [Demo(Demo = FuncType.UpdateDelete, MethodName = "UpdateCompute", MethodDescript = "更新运算")]
        public static void UpdateCompute()
        {
            ////支持的运算符号：+ 、 - 、*、/、%
            ///目前对一个字段更新时，只支持一个运算符号；
            ODAContext ctx = new ODAContext(); 
            var U = ctx.GetCmd<CmdSysUser>();
            var data = U.Where(U.ColUserAccount == "User1", U.ColIsLocked == "N", U.ColEmailAddr.IsNotNull)
                .Update(U.ColFailTimes == U.ColFailTimes + 1, U.ColUserName == U.ColUserAccount + U.ColEmailAddr ); 
        }

        [Demo(Demo = FuncType.UpdateDelete, MethodName = "Delete", MethodDescript = "删除数据")]
        public static void Delete()
        {
            ////Delete的where条件可参考 SELECT 语句 
            ODAContext ctx = new ODAContext();
            var U = ctx.GetCmd<CmdSysUser>();
            var data = U.Where(U.ColUserAccount == "User1", U.ColIsLocked == "N", U.ColEmailAddr.IsNotNull)
                .Delete();
        }
    }
}