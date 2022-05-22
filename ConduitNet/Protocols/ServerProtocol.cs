using Conduit.Net.Protocols.Flow;

namespace Conduit.Net.Protocols
{
    public abstract class ServerProtocol<TFlow> : Protocol
        where TFlow : ProtocolFlow<TFlow>, new()
    {
        public override PacketFlow PacketFlow => ProtocolFlow<TFlow>.ServerFlow;
    }
}