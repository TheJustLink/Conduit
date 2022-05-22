using Conduit.Net.Protocols;

namespace Conduit.Net.Connection
{
    public class Remote : ITickeable
    {
        public static Remote CreateWith<T>(IConnection connection) where T : Protocol, new() => new(connection, new T());

        public readonly IConnection Connection;

        private readonly State _protocolState;

        public Remote(IConnection connection, Protocol protocol) =>
            (Connection, _protocolState) = (connection, new State(connection, protocol));

        public void Tick()
        {
            if (_protocolState.Current is ITickeable tickeable)
                tickeable.Tick();
            else _protocolState.Current.Handle(Connection.Receive());
        }
    }
}