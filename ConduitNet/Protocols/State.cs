using Conduit.Net.Connection;

namespace Conduit.Net.Protocols
{
    public class State
    {
        public Protocol Current { get; private set; }

        private readonly IConnection _connection;

        public State(IConnection connection, Protocol protocol) : this(connection) => Switch(protocol);
        public State(IConnection connection) => _connection = connection;

        public void Switch<T>() where T : Protocol, new() => Switch(new T());
        public void Switch(Protocol protocol)
        {
            if (Current is IEndeable endeable)
                endeable.End();
            
            Change(protocol);

            if (protocol is IStarteable starteable)
                starteable.Start();
        }

        private void Change(Protocol protocol)
        {
            protocol.Initialize(this, _connection);

            _connection.ChangePacketFlow(protocol.PacketFlow);

            Current = protocol;
        }
    }
}