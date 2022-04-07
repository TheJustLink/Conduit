using ConduitServer.Serialization.Attributes;

namespace ConduitServer.Net.Packets.Handshake
{
    class Handshake : Packet
    {
        [VarInt]
        public int ProtocolVersion;
        public string ServerAddress;
        public ushort ServerPort;
        [VarInt]
        public int NextState;
    }
}