using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Data
{
    public struct VarInt : ISerializable
    {
        private static readonly FormatException s_formatException = new("Bad 7BitInt format");
        
        public int Value;

        public VarInt(int value) => Value = value;

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Read(Stream stream)
        {
            unchecked
            {
                uint result = 0;
                byte byteReadJustNow;

                const int MaxBytesWithoutOverflow = 4;

                for (var shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
                {
                    byteReadJustNow = (byte)stream.ReadByte();
                    result |= (byteReadJustNow & 0x7Fu) << shift;

                    if (byteReadJustNow <= 0x7Fu)
                    {
                        Value = (int)result;
                        return;
                    }
                }

                byteReadJustNow = (byte)stream.ReadByte();
                if (byteReadJustNow > 0b_1111u)
                    throw s_formatException;

                result |= (uint)byteReadJustNow << (MaxBytesWithoutOverflow * 7);

                Value = (int)result;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Write(Stream stream)
        {
            var uValue = (uint)Value;

            while (uValue > 0x7Fu)
            {
                stream.WriteByte((byte)(uValue | ~0x7Fu));
                uValue >>= 7;
            }

            stream.WriteByte((byte)uValue);
        }

        public static implicit operator int(VarInt value) => value.Value;
        public static implicit operator VarInt(int value) => new(value);
    }
}