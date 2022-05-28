using Conduit.Net.Data;

namespace Conduit.Net.Protocols.Flow
{
    public abstract class ProtocolFlow<T>
        where T : ProtocolFlow<T>, new()
    {
        private static readonly T s_instance = new();

        public static readonly PacketFlow ServerFlow = s_instance._serverFlow;
        public static readonly PacketFlow ClientFlow = s_instance._clientFlow;

        private readonly PacketFlow _serverFlow;
        private readonly PacketFlow _clientFlow;

        protected ProtocolFlow(TypeMap clientboundMap, TypeMap serverboundMap)
        {
            _serverFlow = new PacketFlow
            {
                InboundMap = serverboundMap,
                OutboundMap = clientboundMap
            };
            _clientFlow = _serverFlow.Reverse();
        }
    }
}