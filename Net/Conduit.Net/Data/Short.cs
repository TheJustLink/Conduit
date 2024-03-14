using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Data
{
    public struct Short : ISerializable
    {
        public short Value;

        // Try FieldOffset performance

        public Short(short value) => Value = value;

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Read(Stream stream) => Value = (short)(
            stream.ReadByte() |
            (stream.ReadByte() << 8)
        );
        //[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        //public void Read1(Stream stream)
        //{
        //    Span<byte> data = stackalloc byte[2];
        //    stream.Read(data);

        //    Value = unchecked((short)(
        //        data[0] |
        //        (data[1] << 8)
        //    ));
        //}
        //[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        //public void Read2(Stream stream)
        //{
        //    Span<byte> data = stackalloc byte[2];
        //    stream.Read(data);

        //    Value = Unsafe.As<byte, short>(ref data[0]);
        //}
        //[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        //public void Read3(Stream stream)
        //{
        //    Span<byte> data = stackalloc byte[2];
        //    stream.Read(data);

        //    Value = Unsafe.ReadUnaligned<short>(ref data[0]);
        //}

        public void Write(Stream stream)
        {
            stream.Write(stackalloc byte[] { (byte)(Value >> 8), (byte)Value });
        }
        public void Write2(Stream stream)
        {   
            Span<byte> span = stackalloc byte[2];
            Unsafe.As<byte, short>(ref span[0]) = Value;

            stream.Write(span);
        }
        public void Write3(Stream stream)
        {
            Span<byte> span = stackalloc byte[2];
            Unsafe.WriteUnaligned(ref span[0], Value);

            stream.Write(span);
        }

        public static implicit operator short(Short value) => value.Value;
        public static implicit operator Short(short value) => new(value);
    }
}