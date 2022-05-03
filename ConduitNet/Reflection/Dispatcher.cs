using System;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Reflection
{
    public static class Dispatcher<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void Action(T target, object argument)
        {
            ActionDispatcher<T>.Dispatch(target, argument);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static object Func(T target, Type type)
        {
            return FuncDispatcher<T>.Dispatch(target, type);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static object Func(T target, int typeHashCode)
        {
            return FuncDispatcher<T>.Dispatch(target, typeHashCode);
        }
    }
}