using Conduit.Net.Connection;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Net.Protocols
{
    public abstract class ServerAutoProtocol<T, TFlow> : AutoProtocol<T>
        where T : ServerAutoProtocol<T, TFlow>
        where TFlow : ProtocolFlow<TFlow>, new()
    {
        protected ServerAutoProtocol(State state, IConnection connection) : base(state, connection) { }

        public override PacketFlow PacketFlow => ProtocolFlow<TFlow>.ServerFlow;
    }
}