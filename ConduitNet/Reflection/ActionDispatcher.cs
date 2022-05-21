using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Reflection
{
    public static class ActionDispatcher<T>
    {
        private static readonly Dictionary<int, Action<T, object>> s_table = ConvertMethodsToActions();

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool Contains(int typeHash) => s_table.ContainsKey(typeHash);
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Type type) => s_table.ContainsKey(type.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static Dictionary<int, Action<T, object>> ConvertMethodsToActions()
        {
            var methods = typeof(T).GetMethods();
            var table = new Dictionary<int, Action<T, object>>(methods.Length);

            foreach (var methodInfo in methods)
            {
                if (methodInfo.ReturnType != typeof(void)) continue;
                if (Object.StandartMethodsHashcodes.Contains(methodInfo.Name.GetHashCode())) continue;

                var parameters = methodInfo.GetParameters();
                if (parameters.Length != 1) continue;

                var parameterType = parameters[0].ParameterType;
                var boxedMethod = ActionMethod<T>.ConvertToBoxedDelegate(methodInfo, parameterType);

                table.Add(parameterType.GetHashCode(), boxedMethod);
            }

            table.TrimExcess();

            return table;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void Dispatch(T target, object argument)
        {
            s_table[argument.GetType().GetHashCode()](target, argument);
        }
    }
}