using Conduit.Net.Protocols.Flow;

namespace Conduit.Net.Protocols
{
    public abstract class ClientProtocol<TFlow> : Protocol
        where TFlow : ProtocolFlow<TFlow>, new()
    {
        public override PacketFlow PacketFlow => ProtocolFlow<TFlow>.ClientFlow;
    }
}