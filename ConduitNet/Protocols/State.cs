using Conduit.Net.Connection;

namespace Conduit.Net.Protocols
{
    public class State
    {
        public Protocol Current { get; private set; }

        private readonly IConnection _connection;

        public State(IConnection connection, Protocol protocol)
        {
            _connection = connection;

            Switch(protocol);
        }

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
            _connection.ChangePacketFlow(protocol.PacketFlow);

            Current = protocol;
        }
    }
}