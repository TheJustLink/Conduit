using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using fNbt;
using fNbt.Tags;

using Conduit.Net.Reflection;

namespace Conduit.Net.IO.Binary
{
    public class Reader : BinaryReader
    {
        private static readonly Dictionary<int, Func<Reader, object>> s_typeTable = new()
        {
            { Object<bool>.HashCode, r => r.ReadBoolean() },
            { Object<sbyte>.HashCode, r => r.ReadSByte() },
            { Object<byte>.HashCode, r => r.ReadByte() },
            { Object<short>.HashCode, r => r.ReadInt16() },
            { Object<ushort>.HashCode, r => r.ReadUInt16() },
            { Object<int>.HashCode, r => r.ReadInt32() },
            { Object<long>.HashCode, r => r.ReadInt64() },
            { Object<float>.HashCode, r => r.ReadSingle() },
            { Object<double>.HashCode, r => r.ReadDouble() },
            { Object<string>.HashCode, r => r.ReadString() },
            { Object<Guid>.HashCode, r => r.ReadGuid() },
            { Object<NbtCompound>.HashCode, r => r.ReadNbt() }
        };

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool CanReadType(int typeHashCode) => s_typeTable.ContainsKey(typeHashCode);
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool CanReadType(Type type) => s_typeTable.ContainsKey(type.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static object ReadObject(Reader reader, int typeHashCode) => s_typeTable[typeHashCode](reader);
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static object ReadObject(Reader reader, Type type) => s_typeTable[type.GetHashCode()](reader);

        public Reader(byte[] data) : this(new MemoryStream(data, false), Encoding.UTF8, true) { }
        public Reader(Stream input) : base(input) { }
        public Reader(Stream input, Encoding encoding, bool leaveOpen = false) : base(input, encoding, leaveOpen) { }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public object ReadObject(int typeHashCode) => s_typeTable[typeHashCode](this);
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public object ReadObject(Type type) => s_typeTable[type.GetHashCode()](this);

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public override ushort ReadUInt16() => BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(2));
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public virtual Guid ReadGuid() => new(ReadBytes(16));
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public NbtTag ReadNbt() => new NbtReader(BaseStream).ReadAsTag();
    }
}