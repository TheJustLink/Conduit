using System.IO;
using System.Text;
using System.Runtime.CompilerServices;

using Conduit.Net.Data;
using Conduit.Net.IO.Packet.Serialization;

namespace Conduit.Net.IO.Packet
{
    public class Writer : IWriter
    {
        private RawPacket.IWriter _rawPacketWriter;
        private TypeMap _packetMap;

        public Writer(Stream stream, bool leaveOpen = false) : this(new Binary.Writer(stream, Encoding.UTF8, leaveOpen)) { }
        public Writer(Binary.Writer binaryWriter) : this(new RawPacket.Writer(binaryWriter)) { }
        public Writer(RawPacket.IWriter rawPacketWriter)
        {
            _rawPacketWriter = rawPacketWriter;
        }

        public void Dispose()
        {
            _rawPacketWriter?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void ChangeRawPacketWriter(RawPacket.IWriter writer) => _rawPacketWriter = writer;
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void ChangePacketMap(TypeMap packetMap) => _packetMap = packetMap;

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Write(Packets.Packet packet)
        {
            var id = _packetMap[packet.GetType()];
            var rawPacket = Serializer.Serialize(packet, id);

            _rawPacketWriter.Write(rawPacket);
        }
    }
}