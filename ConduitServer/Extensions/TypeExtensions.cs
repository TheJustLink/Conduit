using System;
using System.Reflection;

namespace ConduitServer.Extensions
{
    static class TypeExtensions
    {
        public static FieldInfo[] GetDeclaredPublicFields(this Type type)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        }
        public static bool IsStandartValueType(this Type type)
        {
            return type.IsValueType || type == typeof(string);
        }
    }
}