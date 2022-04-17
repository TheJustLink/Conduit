using System.IO;
using System.Text;

namespace Conduit.Net.IO.RawPacket
{
    public class Reader : IReader
    {
        private readonly Binary.Reader _binaryReader;

        public Reader(Stream stream, bool leaveOpen = false) : this(new Binary.Reader(stream, Encoding.UTF8, leaveOpen)) { }
        public Reader(Binary.Reader binaryReader)
        {
            _binaryReader = binaryReader;
        }

        public Packets.RawPacket Read()
        {
            var length = _binaryReader.Read7BitEncodedInt();
            var packet = _binaryReader.ReadBytes(length);

            using var packetReader = new Binary.Reader(packet);
            var id = packetReader.Read7BitEncodedInt();

            var dataLength = length - (int)packetReader.BaseStream.Position;
            var data = packetReader.ReadBytes(dataLength);

            return new Packets.RawPacket { Length = length, Id = id, Data = data };
        }

        public void Dispose()
        {
            _binaryReader?.Dispose();
        }
    }
}