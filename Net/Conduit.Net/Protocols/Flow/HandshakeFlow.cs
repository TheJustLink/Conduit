using Conduit.Net.Data;
using Conduit.Net.Packets.Handshake;

namespace Conduit.Net.Protocols.Flow
{
    public class HandshakeFlow : ProtocolFlow<HandshakeFlow>
    {
        private static readonly TypeMap s_clientboundMap = new();
        private static readonly TypeMap s_serverboundMap = new(
            typeof(Handshake)
        );

        public HandshakeFlow() : base(s_clientboundMap, s_serverboundMap) { }
    }
}