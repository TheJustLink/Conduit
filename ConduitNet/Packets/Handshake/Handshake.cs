using Conduit.Net.Attributes;
using Conduit.Net.Data;

namespace Conduit.Net.Packets.Handshake
{
    [Packet(0x00)]
    public class Handshake : Packet
    {
        [VarInt] public int ProtocolVersion;
        
        public string ServerAddress;
        public ushort ServerPort;

        [VarInt] public ClientState NextState;
    }
}