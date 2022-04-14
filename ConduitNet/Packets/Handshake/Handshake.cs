using Conduit.Net.Data;
using Conduit.Net.Serialization.Attributes;

namespace Conduit.Net.Packets.Handshake
{
    public class Handshake : Packet
    {
        [VarInt]
        public int ProtocolVersion;
        public string ServerAddress;
        public ushort ServerPort;
        [VarInt]
        public ClientState NextState;
    }
}