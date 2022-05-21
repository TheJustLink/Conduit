using Conduit.Net.Connection;
using Conduit.Net.Packets;
using Conduit.Net.Reflection;

namespace Conduit.Net.Protocols
{
    public abstract class AutoProtocol<T> : Protocol
        where T : AutoProtocol<T>
    {
        protected AutoProtocol(State state, IConnection connection) : base(state, connection) { }

        public override void Handle(Packet packet) => Dispatcher<T>.Action(this as T, packet);
    }
}