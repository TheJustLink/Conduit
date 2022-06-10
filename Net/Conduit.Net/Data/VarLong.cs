using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Data
{
    public struct VarLong : ISerializable
    {
        private static readonly FormatException s_formatException = new("Bad 7BitLong format");
        
        public long Value;

        public VarLong(long value) => Value = value;
        
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Read(Stream stream)
        {
            unchecked
            {
                ulong result = 0;
                byte byteReadJustNow;

                const int MaxBytesWithoutOverflow = 9;

                for (var shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
                {
                    byteReadJustNow = (byte)stream.ReadByte();
                    result |= (byteReadJustNow & 0x7Ful) << shift;

                    if (byteReadJustNow <= 0x7Fu)
                    {
                        Value = (long)result;
                        return;
                    }
                }

                byteReadJustNow = (byte)stream.ReadByte();
                if (byteReadJustNow > 0b_1u)
                    throw s_formatException;

                result |= (ulong)byteReadJustNow << (MaxBytesWithoutOverflow * 7);

                Value = (long)result;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Write(Stream stream)
        {
            var uValue = (ulong)Value;

            while (uValue > 0x7Fu)
            {
                stream.WriteByte((byte)((uint)uValue | ~0x7Fu));
                uValue >>= 7;
            }

            stream.WriteByte((byte)uValue);
        }

        public static implicit operator long(VarLong value) => value.Value;
        public static implicit operator VarLong(long value) => new(value);
    }
}