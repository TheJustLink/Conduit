using System;
using System.Reflection;

namespace Conduit.Net.Extensions
{
    public static class TypeExtensions
    {
        public static FieldInfo[] GetDeclaredPublicFields(this Type type)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        }
    }
}