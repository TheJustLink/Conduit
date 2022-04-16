using System;
using System.IO;
using System.Text;

namespace Conduit.Net.IO.Packet
{
    public class Reader : IDisposable, IReader
    {
        private readonly RawPacket.IReader _rawPacketReader;

        public Reader(Stream stream, bool leaveOpen = false) : this(new Binary.Reader(stream, Encoding.UTF8, leaveOpen)) { }
        public Reader(Binary.Reader binaryReader) : this(new RawPacket.Reader(binaryReader)) { }
        public Reader(RawPacket.IReader rawPacketReader)
        {
            _rawPacketReader = rawPacketReader;
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
            return _rawPacketReader.Read();
        }

        public void Dispose()
        {
            _rawPacketReader?.Dispose();
        }
    }
}