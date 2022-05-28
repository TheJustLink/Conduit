using System;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Converters
{
    public static class Enum
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static object ConvertObject(Type enumType, object value) => Convert.GetTypeCode(value) switch
        {
            TypeCode.Char => System.Enum.ToObject(enumType, Unsafe.Unbox<char>(value)),
            TypeCode.SByte => System.Enum.ToObject(enumType, Unsafe.Unbox<sbyte>(value)),
            TypeCode.Byte => System.Enum.ToObject(enumType, Unsafe.Unbox<byte>(value)),
            TypeCode.Int16 => System.Enum.ToObject(enumType, Unsafe.Unbox<short>(value)),
            TypeCode.UInt16 => System.Enum.ToObject(enumType, Unsafe.Unbox<ushort>(value)),
            TypeCode.Int32 => System.Enum.ToObject(enumType, Unsafe.Unbox<int>(value)),
            TypeCode.UInt32 => System.Enum.ToObject(enumType, Unsafe.Unbox<uint>(value)),
            TypeCode.Int64 => System.Enum.ToObject(enumType, Unsafe.Unbox<long>(value)),
            TypeCode.UInt64 => System.Enum.ToObject(enumType, Unsafe.Unbox<ulong>(value)),
            _ => throw new ArgumentException($"Can't convert {Convert.GetTypeCode(value)} to {enumType}", nameof(value))
        };
    }
}