using System;
using System.Reflection;

namespace Core
{
    public static class MemberInfoExtensions
    {
        public static Type GetFieldOrPropertyType(this MemberInfo @this)
        {
            var field = @this as FieldInfo;

            return field != null
                ? field.FieldType
                : ((PropertyInfo)@this).PropertyType;
        }
    }
}
