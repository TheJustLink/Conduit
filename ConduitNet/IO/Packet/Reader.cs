using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using Conduit.Net.Data;
using Conduit.Net.IO.Packet.Serialization;

namespace Conduit.Net.IO.Packet
{
    public class Reader : IReader
    {
        private RawPacket.IReader _rawPacketReader;
        private TypeMap _packetMap;

        public Reader(Stream stream, bool leaveOpen = false) : this(new Binary.Reader(stream, Encoding.UTF8, leaveOpen)) { }
        public Reader(Binary.Reader binaryReader) : this(new RawPacket.Reader(binaryReader)) { }
        public Reader(RawPacket.IReader rawPacketReader) => _rawPacketReader = rawPacketReader;

        public void Dispose() => _rawPacketReader?.Dispose();

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void ChangePacketMap(TypeMap packetMap) => _packetMap = packetMap;
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void ChangeRawPacketReader(RawPacket.IReader reader) => _rawPacketReader = reader;
        
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public Packets.Packet Read()
        {
            var rawPacket = _rawPacketReader.Read();
            var packetType = _packetMap[rawPacket.Id];

            return Deserializer.Deserialize(rawPacket, packetType);
        }
    }
}