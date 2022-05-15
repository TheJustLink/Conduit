using System;
using System.Reflection;

namespace Conduit.Net.Extensions
{
    public static class TypeInfoExtensions
    {
        public static bool HasAttribute<T>(this TypeInfo typeInfo) where T : Attribute =>
            typeInfo.GetCustomAttribute<T>() != null;
    }
}