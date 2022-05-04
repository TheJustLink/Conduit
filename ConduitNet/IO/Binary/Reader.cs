using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using fNbt;
using fNbt.Tags;

using Conduit.Net.Data;

namespace Conduit.Net.IO.Binary
{
    public class Reader : BinaryReader
    {
        private static readonly Dictionary<int, Func<Reader, object>> s_typeTable = new()
        {
            { typeof(bool).GetHashCode(), r => r.ReadBoolean() },
            { typeof(sbyte).GetHashCode(), r => r.ReadSByte() },
            { typeof(byte).GetHashCode(), r => r.ReadByte() },
            { typeof(short).GetHashCode(), r => r.ReadInt16() },
            { typeof(ushort).GetHashCode(), r => r.ReadUInt16() },
            { typeof(int).GetHashCode(), r => r.ReadInt32() },
            { typeof(long).GetHashCode(), r => r.ReadInt64() },
            { typeof(float).GetHashCode(), r => r.ReadSingle() },
            { typeof(double).GetHashCode(), r => r.ReadDouble() },
            { typeof(string).GetHashCode(), r => r.ReadString() },
            { typeof(Guid).GetHashCode(), r => r.ReadGuid() },
            { typeof(VarInt).GetHashCode(), r => r.Read7BitEncodedInt() },
            { typeof(VarLong).GetHashCode(), r => r.Read7BitEncodedInt64() },
            { typeof(NbtCompound).GetHashCode(), r => r.ReadNbt() },
            { typeof(NbtTag).GetHashCode(), r => r.ReadNbt() }
        };

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool CanReadType(int typeHashCode) => s_typeTable.ContainsKey(typeHashCode);
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool CanReadType(Type type) => s_typeTable.ContainsKey(type.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static object ReadObject(Reader reader, int typeHashCode)
        {
            return s_typeTable[typeHashCode](reader);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static object ReadObject(Reader reader, Type type)
        {
            return s_typeTable[type.GetHashCode()](reader);
        }

        public Reader(byte[] data) : this(new MemoryStream(data, false), Encoding.UTF8, true) { }
        public Reader(Stream input) : base(input) { }
        public Reader(Stream input, Encoding encoding, bool leaveOpen = false) : base(input, encoding, leaveOpen) { }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public object ReadObject(int typeHashCode)
        {
            return s_typeTable[typeHashCode](this);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public object ReadObject(Type type)
        {
            return s_typeTable[type.GetHashCode()](this);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public override ushort ReadUInt16()
        {
            return BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(2));
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public virtual Guid ReadGuid()
        {
            return new Guid(ReadBytes(16));
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public NbtTag ReadNbt()
        {
            return new NbtReader(BaseStream).ReadAsTag();
        }
    }
}