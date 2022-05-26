using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using Conduit.Net.Data;
using Conduit.Net.IO.Packet.Serialization;

namespace Conduit.Net.IO.Packet
{
    public class Reader : IReader
    {
        public RawPacket.IReader Raw { get; set; }

        private TypeMap _packetMap;

        public Reader(Stream stream, bool leaveOpen = false) : this(new Binary.Reader(stream, Encoding.UTF8, leaveOpen)) { }
        public Reader(Binary.Reader binaryReader) : this(new RawPacket.Reader(binaryReader)) { }
        public Reader(RawPacket.IReader raw) => Raw = raw;

        public void Dispose() => Raw?.Dispose();

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void ChangePacketMap(TypeMap packetMap) => _packetMap = packetMap;
        
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public Packets.Packet Read()
        {
            while (true)
            {
                var rawPacket = Raw.Read();

                if (_packetMap.Has(rawPacket.Id))
                {
                    var packetType = _packetMap[rawPacket.Id];

                    return Deserializer.Deserialize(rawPacket, packetType);
                }

                Console.WriteLine($"Skip packet with id={rawPacket.Id}({rawPacket.Data.Length} bytes), packet type not found");
            }
        }
    }
}