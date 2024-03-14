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
                { Object<VarInt>.Id, static r => r.Read7BitEncodedInt() },
                { Object<VarLong>.Id, static r => r.Read7BitEncodedInt64() },
                { Object<bool>.Id, static r => r.ReadBoolean() },
                { Object<sbyte>.Id, static r => r.ReadSByte() },
                { Object<byte>.Id, static r => r.ReadByte() },
                { Object<short>.Id, static r => r.ReadInt16() },
                { Object<ushort>.Id, static r => r.ReadUInt16() },
                { Object<int>.Id, static r => r.ReadInt32() },
                { Object<long>.Id, static r => r.ReadInt64() },
                { Object<float>.Id, static r => r.ReadSingle() },
                { Object<double>.Id, static r => r.ReadDouble() },
                { Object<string>.Id, static r => r.ReadString() },
                { Object<Guid>.Id, static r => r.ReadGuid() },
                { Object<NbtCompound>.Id, static r => r.ReadNbt() }
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