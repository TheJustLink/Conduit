using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

using fNbt;
using fNbt.Tags;

using Conduit.Net.Data;

namespace Conduit.Net.IO.Binary
{
    public class Reader : BinaryReader
    {
        private static readonly Dictionary<Type, Func<Reader, object>> s_typeTable = new()
        {
            { typeof(bool), r => r.ReadBoolean() },
            { typeof(sbyte), r => r.ReadSByte() },
            { typeof(byte), r => r.ReadByte() },
            { typeof(short), r => r.ReadInt16() },
            { typeof(ushort), r => r.ReadUInt16() },
            { typeof(int), r => r.ReadInt32() },
            { typeof(long), r => r.ReadInt64() },
            { typeof(float), r => r.ReadSingle() },
            { typeof(double), r => r.ReadDouble() },
            { typeof(string), r => r.ReadString() },
            { typeof(Guid), r => r.ReadGuid() },
            { typeof(VarInt), r => r.Read7BitEncodedInt() },
            { typeof(VarLong), r => r.Read7BitEncodedInt64() },
            { typeof(NbtCompound), r => r.ReadNbt() },
            { typeof(NbtTag), r => r.ReadNbt() }
        };
        public static bool CanReadType(Type type) => s_typeTable.ContainsKey(type);

        public Reader(byte[] data) : this(new MemoryStream(data, false), Encoding.UTF8, true) { }
        public Reader(Stream input) : base(input) { }
        public Reader(Stream input, Encoding encoding, bool leaveOpen = false) : base(input, encoding, leaveOpen) { }
        
        public override ushort ReadUInt16()
        {
            return BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(2));
        }
        public virtual Guid ReadGuid()
        {
            return new Guid(ReadBytes(16));
        }
        public NbtTag ReadNbt()
        {
            return new NbtReader(BaseStream).ReadAsTag();
        }
    }
}