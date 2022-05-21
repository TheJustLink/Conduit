using Conduit.Net.Packets;
using Conduit.Net.Connection;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Net.Protocols
{
    public abstract class Protocol
    {
        public abstract PacketFlow PacketFlow { get; }

        protected readonly State State;
        protected readonly IConnection Connection;

        public Protocol(State state, IConnection connection)
        {
            State = state;
            Connection = connection;
        }

        public virtual void Handle(Packet packet) => throw new System.NotImplementedException();
    }
}