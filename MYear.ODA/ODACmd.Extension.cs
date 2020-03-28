using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MYear.ODA
{
    public static class ODAContextExt
    {
        public static JoinCmd<T> GetJoinCmd<T>(this ODAContext Ctx) where T : ODACmd, new()
        {
            JoinCmd<T> cmd = new JoinCmd<T>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = Ctx.GetCmd<T>();
            return cmd;
        }
    }

    public class JoinCmd<C0> where C0 : ODACmd, new()
    {
        #region 指今寄存
        public ODAContext Ctx { get; set; }
        public C0 Cmd0 { get; set; }
        #endregion

        #region ODA应用语法定义  
        public JoinCmd<C0> OrderbyAsc(Func<C0, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyAsc(OrderBy(this.Cmd0));
            return this;
        }
        public JoinCmd<C0> OrderbyDesc(Func<C0, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyDesc(OrderBy(this.Cmd0));
            return this;
        }
        public JoinCmd<C0> Groupby(Func<C0, IODAColumns[]> Groupby)
        {
            this.Cmd0.Groupby(Groupby(this.Cmd0));
            return this;
        }
        public JoinCmd<C0> Having(Func<C0, IODAColumns> Having)
        {
            this.Cmd0.Having(Having(this.Cmd0));
            return this;
        }
        public JoinCmd<C0> Where(Func<C0, IODAColumns> Where)
        {
            this.Cmd0.Where(Where(this.Cmd0));
            return this;
        }
        public JoinCmd<C0> And(Func<C0, IODAColumns> And)
        {
            this.Cmd0.And(And(this.Cmd0));
            return this;
        }
        public JoinCmd<C0> Or(Func<C0, IODAColumns> Or)
        {
            this.Cmd0.Or(Or(this.Cmd0));
            return this;
        }

        #endregion

        #region 执行SQL语句  
        public int Count()
        {
            return this.Cmd0.Count();
        }
        public int Count(Func<C0, IODAColumns> Col)
        {
            return this.Cmd0.Count(Col(this.Cmd0));
        }
        public DataTable Select(Func<C0, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(Cols(this.Cmd0));
        }
        public DataTable Select(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0));
        }
        public List<T> Select<T>(Func<C0, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(Cols(this.Cmd0));
        }
        public List<T> Select<T>(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0));
        }

        public object[] SelectFirst(Func<C0, IODAColumns[]> Cols)
        {
            return this.Cmd0.SelectFirst(Cols(this.Cmd0));
        }
        #endregion

        #region 表连接扩展

        public JoinCmd<C0, C1> ListCmd<C1>()
            where C1 : ODACmd, new()
        {
            JoinCmd<C0, C1> cmd = new JoinCmd<C0, C1>();
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = Ctx.GetCmd<C1>();
            this.Cmd0.ListCmd(cmd.Cmd1);
            return cmd;
        }
        public JoinCmd<C0, C1, C2> ListCmd<C1, C2>()
            where C1 : ODACmd, new()
            where C2 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2> cmd = new JoinCmd<C0, C1, C2>();
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = Ctx.GetCmd<C1>();
            cmd.Cmd2 = Ctx.GetCmd<C2>();
            this.Cmd0.ListCmd(this.Cmd0, cmd.Cmd1, cmd.Cmd2);
            return cmd;
        }
        public JoinCmd<C0, C1, C2, C3> ListCmd<C1, C2, C3>()
            where C1 : ODACmd, new()
            where C2 : ODACmd, new()
            where C3 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3> cmd = new JoinCmd<C0, C1, C2, C3>();
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = Ctx.GetCmd<C1>();
            cmd.Cmd2 = Ctx.GetCmd<C2>();
            cmd.Cmd3 = Ctx.GetCmd<C3>();
            this.Cmd0.ListCmd(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3);
            return cmd;
        }
        public JoinCmd<C0, C1, C2, C3, C4> ListCmd<C1, C2, C3, C4>()
            where C1 : ODACmd, new()
            where C2 : ODACmd, new()
            where C3 : ODACmd, new()
            where C4 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4> cmd = new JoinCmd<C0, C1, C2, C3, C4>();
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = Ctx.GetCmd<C1>();
            cmd.Cmd2 = Ctx.GetCmd<C2>();
            cmd.Cmd3 = Ctx.GetCmd<C3>();
            cmd.Cmd4 = Ctx.GetCmd<C4>();
            this.Cmd0.ListCmd(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4);
            return cmd;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5> ListCmd<C1, C2, C3, C4, C5>()
            where C1 : ODACmd, new()
            where C2 : ODACmd, new()
            where C3 : ODACmd, new()
            where C4 : ODACmd, new()
            where C5 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5>();
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = Ctx.GetCmd<C1>();
            cmd.Cmd2 = Ctx.GetCmd<C2>();
            cmd.Cmd3 = Ctx.GetCmd<C3>();
            cmd.Cmd4 = Ctx.GetCmd<C4>();
            cmd.Cmd5 = Ctx.GetCmd<C5>();
            this.Cmd0.ListCmd(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5);
            return cmd;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6> ListCmd<C1, C2, C3, C4, C5, C6>()
         where C1 : ODACmd, new()
         where C2 : ODACmd, new()
         where C3 : ODACmd, new()
         where C4 : ODACmd, new()
         where C5 : ODACmd, new()
         where C6 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6>();
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = Ctx.GetCmd<C1>();
            cmd.Cmd2 = Ctx.GetCmd<C2>();
            cmd.Cmd3 = Ctx.GetCmd<C3>();
            cmd.Cmd4 = Ctx.GetCmd<C4>();
            cmd.Cmd5 = Ctx.GetCmd<C5>();
            cmd.Cmd6 = Ctx.GetCmd<C6>();
            this.Cmd0.ListCmd(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6);
            return cmd;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> ListCmd<C1, C2, C3, C4, C5, C6, C7>()
          where C1 : ODACmd, new()
          where C2 : ODACmd, new()
          where C3 : ODACmd, new()
          where C4 : ODACmd, new()
          where C5 : ODACmd, new()
          where C6 : ODACmd, new()
          where C7 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7>();
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = Ctx.GetCmd<C1>();
            cmd.Cmd2 = Ctx.GetCmd<C2>();
            cmd.Cmd3 = Ctx.GetCmd<C3>();
            cmd.Cmd4 = Ctx.GetCmd<C4>();
            cmd.Cmd5 = Ctx.GetCmd<C5>();
            cmd.Cmd6 = Ctx.GetCmd<C6>();
            cmd.Cmd7 = Ctx.GetCmd<C7>();
            this.Cmd0.ListCmd(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6, cmd.Cmd7);
            return cmd;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8> ListCmd<C1, C2, C3, C4, C5, C6, C7, C8>()
          where C1 : ODACmd, new()
          where C2 : ODACmd, new()
          where C3 : ODACmd, new()
          where C4 : ODACmd, new()
          where C5 : ODACmd, new()
          where C6 : ODACmd, new()
          where C7 : ODACmd, new()
          where C8 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8>();
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = Ctx.GetCmd<C1>();
            cmd.Cmd2 = Ctx.GetCmd<C2>();
            cmd.Cmd3 = Ctx.GetCmd<C3>();
            cmd.Cmd4 = Ctx.GetCmd<C4>();
            cmd.Cmd5 = Ctx.GetCmd<C5>();
            cmd.Cmd6 = Ctx.GetCmd<C6>();
            cmd.Cmd7 = Ctx.GetCmd<C7>();
            cmd.Cmd8 = Ctx.GetCmd<C8>();
            this.Cmd0.ListCmd(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6, cmd.Cmd7, cmd.Cmd8);
            return cmd;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8, C9> ListCmd<C1, C2, C3, C4, C5, C6, C7, C8, C9>()
            where C1 : ODACmd, new()
            where C2 : ODACmd, new()
            where C3 : ODACmd, new()
            where C4 : ODACmd, new()
            where C5 : ODACmd, new()
            where C6 : ODACmd, new()
            where C7 : ODACmd, new()
            where C8 : ODACmd, new()
            where C9 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8, C9> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8, C9>();
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = Ctx.GetCmd<C1>();
            cmd.Cmd2 = Ctx.GetCmd<C2>();
            cmd.Cmd3 = Ctx.GetCmd<C3>();
            cmd.Cmd4 = Ctx.GetCmd<C4>();
            cmd.Cmd5 = Ctx.GetCmd<C5>();
            cmd.Cmd6 = Ctx.GetCmd<C6>();
            cmd.Cmd7 = Ctx.GetCmd<C7>();
            cmd.Cmd8 = Ctx.GetCmd<C8>();
            cmd.Cmd9 = Ctx.GetCmd<C9>();
            this.Cmd0.ListCmd(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6, cmd.Cmd7, cmd.Cmd8, cmd.Cmd9);
            return cmd;
        }

        public JoinCmd<C0, C1> LeftJoin<C1>(Func<C0, C1, IODAColumns> On)
            where C1 : ODACmd, new()
        {
            JoinCmd<C0, C1> cmd = new JoinCmd<C0, C1>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = Ctx.GetCmd<C1>(); 
            this.Cmd0.LeftJoin(cmd.Cmd1, On(this.Cmd0, cmd.Cmd1));
            return cmd;
        }

        public JoinCmd<C0, C1> RightJoin<C1>(Func<C0, C1, IODAColumns> On)
            where C1 : ODACmd, new()
        {
            JoinCmd<C0, C1> cmd = new JoinCmd<C0, C1>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = Ctx.GetCmd<C1>();
            this.Cmd0.RightJoin(cmd.Cmd1, On(this.Cmd0, cmd.Cmd1));
            return cmd;
        }

        public JoinCmd<C0, C1> InnerJoin<C1>(Func<C0, C1, IODAColumns> On)
            where C1 : ODACmd, new()
        {
            JoinCmd<C0, C1> cmd = new JoinCmd<C0, C1>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = Ctx.GetCmd<C1>();
            this.Cmd0.InnerJoin(cmd.Cmd1, On(this.Cmd0, cmd.Cmd1));
            return cmd;
        }
        #endregion
    }

    public class JoinCmd<C0, C1> : JoinCmd<C0>
        where C0 : ODACmd, new()
        where C1 : ODACmd, new()
    {
        #region 指今寄存
        public C1 Cmd1 { get; set; }
        #endregion

        #region ODA应用语法定义 

        public JoinCmd<C0, C1> OrderbyAsc(Func<C0, C1, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyAsc(OrderBy(this.Cmd0, this.Cmd1));
            return this;
        }
        public JoinCmd<C0, C1> OrderbyDesc(Func<C0, C1, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyDesc(OrderBy(this.Cmd0, this.Cmd1));
            return this;
        }
        public JoinCmd<C0, C1> Groupby(Func<C0, C1, IODAColumns[]> Groupby)
        {
            this.Cmd0.Groupby(Groupby(this.Cmd0, this.Cmd1));
            return this;
        }
        public JoinCmd<C0, C1> Having(Func<C0, C1, IODAColumns> Having)
        {
            this.Cmd0.Having(Having(this.Cmd0, this.Cmd1));
            return this;
        }
        public JoinCmd<C0, C1> Where(Func<C0, C1, IODAColumns> Where)
        {
            this.Cmd0.Where(Where(this.Cmd0, this.Cmd1));
            return this;
        }
        public JoinCmd<C0, C1> And(Func<C0, C1, IODAColumns> And)
        {
            this.Cmd0.And(And(this.Cmd0, this.Cmd1));
            return this;
        }
        public JoinCmd<C0, C1> Or(Func<C0, C1, IODAColumns> Or)
        {
            this.Cmd0.Or(Or(this.Cmd0, this.Cmd1));
            return this;
        }

        #endregion

        #region 执行SQL语句  
        public int Count(Func<C0, C1, IODAColumns> Col)
        {
            return this.Cmd0.Count(Col(this.Cmd0, this.Cmd1));
        }

        public DataTable Select(Func<C0, C1, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(Cols(this.Cmd0, this.Cmd1));
        }
        public DataTable Select(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1));
        }

        public List<T> Select<T>(Func<C0, C1, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(Cols(this.Cmd0, this.Cmd1));
        }
        public List<T> Select<T>(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1));
        }

        public object[] SelectFirst(Func<C0, C1, IODAColumns[]> Cols)
        {
            return this.Cmd0.SelectFirst(Cols(this.Cmd0, this.Cmd1));
        }
        #endregion

        #region 表连接扩展  
        public JoinCmd<C0, C1, C2> LeftJoin<C2>(Func<C0, C1, C2, IODAColumns> On)
            where C2 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2> cmd = new JoinCmd<C0, C1, C2>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = Ctx.GetCmd<C2>();
            this.Cmd0.InnerJoin(cmd.Cmd2, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2));
            return cmd;
        }

        public JoinCmd<C0, C1, C2> RightJoin<C2>(Func<C0, C1, C2, IODAColumns> On)
            where C2 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2> cmd = new JoinCmd<C0, C1, C2>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = Ctx.GetCmd<C2>();
            this.Cmd0.RightJoin(cmd.Cmd2, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2));
            return cmd;
        }

        public JoinCmd<C0, C1, C2> InnerJoin<C2>(Func<C0, C1, C2, IODAColumns> On)
            where C2 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2> cmd = new JoinCmd<C0, C1, C2>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = Ctx.GetCmd<C2>();
            this.Cmd0.InnerJoin(cmd.Cmd2, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2));
            return cmd;
        }
        #endregion

    }

    public class JoinCmd<C0, C1, C2> : JoinCmd<C0, C1>
        where C0 : ODACmd, new()
        where C1 : ODACmd, new()
        where C2 : ODACmd, new()
    {
        #region 指今寄存
        public C2 Cmd2 { get; set; }
        #endregion

        #region ODA应用语法定义 

        public JoinCmd<C0, C1, C2> OrderbyAsc(Func<C0, C1, C2, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyAsc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2));
            return this;
        }
        public JoinCmd<C0, C1, C2> OrderbyDesc(Func<C0, C1, C2, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyDesc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2));
            return this;
        }
        public JoinCmd<C0, C1, C2> Groupby(Func<C0, C1, C2, IODAColumns[]> Groupby)
        {
            this.Cmd0.Groupby(Groupby(this.Cmd0, this.Cmd1, this.Cmd2));
            return this;
        }
        public JoinCmd<C0, C1, C2> Having(Func<C0, C1, C2, IODAColumns> Having)
        {
            this.Cmd0.Having(Having(this.Cmd0, this.Cmd1, this.Cmd2));
            return this;
        }
        public JoinCmd<C0, C1, C2> Where(Func<C0, C1, C2, IODAColumns> Where)
        {
            this.Cmd0.Where(Where(this.Cmd0, this.Cmd1, this.Cmd2));
            return this;
        }
        public JoinCmd<C0, C1, C2> And(Func<C0, C1, C2, IODAColumns> And)
        {
            this.Cmd0.And(And(this.Cmd0, this.Cmd1, this.Cmd2));
            return this;
        }
        public JoinCmd<C0, C1, C2> Or(Func<C0, C1, C2, IODAColumns> Or)
        {
            this.Cmd0.Or(Or(this.Cmd0, this.Cmd1, this.Cmd2));
            return this;
        }

        #endregion

        #region 执行SQL语句  
        public int Count(Func<C0, C1, C2, IODAColumns> Col)
        {
            return this.Cmd0.Count(Col(this.Cmd0, this.Cmd1, this.Cmd2));
        }
        public DataTable Select(Func<C0, C1, C2, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(Cols(this.Cmd0, this.Cmd1, this.Cmd2));
        }
        public DataTable Select(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2));
        }
        public List<T> Select<T>(Func<C0, C1, C2, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(Cols(this.Cmd0, this.Cmd1, this.Cmd2));
        }
        public List<T> Select<T>(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2));
        }

        public object[] SelectFirst(Func<C0, C1, C2, IODAColumns[]> Cols)
        {
            return this.Cmd0.SelectFirst(Cols(this.Cmd0, this.Cmd1, this.Cmd2));
        }
        #endregion

        #region 表连接扩展 
        public JoinCmd<C0, C1, C2, C3> LeftJoin<C3>(Func<C0, C1, C2, C3, IODAColumns> On)
            where C3 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3> cmd = new JoinCmd<C0, C1, C2, C3>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = Ctx.GetCmd<C3>();
            this.Cmd0.LeftJoin(cmd.Cmd3, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3> RightJoin<C3>(Func<C0, C1, C2, C3, IODAColumns> On)
            where C3 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3> cmd = new JoinCmd<C0, C1, C2, C3>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = Ctx.GetCmd<C3>();
            this.Cmd0.RightJoin(cmd.Cmd3, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3> InnerJoin<C3>(Func<C0, C1, C2, C3, IODAColumns> On)
            where C3 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3> cmd = new JoinCmd<C0, C1, C2, C3>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = Ctx.GetCmd<C3>();
            this.Cmd0.InnerJoin(cmd.Cmd3, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3));
            return cmd;
        }
        #endregion
    }

    public class JoinCmd<C0, C1, C2, C3> : JoinCmd<C0, C1, C2>
        where C0 : ODACmd, new()
        where C1 : ODACmd, new()
        where C2 : ODACmd, new()
        where C3 : ODACmd, new()
    {
        #region 指今寄存
        public C3 Cmd3 { get; set; }
        #endregion

        #region ODA应用语法定义 

        public JoinCmd<C0, C1, C2, C3> OrderbyAsc(Func<C0, C1, C2, C3, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyAsc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3> OrderbyDesc(Func<C0, C1, C2, C3, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyDesc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3> Groupby(Func<C0, C1, C2, C3, IODAColumns[]> Groupby)
        {
            this.Cmd0.Groupby(Groupby(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3> Having(Func<C0, C1, C2, C3, IODAColumns> Having)
        {
            this.Cmd0.Having(Having(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3> Where(Func<C0, C1, C2, C3, IODAColumns> Where)
        {
            this.Cmd0.Where(Where(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3> And(Func<C0, C1, C2, C3, IODAColumns> And)
        {
            this.Cmd0.And(And(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3> Or(Func<C0, C1, C2, C3, IODAColumns> Or)
        {
            this.Cmd0.Or(Or(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
            return this;
        }

        #endregion

        #region 执行SQL语句  
        public int Count(Func<C0, C1, C2, C3, IODAColumns> Col)
        {
            return this.Cmd0.Count(Col(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
        }
        public DataTable Select(Func<C0, C1, C2, C3, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
        }
        public DataTable Select(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
        }
        public List<T> Select<T>(Func<C0, C1, C2, C3, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
        }
        public List<T> Select<T>(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
        }
        public object[] SelectFirst(Func<C0, C1, C2, C3, IODAColumns[]> Cols)
        {
            return this.Cmd0.SelectFirst(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3));
        }
        #endregion

        #region 表连接扩展 
        public JoinCmd<C0, C1, C2, C3, C4> LeftJoin<C4>(Func<C0, C1, C2, C3, C4, IODAColumns> On)
            where C4 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4> cmd = new JoinCmd<C0, C1, C2, C3, C4>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = Ctx.GetCmd<C4>();
            this.Cmd0.LeftJoin(cmd.Cmd4, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3, C4> RightJoin<C4>(Func<C0, C1, C2, C3, C4, IODAColumns> On)
            where C4 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4> cmd = new JoinCmd<C0, C1, C2, C3, C4>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = Ctx.GetCmd<C4>();
            this.Cmd0.RightJoin(cmd.Cmd4, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3, C4> InnerJoin<C4>(Func<C0, C1, C2, C3, C4, IODAColumns> On)
            where C4 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4> cmd = new JoinCmd<C0, C1, C2, C3, C4>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = Ctx.GetCmd<C4>();
            this.Cmd0.InnerJoin(cmd.Cmd4, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4));
            return cmd;
        }
        #endregion
    }

    public class JoinCmd<C0, C1, C2, C3, C4> : JoinCmd<C0, C1, C2, C3>
       where C0 : ODACmd, new()
       where C1 : ODACmd, new()
       where C2 : ODACmd, new()
       where C3 : ODACmd, new()
       where C4 : ODACmd, new()
    {
        #region 指今寄存
        public C4 Cmd4 { get; set; }
        #endregion

        #region ODA应用语法定义 

        public JoinCmd<C0, C1, C2, C3, C4> OrderbyAsc(Func<C0, C1, C2, C3, C4, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyAsc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4> OrderbyDesc(Func<C0, C1, C2, C3, C4, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyDesc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4> Groupby(Func<C0, C1, C2, C3, C4, IODAColumns[]> Groupby)
        {
            this.Cmd0.Groupby(Groupby(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4> Having(Func<C0, C1, C2, C3, C4, IODAColumns> Having)
        {
            this.Cmd0.Having(Having(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4> Where(Func<C0, C1, C2, C3, C4, IODAColumns> Where)
        {
            this.Cmd0.Where(Where(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4> And(Func<C0, C1, C2, C3, C4, IODAColumns> And)
        {
            this.Cmd0.And(And(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4> Or(Func<C0, C1, C2, C3, C4, IODAColumns> Or)
        {
            this.Cmd0.Or(Or(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
            return this;
        }

        #endregion

        #region 执行SQL语句  
        public int Count(Func<C0, C1, C2, C3, C4, IODAColumns> Col)
        {
            return this.Cmd0.Count(Col(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
        }
        public DataTable Select(Func<C0, C1, C2, C3, C4, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
        }
        public DataTable Select(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, C4, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
        }
        public List<T> Select<T>(Func<C0, C1, C2, C3, C4, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
        }
        public List<T> Select<T>(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, C4, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
        }
        public object[] SelectFirst(Func<C0, C1, C2, C3, C4, IODAColumns[]> Cols)
        {
            return this.Cmd0.SelectFirst(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4));
        }
        #endregion

        #region 表连接扩展 
        public JoinCmd<C0, C1, C2, C3, C4, C5> LeftJoin<C5>(Func<C0, C1, C2, C3, C4, C5, IODAColumns> On)
            where C5 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = Ctx.GetCmd<C5>();
            this.Cmd0.LeftJoin(cmd.Cmd5, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3, C4, C5> RightJoin<C5>(Func<C0, C1, C2, C3, C4, C5, IODAColumns> On)
            where C5 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = Ctx.GetCmd<C5>();
            this.Cmd0.RightJoin(cmd.Cmd5, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3, C4, C5> InnerJoin<C5>(Func<C0, C1, C2, C3, C4, C5, IODAColumns> On)
            where C5 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = Ctx.GetCmd<C4>();
            this.Cmd0.InnerJoin(cmd.Cmd5, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5));
            return cmd;
        }
        #endregion
    }

    public class JoinCmd<C0, C1, C2, C3, C4, C5> : JoinCmd<C0, C1, C2, C3, C4>
     where C0 : ODACmd, new()
     where C1 : ODACmd, new()
     where C2 : ODACmd, new()
     where C3 : ODACmd, new()
     where C4 : ODACmd, new()
     where C5 : ODACmd, new()
    {
        #region 指今寄存
        public C5 Cmd5 { get; set; }
        #endregion

        #region ODA应用语法定义 

        public JoinCmd<C0, C1, C2, C3, C4, C5> OrderbyAsc(Func<C0, C1, C2, C3, C4, C5, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyAsc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5> OrderbyDesc(Func<C0, C1, C2, C3, C4, C5, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyDesc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5> Groupby(Func<C0, C1, C2, C3, C4, C5, IODAColumns[]> Groupby)
        {
            this.Cmd0.Groupby(Groupby(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5> Having(Func<C0, C1, C2, C3, C4, C5, IODAColumns> Having)
        {
            this.Cmd0.Having(Having(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5> Where(Func<C0, C1, C2, C3, C4, C5, IODAColumns> Where)
        {
            this.Cmd0.Where(Where(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5> And(Func<C0, C1, C2, C3, C4, C5, IODAColumns> And)
        {
            this.Cmd0.And(And(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5> Or(Func<C0, C1, C2, C3, C4, C5, IODAColumns> Or)
        {
            this.Cmd0.Or(Or(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
            return this;
        }

        #endregion

        #region 执行SQL语句  
        public int Count(Func<C0, C1, C2, C3, C4, C5, IODAColumns> Col)
        {
            return this.Cmd0.Count(Col(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
        }
        public DataTable Select(Func<C0, C1, C2, C3, C4, C5, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
        }
        public DataTable Select(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, C4, C5, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
        }
        public List<T> Select<T>(Func<C0, C1, C2, C3, C4, C5, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
        }
        public List<T> Select<T>(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, C4, C5, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
        }
        public object[] SelectFirst(Func<C0, C1, C2, C3, C4, C5, IODAColumns[]> Cols)
        {
            return this.Cmd0.SelectFirst(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5));
        }
        #endregion

        #region 表连接扩展 
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6> LeftJoin<C6>(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns> On)
            where C6 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = this.Cmd5;
            cmd.Cmd6 = Ctx.GetCmd<C6>();
            this.Cmd0.LeftJoin(cmd.Cmd6, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3, C4, C5, C6> RightJoin<C6>(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns> On)
            where C6 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = this.Cmd5;
            cmd.Cmd6 = Ctx.GetCmd<C6>();
            this.Cmd0.RightJoin(cmd.Cmd6, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3, C4, C5, C6> InnerJoin<C6>(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns> On)
            where C6 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = this.Cmd5;
            cmd.Cmd6 = Ctx.GetCmd<C6>();
            this.Cmd0.InnerJoin(cmd.Cmd6, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6));
            return cmd;
        }
        #endregion
    }

    public class JoinCmd<C0, C1, C2, C3, C4, C5, C6> : JoinCmd<C0, C1, C2, C3, C4, C5>
     where C0 : ODACmd, new()
     where C1 : ODACmd, new()
     where C2 : ODACmd, new()
     where C3 : ODACmd, new()
     where C4 : ODACmd, new()
     where C5 : ODACmd, new()
     where C6 : ODACmd, new()
    {
        #region 指今寄存
        public C6 Cmd6 { get; set; }
        #endregion

        #region ODA应用语法定义 

        public JoinCmd<C0, C1, C2, C3, C4, C5, C6> OrderbyAsc(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyAsc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6> OrderbyDesc(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyDesc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6> Groupby(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns[]> Groupby)
        {
            this.Cmd0.Groupby(Groupby(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6> Having(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns> Having)
        {
            this.Cmd0.Having(Having(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6> Where(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns> Where)
        {
            this.Cmd0.Where(Where(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6> And(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns> And)
        {
            this.Cmd0.And(And(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6> Or(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns> Or)
        {
            this.Cmd0.Or(Or(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
            return this;
        }

        #endregion

        #region 执行SQL语句  
        public int Count(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns> Col)
        {
            return this.Cmd0.Count(Col(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
        }
        public DataTable Select(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
        }
        public DataTable Select(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
        }
        public List<T> Select<T>(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
        }
        public List<T> Select<T>(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
        }
        public object[] SelectFirst(Func<C0, C1, C2, C3, C4, C5, C6, IODAColumns[]> Cols)
        {
            return this.Cmd0.SelectFirst(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6));
        }
        #endregion

        #region 表连接扩展 
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> LeftJoin<C7>(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns> On)
            where C7 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = this.Cmd5;
            cmd.Cmd6 = this.Cmd6;
            cmd.Cmd7 = Ctx.GetCmd<C7>();
            this.Cmd0.LeftJoin(cmd.Cmd7, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6, cmd.Cmd7));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> RightJoin<C7>(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns> On)
            where C7 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = this.Cmd5;
            cmd.Cmd6 = this.Cmd6;
            cmd.Cmd7 = Ctx.GetCmd<C7>();
            this.Cmd0.RightJoin(cmd.Cmd7, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6, cmd.Cmd7));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> InnerJoin<C7>(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns> On)
            where C7 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = this.Cmd5;
            cmd.Cmd6 = this.Cmd6;
            cmd.Cmd7 = Ctx.GetCmd<C7>();
            this.Cmd0.InnerJoin(cmd.Cmd7, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6, cmd.Cmd7));
            return cmd;
        }
        #endregion
    }


    public class JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> : JoinCmd<C0, C1, C2, C3, C4, C5, C6>
        where C0 : ODACmd, new()
        where C1 : ODACmd, new()
        where C2 : ODACmd, new()
        where C3 : ODACmd, new()
        where C4 : ODACmd, new()
        where C5 : ODACmd, new()
        where C6 : ODACmd, new()
        where C7 : ODACmd, new()
    {
        #region 指今寄存
        public C7 Cmd7 { get; set; }
        #endregion

        #region ODA应用语法定义 

        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> OrderbyAsc(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyAsc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> OrderbyDesc(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyDesc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> Groupby(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns[]> Groupby)
        {
            this.Cmd0.Groupby(Groupby(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> Having(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns> Having)
        {
            this.Cmd0.Having(Having(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> Where(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns> Where)
        {
            this.Cmd0.Where(Where(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> And(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns> And)
        {
            this.Cmd0.And(And(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7> Or(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns> Or)
        {
            this.Cmd0.Or(Or(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
            return this;
        }

        #endregion

        #region 执行SQL语句  
        public int Count(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns> Col)
        {
            return this.Cmd0.Count(Col(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
        }
        public DataTable Select(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
        }
        public DataTable Select(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
        }
        public List<T> Select<T>(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
        }
        public List<T> Select<T>(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
        }
        public object[] SelectFirst(Func<C0, C1, C2, C3, C4, C5, C6, C7, IODAColumns[]> Cols)
        {
            return this.Cmd0.SelectFirst(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7));
        }
        #endregion

        #region 表连接扩展 
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8> LeftJoin<C8>(Func<C0, C1, C2, C3, C4, C5, C6, C7, C8, IODAColumns> On)
            where C8 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = this.Cmd5;
            cmd.Cmd6 = this.Cmd6;
            cmd.Cmd7 = this.Cmd7;
            cmd.Cmd8 = Ctx.GetCmd<C8>();
            this.Cmd0.LeftJoin(cmd.Cmd8, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6, cmd.Cmd7, cmd.Cmd8));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8> RightJoin<C8>(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns> On)
            where C8 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = this.Cmd5;
            cmd.Cmd6 = this.Cmd6;
            cmd.Cmd7 = this.Cmd7;
            cmd.Cmd8 = Ctx.GetCmd<C8>();
            this.Cmd0.RightJoin(cmd.Cmd8, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6, cmd.Cmd7, cmd.Cmd8));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8> InnerJoin<C8>(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns> On)
            where C8 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = this.Cmd5;
            cmd.Cmd6 = this.Cmd6;
            cmd.Cmd7 = this.Cmd7;
            cmd.Cmd8 = Ctx.GetCmd<C8>();
            this.Cmd0.InnerJoin(cmd.Cmd8, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6, cmd.Cmd7, cmd.Cmd8));
            return cmd;
        }
        #endregion
    }


    public class JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8> : JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7>
        where C0 : ODACmd, new()
        where C1 : ODACmd, new()
        where C2 : ODACmd, new()
        where C3 : ODACmd, new()
        where C4 : ODACmd, new()
        where C5 : ODACmd, new()
        where C6 : ODACmd, new()
        where C7 : ODACmd, new()
        where C8 : ODACmd, new()
    {
        #region 指今寄存
        public C8 Cmd8 { get; set; }
        #endregion

        #region ODA应用语法定义 

        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8> OrderbyAsc(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyAsc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8> OrderbyDesc(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyDesc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8> Groupby(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns[]> Groupby)
        {
            this.Cmd0.Groupby(Groupby(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8> Having(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns> Having)
        {
            this.Cmd0.Having(Having(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8> Where(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns> Where)
        {
            this.Cmd0.Where(Where(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8> And(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns> And)
        {
            this.Cmd0.And(And(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8> Or(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns> Or)
        {
            this.Cmd0.Or(Or(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
            return this;
        }

        #endregion

        #region 执行SQL语句  
        public int Count(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns> Col)
        {
            return this.Cmd0.Count(Col(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
        }
        public DataTable Select(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
        }
        public DataTable Select(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
        }
        public List<T> Select<T>(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
        }
        public List<T> Select<T>(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
        }
        public object[] SelectFirst(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8, IODAColumns[]> Cols)
        {
            return this.Cmd0.SelectFirst(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8));
        }
        #endregion

        #region 表连接扩展 
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8,C9> LeftJoin<C9>(Func<C0, C1, C2, C3, C4, C5, C6, C7, C8, C9, IODAColumns> On)
            where C9 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8,C9> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8,C9>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = this.Cmd5;
            cmd.Cmd6 = this.Cmd6;
            cmd.Cmd7 = this.Cmd7;
            cmd.Cmd8 = this.Cmd8;
            cmd.Cmd9 = Ctx.GetCmd<C9>();
            this.Cmd0.LeftJoin(cmd.Cmd9, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6, cmd.Cmd7, cmd.Cmd8,cmd.Cmd9));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8, C9> RightJoin<C9>(Func<C0, C1, C2, C3, C4, C5, C6, C7, C8, C9, IODAColumns> On)
            where C9 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8, C9> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8, C9>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = this.Cmd5;
            cmd.Cmd6 = this.Cmd6;
            cmd.Cmd7 = this.Cmd7;
            cmd.Cmd8 = this.Cmd8;
            cmd.Cmd9 = Ctx.GetCmd<C9>();
            this.Cmd0.RightJoin(cmd.Cmd9, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6, cmd.Cmd7, cmd.Cmd8, cmd.Cmd9));
            return cmd;
        }

        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8, C9> InnerJoin<C9>(Func<C0, C1, C2, C3, C4, C5, C6, C7, C8, C9, IODAColumns> On)
            where C9 : ODACmd, new()
        {
            JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8, C9> cmd = new JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8, C9>();
            cmd.Ctx = Ctx;
            cmd.Cmd0 = this.Cmd0;
            cmd.Cmd1 = this.Cmd1;
            cmd.Cmd2 = this.Cmd2;
            cmd.Cmd3 = this.Cmd3;
            cmd.Cmd4 = this.Cmd4;
            cmd.Cmd5 = this.Cmd5;
            cmd.Cmd6 = this.Cmd6;
            cmd.Cmd7 = this.Cmd7;
            cmd.Cmd8 = this.Cmd8;
            cmd.Cmd9 = Ctx.GetCmd<C9>();
            this.Cmd0.InnerJoin(cmd.Cmd9, On(this.Cmd0, cmd.Cmd1, cmd.Cmd2, cmd.Cmd3, cmd.Cmd4, cmd.Cmd5, cmd.Cmd6, cmd.Cmd7, cmd.Cmd8, cmd.Cmd9));
            return cmd;
        }
        #endregion
    }


    public class JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8,C9> : JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7, C8>
        where C0 : ODACmd, new()
        where C1 : ODACmd, new()
        where C2 : ODACmd, new()
        where C3 : ODACmd, new()
        where C4 : ODACmd, new()
        where C5 : ODACmd, new()
        where C6 : ODACmd, new()
        where C7 : ODACmd, new()
        where C8 : ODACmd, new()
        where C9 : ODACmd, new()
    {
        #region 指今寄存
        public C9 Cmd9 { get; set; }
        #endregion

        #region ODA应用语法定义 

        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9> OrderbyAsc(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyAsc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9> OrderbyDesc(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns[]> OrderBy)
        {
            this.Cmd0.OrderbyDesc(OrderBy(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9> Groupby(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns[]> Groupby)
        {
            this.Cmd0.Groupby(Groupby(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9> Having(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns> Having)
        {
            this.Cmd0.Having(Having(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9> Where(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns> Where)
        {
            this.Cmd0.Where(Where(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9> And(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns> And)
        {
            this.Cmd0.And(And(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
            return this;
        }
        public JoinCmd<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9> Or(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns> Or)
        {
            this.Cmd0.Or(Or(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
            return this;
        }

        #endregion

        #region 执行SQL语句  
        public int Count(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns> Col)
        {
            return this.Cmd0.Count(Col(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
        }
        public DataTable Select(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
        }
        public DataTable Select(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns[]> Cols)
        {
            return this.Cmd0.Select(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
        }
        public List<T> Select<T>(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
        }
        public List<T> Select<T>(int StartIndex, int MaxRecord, out int TotalRecord, Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns[]> Cols) where T : class
        {
            return this.Cmd0.Select<T>(StartIndex, MaxRecord, out TotalRecord, Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
        }
        public object[] SelectFirst(Func<C0, C1, C2, C3, C4, C5, C6, C7,C8,C9, IODAColumns[]> Cols)
        {
            return this.Cmd0.SelectFirst(Cols(this.Cmd0, this.Cmd1, this.Cmd2, this.Cmd3, this.Cmd4, this.Cmd5, this.Cmd6, this.Cmd7,this.Cmd8,this.Cmd9));
        }
        #endregion
    }

}
