using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Reflection
{
    public static class FuncMethod<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Func<T, object> ConvertToBoxedDelegate(MethodInfo method, Type returnType)
        {
            return Unsafe.As<Func<T, object>>(typeof(FuncMethod<T>).
                GetMethod(nameof(ConvertToDelegate), BindingFlags.Static | BindingFlags.Public).
                MakeGenericMethod(returnType).
                Invoke(null, new object[] { method }));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Func<T, object> ConvertToDelegate<TOut>(MethodInfo method)
        {
            var action = method.CreateDelegate<Func<T, TOut>>();
            return target => action(target);
        }
    }
}