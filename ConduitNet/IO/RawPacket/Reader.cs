using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Conduit.Net.IO.RawPacket
{
    public class Reader : IReader
    {
        public Binary.Reader BinaryReader { get; set; }

        public Reader(Stream stream, bool leaveOpen = false) : this(new Binary.Reader(stream, Encoding.UTF8, leaveOpen)) { }
        public Reader(Binary.Reader binaryReader)
        {
            BinaryReader = binaryReader;
        }
        public Reader() { }

        public void Dispose()
        {
            BinaryReader?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public Packets.RawPacket Read()
        {
            var length = BinaryReader.Read7BitEncodedInt();
            var packet = BinaryReader.ReadBytes(length);

            using var packetReader = new Binary.Reader(packet);
            var id = packetReader.Read7BitEncodedInt();

            var dataLength = length - (int)packetReader.BaseStream.Position;
            var data = packetReader.ReadBytes(dataLength);

            return new Packets.RawPacket { Length = length, Id = id, Data = data };
        }
    }
}