using Conduit.Net.Protocols;

namespace Conduit.Net.Connection
{
    public class Remote : ITickeable
    {
        public IConnection Connection { get; }

        private readonly State _protocol;

        public Remote(IConnection connection, Protocol protocol)
        {
            Connection = connection;

            _protocol = new State(connection, protocol);
        }

        public void Tick()
        {
            if (_protocol.Current is ITickeable tickeable)
                tickeable.Tick();
            else _protocol.Current.Handle(Connection.Receive());
        }
    }
}