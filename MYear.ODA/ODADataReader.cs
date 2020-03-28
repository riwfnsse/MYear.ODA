using System;
using System.Data;
using System.Globalization;

namespace MYear.ODA
{
    public static class ODADataReader
    { 
        public static object GetEnumDigit(this IDataRecord dr, int i,Type EnumType )
        { 
            return Enum.ToObject(EnumType, dr.GetValue(i));
        } 
        public static object GetEnumString(this IDataRecord dr, int i, Type EnumType)
        {
            return Enum.Parse(EnumType, dr.GetString(i));
        }
        public static byte[] GetBytes(this IDataRecord dr, int i)
        {
           return dr.GetValue(i) as byte[];
        }
        public static char[] GetChars(this IDataRecord dr, int i)
        {
            return dr.GetValue(i) as char[];
        } 
        public static sbyte GetSbyte(this IDataRecord dr, int i)
        {
            sbyte v = 0x00;
            sbyte.TryParse(dr.GetValue(i).ToString(), out v);
            return v;
        }   
        public static uint GetUInt32(this IDataRecord dr, int i)
        {
            uint v = 0x00;
            uint.TryParse(dr.GetValue(i).ToString(), out v);
            return v; 
        } 
        public static ulong GetUInt64(this IDataRecord dr,  int i)
        {
            ulong v = 0x00;
            ulong.TryParse(dr.GetValue(i).ToString(), out v);
            return v;
        } 
        public static ushort GetUInt16(this IDataRecord dr, int i)
        {
            ushort v = 0x0;
            ushort.TryParse(dr.GetValue(i).ToString(), out v);
            return v; 
        } 
         
        public static DateTimeOffset GetDateTimeOffset(this IDataRecord dr, int i)
        {
            return (DateTimeOffset)dr.GetValue(i);
        }
        public static DateTimeOffset GetDateTimeOffsetDateTime(this IDataRecord dr, int i)
        {
            return new DateTimeOffset(dr.GetDateTime(i));
        }

        public static Single GetSingleInt(this IDataRecord dr, int i)
        {
            return dr.GetInt32(i);
        }
        public static Single GetSingleFloat(this IDataRecord dr, int i)
        {
            return dr.GetFloat(i);
        }
        public static Single GetSingleLong(this IDataRecord dr, int i)
        {
            return dr.GetInt64(i);
        } 
        public static TimeSpan GetTimeSpanInt(this IDataRecord dr, int i)
        {
            return new TimeSpan(dr.GetInt32(i));
        }
        public static TimeSpan GetTimeSpanLong(this IDataRecord dr, int i)
        {
            return new TimeSpan(dr.GetInt64(i));
        } 
        public static string GetStringValue(this IDataRecord dr, int i)
        {
            return dr.GetValue(i).ToString();
        }
        public static object GetValueConvert(this IDataRecord dr, int i, Type TargetType)
        {
            return Convert.ChangeType(dr.GetValue(i), TargetType, CultureInfo.CurrentCulture);
        }
    }
}
