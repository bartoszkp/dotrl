using System;

namespace Environments.Infrastructure
{
    public static class TypeExtensions
    {
        public static bool IsEnvironment(this Type @this)
        {
            return !@this.IsAbstract
                && @this.BaseType != null 
                && @this.BaseType.IsGenericType 
                && @this.BaseType.GetGenericTypeDefinition().Equals(typeof(Environment<,>));
        }
    }
}
