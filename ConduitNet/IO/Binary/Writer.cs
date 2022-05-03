using System;
using System.IO;
using System.Text;

using fNbt.Tags;
using fNbt;

using Conduit.Net.Data;

namespace Conduit.Net.IO.Binary
{
    public class Writer : BinaryWriter
    {
        public Writer() : this(new MemoryStream(), Encoding.UTF8, false) { }
        public Writer(Stream output) : base(output) { }
        public Writer(Stream output, Encoding encoding) : base(output, encoding) { }
        public Writer(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen) { }

        public virtual void Write(Guid guid)
        {
            base.Write(guid.ToByteArray());
        }
        public virtual void Write(VarInt varInt)
        {
            base.Write7BitEncodedInt(varInt);
        }
        public virtual void Write(VarLong varLong)
        {
            base.Write7BitEncodedInt64(varLong);
        }
        public virtual void Write(NbtCompound tag)
        {
            var nbtFile = new NbtFile(tag);
            nbtFile.SaveToStream(BaseStream, NbtCompression.None);
        }
    }
}