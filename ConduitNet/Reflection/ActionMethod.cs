using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Reflection
{
    public static class ActionMethod<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Action<T, object> ConvertToBoxedDelegate(MethodInfo method, Type parameterType)
        {
            return Unsafe.As<Action<T, object>>(typeof(ActionMethod<T>).
                GetMethod(nameof(ConvertToDelegate), BindingFlags.Static | BindingFlags.Public).
                MakeGenericMethod(parameterType).
                Invoke(null, new object[] { method }));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Action<T, object> ConvertToDelegate<TParameter>(MethodInfo method) where TParameter : class
        {
            var action = method.CreateDelegate<Action<T, TParameter>>();
            return (target, parameter) => action(target, Unsafe.As<TParameter>(parameter));
        }
    }
}