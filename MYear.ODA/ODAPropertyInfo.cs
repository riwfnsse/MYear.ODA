using System;
using System.Reflection;

namespace MYear.ODA
{
    public class ODAPropertyInfo
    {
        public static bool IsNullable(Type t)
        {
            if (t.IsValueType)
            {
                return IsNullableType(t);
            }
            return true;
        }
        public static bool IsNullableType(Type t)
        {
            return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        #region 实体信息
        public PropertyInfo OriginProperty { get; private set; }
        public string PropertyName
        {
            get;
            private set;
        }
        public Type OriginType
        {
            get;
            private set;
        }
        public Type UnderlyingType
        {
            get;
            private set;
        }
        public bool IsNullableTypeProperty
        {
            get;
            private set;
        }
        public bool IsNullableProperty
        {
            get;
            private set;
        }
        #endregion 

        public ODAPropertyInfo(PropertyInfo Property)
        {
            if (Property == null)
                throw new ArgumentNullException(nameof(Property));
            OriginProperty = Property;
            OriginType = Property.PropertyType;
            PropertyName = Property.Name;
            IsNullableProperty = IsNullable(OriginType);
            IsNullableTypeProperty = IsNullableType(OriginType.UnderlyingSystemType);
            UnderlyingType = IsNullableProperty && IsNullableTypeProperty ? Nullable.GetUnderlyingType(OriginType) : OriginType;
        }
    }

}
