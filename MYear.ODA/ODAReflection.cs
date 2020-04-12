using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MYear.ODA
{
    public static class ODAReflection
    {
        private static readonly MethodInfo IsDBNull = typeof(IDataRecord).GetMethod("IsDBNull", new Type[] { typeof(int) });
        private static readonly MethodInfo GetValue = typeof(IDataRecord).GetMethod("GetValue", new Type[] { typeof(int) });   
        private static readonly MethodInfo GetBoolean = typeof(IDataRecord).GetMethod("GetBoolean", new Type[] { typeof(int) });
        private static readonly MethodInfo GetByte = typeof(IDataRecord).GetMethod("GetByte", new Type[] { typeof(int) }); 
        private static readonly MethodInfo GetChar = typeof(IDataRecord).GetMethod("GetChar", new Type[] { typeof(int) });
        //private static readonly MethodInfo GetDateTime = typeof(IDataRecord).GetMethod("GetDateTime", new Type[] { typeof(int) });
        private static readonly MethodInfo GetDecimal = typeof(IDataRecord).GetMethod("GetDecimal", new Type[] { typeof(int) });
        private static readonly MethodInfo GetDouble = typeof(IDataRecord).GetMethod("GetDouble", new Type[] { typeof(int) });
        private static readonly MethodInfo GetFloat = typeof(IDataRecord).GetMethod("GetFloat", new Type[] { typeof(int) });
        private static readonly MethodInfo GetGuid = typeof(IDataRecord).GetMethod("GetGuid", new Type[] { typeof(int) });
        private static readonly MethodInfo GetInt16 = typeof(IDataRecord).GetMethod("GetInt16", new Type[] { typeof(int) });
        private static readonly MethodInfo GetInt32 = typeof(IDataRecord).GetMethod("GetInt32", new Type[] { typeof(int) });
        private static readonly MethodInfo GetInt64 = typeof(IDataRecord).GetMethod("GetInt64", new Type[] { typeof(int) });
        private static readonly MethodInfo GetString = typeof(IDataRecord).GetMethod("GetString", new Type[] { typeof(int) });

        private static readonly MethodInfo GetODADateTime = typeof(ODADataReader).GetMethod("GetODADateTime");
        private static readonly MethodInfo GetEnumDigit = typeof(ODADataReader).GetMethod("GetEnumDigit");
        private static readonly MethodInfo GetEnumString = typeof(ODADataReader).GetMethod("GetEnumString");
        private static readonly MethodInfo GetBytes = typeof(ODADataReader).GetMethod("GetBytes");
        private static readonly MethodInfo GetChars = typeof(ODADataReader).GetMethod("GetChars");
        private static readonly MethodInfo GetSbyte = typeof(ODADataReader).GetMethod("GetSbyte");
        private static readonly MethodInfo GetUInt32 = typeof(ODADataReader).GetMethod("GetUInt32");
        private static readonly MethodInfo GetUInt64 = typeof(ODADataReader).GetMethod("GetUInt64");
        private static readonly MethodInfo GetUInt16 = typeof(ODADataReader).GetMethod("GetUInt16");
        private static readonly MethodInfo GetDateTimeOffset = typeof(ODADataReader).GetMethod("GetDateTimeOffset");
        private static readonly MethodInfo GetDateTimeOffsetDateTime = typeof(ODADataReader).GetMethod("GetDateTimeOffsetDateTime");
        private static readonly MethodInfo GetSingleInt = typeof(ODADataReader).GetMethod("GetSingleInt");
        private static readonly MethodInfo GetSingleFloat = typeof(ODADataReader).GetMethod("GetSingleFloat");
        private static readonly MethodInfo GetSingleLong = typeof(ODADataReader).GetMethod("GetSingleLong"); 
        private static readonly MethodInfo GetTimeSpanInt = typeof(ODADataReader).GetMethod("GetTimeSpanInt");
        private static readonly MethodInfo GetTimeSpanLong = typeof(ODADataReader).GetMethod("GetTimeSpanLong");  
        private static readonly MethodInfo GetStringValue = typeof(ODADataReader).GetMethod("GetStringValue");
        private static readonly MethodInfo GetValueConvert = typeof(ODADataReader).GetMethod("GetValueConvert");

        private class ReadFieldInfo
        {
            public int FieldIndex = -1;
            public string FieldName = "";
            public Type FieldType = null;
        }

        private static SafeDictionary<string, object> CreatorsCache = new SafeDictionary<string, object>();
        public static SafeDictionary<Type, Type> DBTypeMapping { get; private set; } = new SafeDictionary<Type, Type>();

        public static Func<IDataReader, T> GetCreator<T>(IDataReader Reader)
        {
            object func = null;
            Type classType = typeof(T);
            ReadFieldInfo[]  FieldInfos = GetDataReaderFieldInfo(Reader); 
            StringBuilder sber = new StringBuilder().Append(classType.GetHashCode());
            for(int c = 0; c <  FieldInfos.Length; c ++)
            {
                sber.Append(FieldInfos[c].FieldType.Name);
            }  
            if (CreatorsCache.TryGetValue(sber.ToString(), out func))
                return (Func<IDataReader, T>)func; 

            var ppIndex = new List<ODAPropertyInfo>();
            PropertyInfo[] prptys = classType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < prptys.Length; i++)
            {
                if (prptys[i].CanWrite)
                {
                    ppIndex.Add(new ODAPropertyInfo(prptys[i]));
                }
            }
            DynamicMethod method;
            if (classType.IsInterface)
                method = new DynamicMethod("Create" + classType.Name, classType, new Type[] { typeof(IDataReader) }, classType.Module, true);
            else
                method = new DynamicMethod("Create" + classType.Name, classType, new Type[] { typeof(IDataReader) }, classType, true);
            ILGenerator il = method.GetILGenerator();
            LocalBuilder result = il.DeclareLocal(classType);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, classType.GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc, result);
            for (int i = 0; i < ppIndex.Count; i++)
            {
                ReadFieldInfo Field = null;
                foreach (var fld in FieldInfos)
                {
                    if (fld.FieldName == ppIndex[i].PropertyName)
                    {
                        Field = fld;
                        break;
                    }
                }

                var setter = ppIndex[i].OriginProperty.GetSetMethod(true);
                if (setter != null && Field != null)
                {
                    Label lb = il.DefineLabel();
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldc_I4, Field.FieldIndex);
                    il.Emit(OpCodes.Callvirt, IsDBNull);
                    il.Emit(OpCodes.Brtrue, lb);
                    il.Emit(OpCodes.Ldloc, result);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldc_I4, Field.FieldIndex);

                    BindReaderMethod(il, ppIndex[i], Field.FieldType);
                    if (setter.IsFinal || !setter.IsVirtual)
                    {
                        il.Emit(OpCodes.Call, setter);
                    }
                    else
                    {
                        il.Emit(OpCodes.Callvirt, setter);
                    }
                    il.MarkLabel(lb);
                }
            }
            il.Emit(OpCodes.Ldloc, result);
            il.Emit(OpCodes.Ret);
            object reator = method.CreateDelegate(typeof(Func<IDataReader, T>));
            CreatorsCache.Add(sber.ToString(), reator);
            GC.KeepAlive(reator);
            return (Func<IDataReader, T>)reator;
        }
        private static ReadFieldInfo[] GetDataReaderFieldInfo(IDataReader Reader)
        {
            ReadFieldInfo[] Fields = new ReadFieldInfo[Reader.FieldCount];
            var count = Reader.FieldCount;
            for (int i = 0; i < count; i++)
            {
                Type dbType = Reader.GetFieldType(i);
                Type CsType;
                //////不同数据库类型与CSharp类型对应
                if (DBTypeMapping.TryGetValue(dbType, out CsType))
                {
                    Fields[i] = new ReadFieldInfo() { FieldIndex = i, FieldName = Reader.GetName(i), FieldType = CsType };
                }
                else
                {
                    Fields[i] = new ReadFieldInfo() { FieldIndex = i, FieldName = Reader.GetName(i), FieldType = dbType };
                } 
            }
            return Fields;
        }
        private static void BindReaderMethod(ILGenerator il, ODAPropertyInfo PptyInfo,Type FieldType)
        {
            if (PptyInfo.UnderlyingType == FieldType)
            {
                if (PptyInfo.UnderlyingType == typeof(bool))
                {
                    il.Emit(OpCodes.Callvirt, GetBoolean);
                }
                else if (PptyInfo.UnderlyingType == typeof(byte))
                {
                    il.Emit(OpCodes.Callvirt, GetByte);
                }
                else if (PptyInfo.UnderlyingType == typeof(char))
                {
                    il.Emit(OpCodes.Callvirt, GetChar);
                }
                else if (PptyInfo.UnderlyingType == typeof(DateTime))
                {
                    // il.Emit(OpCodes.Callvirt, GetODADateTime); 
                    il.Emit(OpCodes.Call, GetODADateTime);
                }
                else if (PptyInfo.UnderlyingType == typeof(decimal))
                {
                    il.Emit(OpCodes.Callvirt, GetDecimal);
                }
                else if (PptyInfo.UnderlyingType == typeof(double))
                {
                    il.Emit(OpCodes.Callvirt, GetDouble);
                }
                else if (PptyInfo.UnderlyingType == typeof(float))
                {
                    il.Emit(OpCodes.Callvirt, GetFloat);
                }
                else if (PptyInfo.UnderlyingType == typeof(Guid))
                {
                    il.Emit(OpCodes.Callvirt, GetGuid);
                }
                else if (PptyInfo.UnderlyingType == typeof(short))
                {
                    il.Emit(OpCodes.Callvirt, GetInt16);
                }
                else if (PptyInfo.UnderlyingType == typeof(int))
                {
                    il.Emit(OpCodes.Callvirt, GetInt32);
                }
                else if (PptyInfo.UnderlyingType == typeof(long))
                {
                    il.Emit(OpCodes.Callvirt, GetInt64);
                }
                else if (PptyInfo.UnderlyingType == typeof(string))
                {
                    il.Emit(OpCodes.Callvirt, GetString);
                }
                else if (PptyInfo.UnderlyingType == typeof(byte[]))
                {
                    il.Emit(OpCodes.Call, GetBytes);
                }
                else if (PptyInfo.UnderlyingType == typeof(char[]))
                {
                    il.Emit(OpCodes.Call, GetChars);
                }
                else if (PptyInfo.UnderlyingType == typeof(sbyte))
                {
                    il.Emit(OpCodes.Call, GetSbyte);
                }
                else if (PptyInfo.UnderlyingType == typeof(uint))
                {
                    il.Emit(OpCodes.Call, GetUInt32);
                }
                else if (PptyInfo.UnderlyingType == typeof(ulong))
                {
                    il.Emit(OpCodes.Call, GetUInt64);
                }
                else if (PptyInfo.UnderlyingType == typeof(ushort))
                {
                    il.Emit(OpCodes.Call, GetUInt16);
                }
                else if (PptyInfo.UnderlyingType == typeof(DateTimeOffset))
                {
                    il.Emit(OpCodes.Call, GetDateTimeOffset);
                }
                else
                {
                    il.Emit(OpCodes.Callvirt, GetValue);
                    if (PptyInfo.OriginType.IsValueType)
                        il.Emit(OpCodes.Unbox_Any, PptyInfo.UnderlyingType);
                    else
                        il.Emit(OpCodes.Castclass, PptyInfo.UnderlyingType);
                }
            }
            else
            {
                if (PptyInfo.UnderlyingType.IsEnum)
                {
                    il.Emit(OpCodes.Ldtoken, PptyInfo.UnderlyingType);
                    il.EmitCall(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle)), null);
                    if (FieldType == typeof(string))
                        il.Emit(OpCodes.Call, GetEnumString);
                    else
                        il.Emit(OpCodes.Call, GetEnumDigit); 
                    il.Emit(OpCodes.Unbox_Any, PptyInfo.UnderlyingType);
                } 
                else if (PptyInfo.UnderlyingType == typeof(sbyte))
                {
                    il.Emit(OpCodes.Call, GetSbyte);
                }
                else if (PptyInfo.UnderlyingType == typeof(uint))
                {
                    il.Emit(OpCodes.Call, GetUInt32);
                }
                else if (PptyInfo.UnderlyingType == typeof(ulong))
                {
                    il.Emit(OpCodes.Call, GetUInt64);
                }
                else if (PptyInfo.UnderlyingType == typeof(ushort))
                {
                    il.Emit(OpCodes.Call, GetUInt16);
                }

                else if (PptyInfo.UnderlyingType == typeof(DateTimeOffset) && FieldType == typeof(DateTime))
                {
                    il.Emit(OpCodes.Call, GetDateTimeOffsetDateTime);
                }

                else if (PptyInfo.UnderlyingType == typeof(Single) && FieldType == typeof(int))
                {
                    il.Emit(OpCodes.Call, GetSingleInt);
                }
                else if (PptyInfo.UnderlyingType == typeof(Single) && FieldType == typeof(float))
                {
                    il.Emit(OpCodes.Call, GetSingleFloat);
                }
                else if (PptyInfo.UnderlyingType == typeof(Single) && FieldType == typeof(long))
                {
                    il.Emit(OpCodes.Call, GetSingleLong);
                }
                else if (PptyInfo.UnderlyingType == typeof(TimeSpan) && FieldType == typeof(int))
                {
                    il.Emit(OpCodes.Call, GetTimeSpanInt);
                }
                else if (PptyInfo.UnderlyingType == typeof(TimeSpan) && FieldType == typeof(long))
                {
                    il.Emit(OpCodes.Call, GetTimeSpanLong);
                }


                else if (PptyInfo.UnderlyingType == typeof(decimal) && FieldType == typeof(double))
                {
                    il.Emit(OpCodes.Callvirt, GetDouble);
                    il.Emit(OpCodes.Newobj, typeof(decimal).GetConstructor(new Type[] { typeof(double) }));
                }
                else if (PptyInfo.UnderlyingType == typeof(decimal) && FieldType == typeof(float))
                {
                    il.Emit(OpCodes.Callvirt, GetFloat);
                    il.Emit(OpCodes.Newobj, typeof(decimal).GetConstructor(new Type[] { typeof(float) }));
                }
                else if (PptyInfo.UnderlyingType == typeof(decimal) && FieldType == typeof(short))
                {
                    il.Emit(OpCodes.Callvirt, GetInt16);
                    il.Emit(OpCodes.Newobj, typeof(decimal).GetConstructor(new Type[] { typeof(short) }));
                }
                else if (PptyInfo.UnderlyingType == typeof(decimal) && FieldType == typeof(int))
                {
                    il.Emit(OpCodes.Callvirt, GetInt32);
                    il.Emit(OpCodes.Newobj, typeof(decimal).GetConstructor(new Type[] { typeof(int) }));
                }
                else if (PptyInfo.UnderlyingType == typeof(decimal) && FieldType == typeof(long))
                {
                    il.Emit(OpCodes.Callvirt, GetInt64);
                    il.Emit(OpCodes.Newobj, typeof(decimal).GetConstructor(new Type[] { typeof(long) }));
                }



                else if (PptyInfo.UnderlyingType == typeof(double) && FieldType == typeof(decimal))
                {
                    il.Emit(OpCodes.Callvirt, GetDecimal); 
                    il.EmitCall(OpCodes.Call, typeof(decimal).GetMethod("ToDouble"), null); 
                }
                else if (PptyInfo.UnderlyingType == typeof(double) && FieldType == typeof(float))
                {
                    il.Emit(OpCodes.Callvirt, GetFloat);
                    il.Emit(OpCodes.Conv_R8);
                }
                else if (PptyInfo.UnderlyingType == typeof(double) && FieldType == typeof(short))
                {
                    il.Emit(OpCodes.Callvirt, GetInt16);
                    il.Emit(OpCodes.Conv_R8);
                }
                else if (PptyInfo.UnderlyingType == typeof(double) && FieldType == typeof(int))
                {
                    il.Emit(OpCodes.Callvirt, GetInt32);
                    il.Emit(OpCodes.Conv_R8);
                }
                else if (PptyInfo.UnderlyingType == typeof(double) && FieldType == typeof(long))
                {
                    il.Emit(OpCodes.Callvirt, GetInt64);
                    il.Emit(OpCodes.Conv_R8);
                }
                

                else if (PptyInfo.UnderlyingType == typeof(float) && FieldType == typeof(decimal))
                {
                    il.Emit(OpCodes.Callvirt, GetDecimal);
                    il.Emit(OpCodes.Conv_R4);
                }
                else if (PptyInfo.UnderlyingType == typeof(float) && FieldType == typeof(double))
                {
                    il.Emit(OpCodes.Callvirt, GetDouble);
                    il.Emit(OpCodes.Conv_R4);
                }
                else if (PptyInfo.UnderlyingType == typeof(float) && FieldType == typeof(short))
                {
                    il.Emit(OpCodes.Callvirt, GetInt16);
                    il.Emit(OpCodes.Conv_R4);
                }
                else if (PptyInfo.UnderlyingType == typeof(float) && FieldType == typeof(int))
                {
                    il.Emit(OpCodes.Callvirt, GetInt32);
                    il.Emit(OpCodes.Conv_R4);
                }
                else if (PptyInfo.UnderlyingType == typeof(float) && FieldType == typeof(long))
                {
                    il.Emit(OpCodes.Callvirt, GetInt64);
                    il.Emit(OpCodes.Conv_R4);
                }

                else if (PptyInfo.UnderlyingType == typeof(short) && FieldType == typeof(decimal))
                {
                    il.Emit(OpCodes.Callvirt, GetDecimal); 
                    il.EmitCall(OpCodes.Call, typeof(decimal).GetMethod("ToInt16"), null);
                }
                else if (PptyInfo.UnderlyingType == typeof(short) && FieldType == typeof(double))
                {
                    il.Emit(OpCodes.Callvirt, GetDouble);
                    il.Emit(OpCodes.Conv_I2);
                }
                else if (PptyInfo.UnderlyingType == typeof(short) && FieldType == typeof(float))
                {
                    il.Emit(OpCodes.Callvirt, GetFloat);
                    il.Emit(OpCodes.Conv_I2);
                }
                else if (PptyInfo.UnderlyingType == typeof(short) && FieldType == typeof(int))
                {
                    il.Emit(OpCodes.Callvirt, GetInt32);
                    il.Emit(OpCodes.Conv_I2);
                }
                else if (PptyInfo.UnderlyingType == typeof(short) && FieldType == typeof(long))
                {
                    il.Emit(OpCodes.Callvirt, GetInt64);
                    il.Emit(OpCodes.Conv_I2);
                }

                else if (PptyInfo.UnderlyingType == typeof(int) && FieldType == typeof(decimal))
                {
                    il.Emit(OpCodes.Callvirt, GetDecimal); 
                    il.EmitCall(OpCodes.Call, typeof(decimal).GetMethod("ToInt32"), null);
                }
                else if (PptyInfo.UnderlyingType == typeof(int) && FieldType == typeof(double))
                {
                    il.Emit(OpCodes.Callvirt, GetDouble);
                    il.Emit(OpCodes.Conv_I4);
                }
                else if (PptyInfo.UnderlyingType == typeof(int) && FieldType == typeof(float))
                {
                    il.Emit(OpCodes.Callvirt, GetFloat);
                    il.Emit(OpCodes.Conv_I4);
                }
                else if (PptyInfo.UnderlyingType == typeof(int) && FieldType == typeof(short))
                {
                    il.Emit(OpCodes.Callvirt, GetInt16);
                    il.Emit(OpCodes.Conv_I4);
                }
                else if (PptyInfo.UnderlyingType == typeof(int) && FieldType == typeof(long))
                {
                    il.Emit(OpCodes.Callvirt, GetInt64);
                    il.Emit(OpCodes.Conv_I4);
                }

                else if (PptyInfo.UnderlyingType == typeof(long) && FieldType == typeof(decimal))
                {
                    il.Emit(OpCodes.Callvirt, GetDecimal); 
                    il.EmitCall(OpCodes.Call, typeof(decimal).GetMethod("ToInt64"), null);
                }
                else if (PptyInfo.UnderlyingType == typeof(long) && FieldType == typeof(double))
                {
                    il.Emit(OpCodes.Callvirt, GetDouble);
                    il.Emit(OpCodes.Conv_I8);
                }
                else if (PptyInfo.UnderlyingType == typeof(long) && FieldType == typeof(float))
                {
                    il.Emit(OpCodes.Callvirt, GetFloat);
                    il.Emit(OpCodes.Conv_I8);
                }
                else if (PptyInfo.UnderlyingType == typeof(long) && FieldType == typeof(short))
                {
                    il.Emit(OpCodes.Callvirt, GetInt16);
                    il.Emit(OpCodes.Conv_I8);
                }
                else if (PptyInfo.UnderlyingType == typeof(long) && FieldType == typeof(int))
                {
                    il.Emit(OpCodes.Callvirt, GetInt32);
                    il.Emit(OpCodes.Conv_I8);
                } 
                else if (PptyInfo.UnderlyingType == typeof(string))
                {
                    il.Emit(OpCodes.Call, GetStringValue);
                }
                else
                {
                    il.Emit(OpCodes.Ldtoken, PptyInfo.UnderlyingType);
                    il.EmitCall(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle)), null);
                    il.Emit(OpCodes.Call, GetValueConvert);
                    if (PptyInfo.OriginType.IsValueType)
                        il.Emit(OpCodes.Unbox_Any, PptyInfo.UnderlyingType);
                    else
                        il.Emit(OpCodes.Castclass, PptyInfo.UnderlyingType);
                }
            }
            if (PptyInfo.IsNullableTypeProperty)
                il.Emit(OpCodes.Newobj, PptyInfo.OriginType.GetConstructor(new Type[] { PptyInfo.UnderlyingType })); 
        } 
    }
}
 