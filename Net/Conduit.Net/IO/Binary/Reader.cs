using System;
using System.IO;
using System.Text;
using System.Buffers.Binary;
using System.Collections.Generic;

using fNbt;
using fNbt.Tags;

using Conduit.Net.Data;
using Conduit.Net.Reflection;

namespace Conduit.Net.IO.Binary
{
    public class Reader : BinaryReader, IReader
    {
        private static readonly Func<Reader, object>[] s_readers;
        
        static Reader()
        {
            var dictionary = new Dictionary<int, Func<Reader, object>>
            {
                { Object<VarInt>.Id, r => r.Read7BitEncodedInt() },
                { Object<VarLong>.Id, r => r.Read7BitEncodedInt64() },
                { Object<bool>.Id, r => r.ReadBoolean() },
                { Object<sbyte>.Id, r => r.ReadSByte() },
                { Object<byte>.Id, r => r.ReadByte() },
                { Object<short>.Id, r => r.ReadInt16() },
                { Object<ushort>.Id, r => r.ReadUInt16() },
                { Object<int>.Id, r => r.ReadInt32() },
                { Object<long>.Id, r => r.ReadInt64() },
                { Object<float>.Id, r => r.ReadSingle() },
                { Object<double>.Id, r => r.ReadDouble() },
                { Object<string>.Id, r => r.ReadString() },
                { Object<Guid>.Id, r => r.ReadGuid() },
                { Object<NbtCompound>.Id, r => r.ReadNbt() }
            };
            
            s_readers = new Func<Reader, object>[dictionary.Count];
            foreach (var pair in dictionary)
                s_readers[pair.Key] = pair.Value;
        }
        
        public static bool CanReadType(int id) => s_readers.Length > id;
        public static object ReadObject(Reader reader, int id) => s_readers[id](reader);
        
        public Reader(Stream input) : base(input) { }
        public Reader(byte[] data) : this(new MemoryStream(data, false), Encoding.UTF8) { }
        public Reader(Stream input, Encoding encoding, bool leaveOpen = false) : base(input, encoding, leaveOpen) { }
        
        public bool CanReadTypeStatic(int id) => CanReadType(id);

        public object ReadObject(int id) => ReadObject(this, id);
        
        public override ushort ReadUInt16() => BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(2));
        public Guid ReadGuid() => new(ReadBytes(16));
        public NbtTag ReadNbt() => new NbtReader(BaseStream).ReadAsTag();
    }
}