using Conduit.Net.Protocols.Flow;

namespace Conduit.Net.Protocols
{
    public abstract class ClientAutoProtocol<T, TFlow> : AutoProtocol<T>
        where T : ClientAutoProtocol<T, TFlow>
        where TFlow : ProtocolFlow<TFlow>, new()
    {
        public override PacketFlow PacketFlow => ProtocolFlow<TFlow>.ClientFlow;
    }
}