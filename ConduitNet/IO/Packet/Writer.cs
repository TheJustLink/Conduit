using System.IO;
using System.Text;
using Conduit.Net.IO.Packet.Serialization;

namespace Conduit.Net.IO.Packet
{
    public class Writer : IWriter
    {
        public RawPacket.IWriter RawPacketWriter { get; set; }

        public Writer(Stream stream, bool leaveOpen = false) : this(new Binary.Writer(stream, Encoding.UTF8, leaveOpen)) { }
        public Writer(Binary.Writer binaryWriter) : this(new RawPacket.Writer(binaryWriter)) { }
        public Writer(RawPacket.IWriter rawPacketWriter)
        {
            RawPacketWriter = rawPacketWriter;
        }
        public Writer() { }

        public void Dispose()
        {
            RawPacketWriter?.Dispose();
        }

        public void Write(Packets.Packet packet)
        {
            Write(Serializer.Serialize(packet));
        }
        public void Write(Packets.RawPacket rawPacket)
        {
            RawPacketWriter.Write(rawPacket);
        }
    }
}