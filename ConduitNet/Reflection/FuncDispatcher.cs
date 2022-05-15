using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Reflection
{
    public static class FuncDispatcher<T>
    {
        private static readonly Dictionary<int, Func<T, object>> s_table = ConvertMethodsToFunctions();

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool Contains(int typeHash)
        {
            return s_table.ContainsKey(typeHash);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Type type)
        {
            return s_table.ContainsKey(type.GetHashCode());
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static Dictionary<int, Func<T, object>> ConvertMethodsToFunctions()
        {
            var methods = typeof(T).GetMethods();
            var table = new Dictionary<int, Func<T, object>>(methods.Length);

            foreach (var methodInfo in methods)
            {
                if (Object.StandartMethodsHashcodes.Contains(methodInfo.Name.GetHashCode())) continue;
                if (methodInfo.GetParameters().Length != 0) continue;

                var returnType = methodInfo.ReturnType;
                var boxedMethod = FuncMethod<T>.ConvertToBoxedDelegate(methodInfo, returnType);

                table.Add(returnType.GetHashCode(), boxedMethod);
            }

            table.TrimExcess();

            return table;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static object Dispatch(T target, int typeHashCode)
        {
            return s_table[typeHashCode](target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static object Dispatch(T target, Type type)
        {
            return s_table[type.GetHashCode()](target);
        }
    }
}