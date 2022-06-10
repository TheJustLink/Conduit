using System;

using fNbt.Tags;

namespace Conduit.Net.IO.Binary
{
    public interface IWriter : IDisposable
    {
        bool CanWriteStatic(int id);
        
        void WriteObject(object @object, int id);

        void Write(bool value);
        void Write(byte value);
        void Write(sbyte value);
        void Write(byte[] buffer);
        void Write(ReadOnlySpan<byte> buffer);

        void Write(short value);
        void Write(int value);
        void Write(long value);
        void Write(ushort value);
        void Write(uint value);
        void Write(ulong value);

        void Write7BitEncodedInt(int value);
        void Write7BitEncodedInt64(long value);

        void Write(Half value);
        void Write(float value);
        void Write(double value);
        void Write(decimal value);

        void Write(char ch);
        void Write(char[] chars);
        void Write(ReadOnlySpan<char> chars);
        void Write(string value);

        void Write(Guid guid);
        void Write(NbtCompound tag);
    }
}