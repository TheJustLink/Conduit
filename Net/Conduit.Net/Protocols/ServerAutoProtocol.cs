using Conduit.Net.Protocols.Flow;

namespace Conduit.Net.Protocols
{
    public abstract class ServerAutoProtocol<T, TFlow> : AutoProtocol<T>
        where T : ServerAutoProtocol<T, TFlow>
        where TFlow : ProtocolFlow<TFlow>, new()
    {
        public override PacketFlow PacketFlow => ProtocolFlow<TFlow>.ServerFlow;
    }
}