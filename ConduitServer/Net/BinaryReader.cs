using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConduitServer.Net
{
    class BinaryReader : System.IO.BinaryReader
    {
        private static readonly Dictionary<Type, Func<BinaryReader, object>> s_typeTable = new()
        {
            { typeof(bool), input => input.ReadBoolean() },
            { typeof(sbyte), input => input.ReadSByte() },
            { typeof(byte), input => input.ReadByte() },
            { typeof(short), input => input.ReadInt16() },
            { typeof(ushort), input => input.ReadUInt16() },
            { typeof(int), input => input.ReadInt32() },
            { typeof(long), input => input.ReadInt64() },
            { typeof(float), input => input.ReadSingle() },
            { typeof(double), input => input.ReadDouble() },
            { typeof(string), input => input.ReadString() },
            { typeof(Guid), input => input.ReadGuid() }
        };

        public BinaryReader(Stream input) : base(input) { }
        public BinaryReader(Stream input, Encoding encoding) : base(input, encoding) { }
        public BinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) { }

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