using System;
using System.Reflection;

namespace Core
{
    public static class TypeExtensions
    {
        public static string GetDisplayName(this MemberInfo @this)
        {
            int quoteIndex = @this.Name.IndexOf('`');
            if (quoteIndex < 0)
            {
                return @this.Name;
            }

            return @this.Name.Substring(0, quoteIndex);
        }

        public static T InstantiateWithDefaultConstructor<T>(this Type @this) where T : class
        {
            var constructor = @this.GetConstructor(Type.EmptyTypes);

            if (constructor == null)
            {
                throw new InvalidOperationException(@this.FullName + " does not have parameterless constructor.");
            }

            return constructor.Invoke(null) as T;
        }
    }
}
