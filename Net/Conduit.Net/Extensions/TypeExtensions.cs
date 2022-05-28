using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Extensions
{
    public static class TypeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static FieldInfo[] GetDeclaredPublicFields(this Type type) =>
            type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
    }
}