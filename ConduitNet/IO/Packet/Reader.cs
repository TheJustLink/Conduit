using System.IO;
using System.Text;

namespace Conduit.Net.IO.Packet
{
    public class Reader : IReader
    {
        public RawPacket.IReader RawPacketReader { get; set; }

        public Reader(Stream stream, bool leaveOpen = false) : this(new Binary.Reader(stream, Encoding.UTF8, leaveOpen)) { }
        public Reader(Binary.Reader binaryReader) : this(new RawPacket.Reader(binaryReader)) { }
        public Reader(RawPacket.IReader rawPacketReader)
        {
            RawPacketReader = rawPacketReader;
        }
        public Reader() { }

        public void Dispose()
        {
            RawPacketReader?.Dispose();
        }

        public T Read<T>() where T : Packets.Packet, new()
        {
            return Read<T>(Read());
        }
        public T Read<T>(Packets.RawPacket packet) where T : Packets.Packet, new()
        {
            return Deserializer.Deserialize<T>(packet);
        }
        public Packets.RawPacket Read()
        {
            return RawPacketReader.Read();
        }
    }
}