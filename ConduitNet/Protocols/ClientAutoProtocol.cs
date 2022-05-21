using Conduit.Net.Connection;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Net.Protocols
{
    public abstract class ClientAutoProtocol<T, TFlow> : AutoProtocol<T>
        where T : ClientAutoProtocol<T, TFlow>
        where TFlow : ProtocolFlow<TFlow>, new()
    {
        protected ClientAutoProtocol(State state, IConnection connection) : base(state, connection) { }

        public override PacketFlow PacketFlow => ProtocolFlow<TFlow>.ClientFlow;
    }
}