using System;
using System.IO;
using System.Text;

namespace Conduit.Net.IO.Binary
{
    public class Writer : BinaryWriter
    {
        public Writer() : base() { }
        public Writer(Stream output) : base(output) { }
        public Writer(Stream output, Encoding encoding) : base(output, encoding) { }
        public Writer(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen) { }

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
        public virtual void Write(Guid guid)
        {
            base.Write(guid.ToByteArray());
        }
    }
}