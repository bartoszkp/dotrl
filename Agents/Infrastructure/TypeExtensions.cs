using System;

namespace Agents.Infrastructure
{
    public static class TypeExtensions
    {
        public static bool IsAgent(this Type @this)
        {
            return !@this.IsAbstract
                && @this.BaseType != null
                && @this.BaseType.IsGenericType
                && @this.BaseType.GetGenericTypeDefinition().Equals(typeof(Agent<,>));
        }
    }
}
