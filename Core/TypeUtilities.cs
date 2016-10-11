using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core
{
    public static class TypeUtilities
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004", Justification = "This design is comfortable for the callers, and as simple as possible.")]
        public static IEnumerable<Type> GetTypes<T>()
        {
            return Assembly
                .GetAssembly(typeof(T))
                .GetTypes()
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract);
        }
    }
}
