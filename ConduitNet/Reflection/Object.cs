using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Reflection
{
    public static class Object
    {
        public static readonly Assembly Assembly = typeof(Object).Assembly;
        public static readonly IEnumerable<TypeInfo> AssemblyTypes = Assembly.DefinedTypes;

        public static readonly HashSet<int> StandartMethodsHashcodes =
            (from method in typeof(object).GetMethods()
                select method.Name.GetHashCode()).ToHashSet();
    }
    public static class Object<T>
    {
        public static readonly int HashCode = typeof(T).GetHashCode();

        private static IEnumerable<TypeInfo> s_implementations;
        
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TypeInfo> GetImplementations() => s_implementations ??=
            Object.AssemblyTypes.Where(IsImplementation);
        public static bool IsImplementation(Type type) =>
            type.BaseType.GetHashCode() == HashCode
            && !type.IsAbstract;
    }
}