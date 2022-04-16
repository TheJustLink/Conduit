using System;
using System.IO;
using System.Text;

namespace Conduit.Net.IO.Packet
{
    public class Writer : IDisposable, IWriter
    {
        private readonly RawPacket.IWriter _rawPacketWriter;

        public Writer(Stream stream, bool leaveOpen = false) : this(new Binary.Writer(stream, Encoding.UTF8, leaveOpen)) { }
        public Writer(Binary.Writer binaryWriter) : this(new RawPacket.Writer(binaryWriter)) { }
        public Writer(RawPacket.IWriter rawPacketWriter)
        {
            _rawPacketWriter = rawPacketWriter;
        }

        public void Write(Packets.Packet packet)
        {
            Write(Serializer.Serialize(packet));
        }
        public void Write(Packets.RawPacket rawPacket)
        {
            _rawPacketWriter.Write(rawPacket);
        }

        public void Dispose()
        {
            _rawPacketWriter?.Dispose();
        }
    }
}