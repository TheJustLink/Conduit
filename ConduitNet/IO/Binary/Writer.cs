using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using fNbt;
using fNbt.Tags;

using Conduit.Net.Data;
using Conduit.Net.Reflection;

namespace Conduit.Net.IO.Binary
{
    public class Writer : BinaryWriter
    {
        private static readonly Dictionary<int, Action<Writer, object>> s_typeTable = new()
        {
            { Object<VarInt>.HashCode, (w, o) => w.Write7BitEncodedInt(Unsafe.Unbox<int>(o)) },
            { Object<VarLong>.HashCode, (w, o) => w.Write7BitEncodedInt64(Unsafe.Unbox<long>(o)) },
            { Object<bool>.HashCode, (w, o) => w.Write(Unsafe.Unbox<bool>(o)) },
            { Object<sbyte>.HashCode, (w, o) => w.Write(Unsafe.Unbox<sbyte>(o)) },
            { Object<byte>.HashCode, (w, o) => w.Write(Unsafe.Unbox<byte>(o)) },
            { Object<short>.HashCode, (w, o) => w.Write(Unsafe.Unbox<short>(o)) },
            { Object<ushort>.HashCode, (w, o) => w.Write(Unsafe.Unbox<ushort>(o)) },
            { Object<int>.HashCode, (w, o) => w.Write(Unsafe.Unbox<int>(o)) },
            { Object<uint>.HashCode, (w, o) => w.Write(Unsafe.Unbox<uint>(o)) },
            { Object<long>.HashCode, (w, o) => w.Write(Unsafe.Unbox<long>(o)) },
            { Object<ulong>.HashCode, (w, o) => w.Write(Unsafe.Unbox<ulong>(o)) },
            { Object<float>.HashCode, (w, o) => w.Write(Unsafe.Unbox<float>(o)) },
            { Object<double>.HashCode, (w, o) => w.Write(Unsafe.Unbox<double>(o)) },
            { Object<string>.HashCode, (w, o) => w.Write(Unsafe.As<string>(o)) },
            { Object<Guid>.HashCode, (w, o) => w.Write(Unsafe.Unbox<Guid>(o)) },
            { Object<NbtCompound>.HashCode, (w, o) => w.Write(Unsafe.As<NbtCompound>(o)) }
        };

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool CanWrite(int typeHashCode) => s_typeTable.ContainsKey(typeHashCode);
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool CanWrite(Type type) => s_typeTable.ContainsKey(type.GetHashCode());

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