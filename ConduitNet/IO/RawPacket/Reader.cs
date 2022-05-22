using System.IO;
using System.Text;
using System.Runtime.CompilerServices;

namespace Conduit.Net.IO.RawPacket
{
    public class Reader : IReader
    {
        public Binary.Reader Binary { get; set; }

        public Reader(Stream stream, bool leaveOpen = false) : this(new Binary.Reader(stream, Encoding.UTF8, leaveOpen)) { }
        public Reader(Binary.Reader binaryReader) => Binary = binaryReader;

        public void Dispose() => Binary?.Dispose();

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public Packets.RawPacket Read()
        {
            var length = Binary.Read7BitEncodedInt();
            var packet = Binary.ReadBytes(length);

            using var packetReader = new Binary.Reader(packet);
            var id = packetReader.Read7BitEncodedInt();

            var dataLength = length - (int)packetReader.BaseStream.Position;
            var data = packetReader.ReadBytes(dataLength);

            return new Packets.RawPacket { Length = length, Id = id, Data = data };
        }
    }
}