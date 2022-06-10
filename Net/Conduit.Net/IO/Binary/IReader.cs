using System;

using fNbt.Tags;

namespace Conduit.Net.IO.Binary
{
    public interface IReader : IDisposable
    {
        bool CanReadTypeStatic(int id);
        
        object ReadObject(int id);

        int Read(Span<byte> buffer);
        int Read(Span<char> buffer);

        bool ReadBoolean();
        byte ReadByte();
        sbyte ReadSByte();
        byte[] ReadBytes(int count);

        short ReadInt16();
        int ReadInt32();
        long ReadInt64();
        ushort ReadUInt16();
        uint ReadUInt32();
        ulong ReadUInt64();

        int Read7BitEncodedInt();
        long Read7BitEncodedInt64();

        Half ReadHalf();
        float ReadSingle();
        double ReadDouble();
        decimal ReadDecimal();

        char ReadChar();
        char[] ReadChars(int count);
        string ReadString();

        Guid ReadGuid();
        NbtTag ReadNbt();
    }
}