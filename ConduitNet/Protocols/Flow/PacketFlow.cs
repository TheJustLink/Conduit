using Conduit.Net.Data;

namespace Conduit.Net.Protocols.Flow
{
    public class PacketFlow
    {
        public TypeMap InboundMap;
        public TypeMap OutboundMap;

        public PacketFlow Reverse() => new()
        {
            InboundMap = OutboundMap,
            OutboundMap = InboundMap
        };
    }
}