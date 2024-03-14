using System;
using System.IO;
using System.Text;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using fNbt;
using fNbt.Tags;

using Conduit.Net.Data;
using Conduit.Net.Reflection;

namespace Conduit.Net.IO.Binary
{
    public class Writer : BinaryWriter, IWriter
    {
        private static readonly Action<Writer, object>[] s_writers;

        static Writer()
        {
            var dictionary = new Dictionary<int, Action<Writer, object>>
            {
                { Object<VarInt>.Id, static (w, o) => w.Write7BitEncodedInt(Unsafe.Unbox<int>(o)) },
                { Object<VarLong>.Id, static (w, o) => w.Write7BitEncodedInt64(Unsafe.Unbox<long>(o)) },
                { Object<bool>.Id, static (w, o) => w.Write(Unsafe.Unbox<bool>(o)) },
                { Object<sbyte>.Id, static (w, o) => w.Write(Unsafe.Unbox<sbyte>(o)) },
                { Object<byte>.Id, static (w, o) => w.Write(Unsafe.Unbox<byte>(o)) },
                { Object<short>.Id, static (w, o) => w.Write(Unsafe.Unbox<short>(o)) },
                { Object<ushort>.Id, static (w, o) => w.Write(Unsafe.Unbox<ushort>(o)) },
                { Object<int>.Id, static (w, o) => w.Write(Unsafe.Unbox<int>(o)) },
                { Object<uint>.Id, static (w, o) => w.Write(Unsafe.Unbox<uint>(o)) },
                { Object<long>.Id, static (w, o) => w.Write(Unsafe.Unbox<long>(o)) },
                { Object<ulong>.Id, static (w, o) => w.Write(Unsafe.Unbox<ulong>(o)) },
                { Object<float>.Id, static (w, o) => w.Write(Unsafe.Unbox<float>(o)) },
                { Object<double>.Id, static (w, o) => w.Write(Unsafe.Unbox<double>(o)) },
                { Object<string>.Id, static (w, o) => w.Write(Unsafe.As<string>(o)) },
                { Object<Guid>.Id, static (w, o) => w.Write(Unsafe.Unbox<Guid>(o)) },
                { Object<NbtCompound>.Id, static (w, o) => w.Write(Unsafe.As<NbtCompound>(o)) }
            };
            
            s_writers = new Action<Writer, object>[dictionary.Count];
            foreach (var pair in dictionary)
                s_writers[pair.Key] = pair.Value;
        }
        
        public static bool CanWrite(int id) => s_writers.Length > id;
        public static void WriteObject(Writer writer, object @object, int id) => s_writers[id](writer, @object);
        
        public Writer(Stream output) : base(output) { }
        public Writer(Stream output, Encoding encoding, bool leaveOpen = false) : base(output, encoding, leaveOpen) { }
        
        public bool CanWriteStatic(int id) => CanWrite(id);
        
        public void WriteObject(object @object, int id) => WriteObject(this, @object, id);
        
        public override void Write(ushort value) => base.Write(BinaryPrimitives.ReverseEndianness(value));
        public void Write(Guid guid) => base.Write(guid.ToByteArray());
        public void Write(NbtCompound tag) => new NbtFile(tag).SaveToStream(BaseStream, NbtCompression.None);
    }
}