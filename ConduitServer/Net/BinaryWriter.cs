using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
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