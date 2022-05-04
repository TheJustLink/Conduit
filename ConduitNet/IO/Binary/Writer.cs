using System;
using System.IO;
using System.Text;

using fNbt.Tags;
using fNbt;

using Conduit.Net.Data;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Conduit.Net.IO.Binary
{
    public class Writer : BinaryWriter
    {
        private static readonly Dictionary<int, Action<Writer, object>> s_typeTable = new()
        {
            { typeof(bool).GetHashCode(), (w, o) => w.Write((bool)o) },
            { typeof(sbyte).GetHashCode(), (w, o) => w.Write((sbyte)o) },
            { typeof(byte).GetHashCode(), (w, o) => w.Write((byte)o) },
            { typeof(short).GetHashCode(), (w, o) => w.Write((short)o) },
            { typeof(ushort).GetHashCode(), (w, o) => w.Write((ushort)o) },
            { typeof(int).GetHashCode(), (w, o) => w.Write((int)o) },
            { typeof(uint).GetHashCode(), (w, o) => w.Write((uint)o) },
            { typeof(long).GetHashCode(), (w, o) => w.Write((long)o) },
            { typeof(ulong).GetHashCode(), (w, o) => w.Write((ulong)o) },
            { typeof(float).GetHashCode(), (w, o) => w.Write((float)o) },
            { typeof(double).GetHashCode(), (w, o) => w.Write((double)o) },
            { typeof(string).GetHashCode(), (w, o) => w.Write((string)o) },
            { typeof(Guid).GetHashCode(), (w, o) => w.Write((Guid)o) },
            { typeof(VarInt).GetHashCode(), (w, o) => w.Write7BitEncodedInt((int)o) },
            { typeof(VarLong).GetHashCode(), (w, o) => w.Write7BitEncodedInt64((long)o) },
            { typeof(NbtCompound).GetHashCode(), (w, o) => w.Write((NbtCompound)o) }
        };

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool CanWriteType(int typeHashCode) => s_typeTable.ContainsKey(typeHashCode);
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool CanWriteType(Type type) => s_typeTable.ContainsKey(type.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static void WriteObject(Writer writer, object @object)
        {
            s_typeTable[@object.GetType().GetHashCode()](writer, @object);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static void WriteObject(Writer writer, object @object, Type type)
        {
            s_typeTable[type.GetHashCode()](writer, @object);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static void WriteObject(Writer writer, object @object, int typeHashCode)
        {
            s_typeTable[typeHashCode](writer, @object);
        }

        public Writer() : this(new MemoryStream(), Encoding.UTF8, false) { }
        public Writer(Stream output) : base(output) { }
        public Writer(Stream output, Encoding encoding) : base(output, encoding) { }
        public Writer(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen) { }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public virtual void WriteObject(object @object)
        {
            s_typeTable[@object.GetType().GetHashCode()](this, @object);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public virtual void WriteObject(object @object, Type type)
        {
            s_typeTable[type.GetHashCode()](this, @object);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public virtual void WriteObject(object @object, int typeHashCode)
        {
            s_typeTable[typeHashCode](this, @object);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public virtual void Write(Guid guid)
        {
            base.Write(guid.ToByteArray());
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public virtual void Write(NbtCompound tag)
        {
            var nbtFile = new NbtFile(tag);
            nbtFile.SaveToStream(BaseStream, NbtCompression.None);
        }
    }
}