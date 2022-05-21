using Conduit.Net.Data;
using Conduit.Net.Packets.Status;

namespace Conduit.Net.Protocols.Flow
{
    public class StatusFlow : ProtocolFlow<StatusFlow>
    {
        private static readonly TypeMap s_clientboundMap = new(
            typeof(Response),
            typeof(Ping)
        );
        private static readonly TypeMap s_serverboundMap = new(
            typeof(Request),
            typeof(Ping)
        );

        public StatusFlow() : base(s_clientboundMap, s_serverboundMap) { }
    }
}