using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Serialization
{
    class PBinaryWriter : BinaryWriter
    {
        public PBinaryWriter()
        {
        }

        public PBinaryWriter(Stream output) : base(output)
        {
        }

        public PBinaryWriter(Stream output, Encoding encoding) : base(output, encoding)
        {
        }

        public PBinaryWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
        {
        }
        public virtual void Write(Guid guid)
        {
            base.Write(guid.ToByteArray());
        }
        public virtual void WriteObject(object @object)
        {
            switch (@object)
            {
                case bool v: Write(v); return;
                case sbyte v: Write(v); return;
                case byte v: Write(v); return;
                case short v: Write(v); return;
                case ushort v: Write(v); return;
                case int v: Write(v); return;
                case long v: Write(v); return;
                case float v: Write(v); return;
                case double v: Write(v); return;
                case string v: Write(v); return;
                case Guid v: Write(v); return;
            }
        }
    }
}
