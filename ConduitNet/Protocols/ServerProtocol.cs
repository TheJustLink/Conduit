using Conduit.Net.Connection;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Net.Protocols
{
    public abstract class ServerProtocol<TFlow> : Protocol
        where TFlow : ProtocolFlow<TFlow>, new()
    {
        protected ServerProtocol(State state, IConnection connection) : base(state, connection) { }

        public override PacketFlow PacketFlow => ProtocolFlow<TFlow>.ServerFlow;
    }
}