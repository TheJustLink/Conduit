using System;
using System.IO;
using System.Text;

namespace ConduitServer.Net
{
    class BinaryWriter : System.IO.BinaryWriter
    {
        public BinaryWriter() : base() { }
        public BinaryWriter(Stream output) : base(output) { }
        public BinaryWriter(Stream output, Encoding encoding) : base(output, encoding) { }
        public BinaryWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen) { }
        
        public virtual void Write(Guid guid)
        {
            base.Write(guid.ToByteArray());
        }
    }
}