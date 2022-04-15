using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            { typeof(Guid), r => r.ReadGuid() }
        };

        public Reader(byte[] data) : this(new MemoryStream(data, false), Encoding.UTF8, true) { }
        public Reader(Stream input, Encoding encoding, bool leaveOpen = false) : base(input, encoding, leaveOpen) { }

        public virtual object ReadObject(Type type)
        {
            return s_typeTable[type](this);
        }

        public override ushort ReadUInt16()
        {
            return BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(2));
        }
        public virtual Guid ReadGuid()
        {
            return new Guid(ReadBytes(16));
        }
    }
}