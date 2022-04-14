using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Conduit.Net.IO.Binary
{
    public class Reader : BinaryReader
    {
        private readonly Dictionary<Type, Func<object>> _typeTable;

        public Reader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            _typeTable = new Dictionary<Type, Func<object>>
            {
                { typeof(bool), () => ReadBoolean },
                { typeof(sbyte), () => ReadSByte },
                { typeof(byte), () => ReadByte },
                { typeof(short), () => ReadInt16 },
                { typeof(ushort), () => ReadUInt16 },
                { typeof(int), () => ReadInt32 },
                { typeof(long), () => ReadInt64 },
                { typeof(float), () => ReadSingle },
                { typeof(double), () => ReadDouble },
                { typeof(string), () => ReadString },
                { typeof(Guid), () => ReadGuid }
            };
        }

        public virtual object ReadObject(Type type)
        {
            return _typeTable[type]();
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