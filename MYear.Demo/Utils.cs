using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Reflection;


namespace MYear.Demo
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DemoAttribute : Attribute
    {
        public FuncType Demo { get; set; }
        public string MethodName { get; set; }
        public string MethodDescript { get; set; }
    }
    public enum FuncType
    {
        Insert,
        UpdateDelete,
        Select,
        Function, 
        Advance,
    }

    public class DemoMethodInfo
    {
        public FuncType DemoFunc { get; set; }
        public string MethodName { get; set; }
        public string MethodDescript { get; set; }
        public MethodInfo DemoMethod { get; set; }
    }  
}
