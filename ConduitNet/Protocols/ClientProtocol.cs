using Conduit.Net.Connection;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Net.Protocols
{
    public abstract class ClientProtocol<TFlow> : Protocol
        where TFlow : ProtocolFlow<TFlow>, new()
    {
        protected ClientProtocol(State state, IConnection connection) : base(state, connection) { }

        public override PacketFlow PacketFlow => ProtocolFlow<TFlow>.ClientFlow;
    }
}