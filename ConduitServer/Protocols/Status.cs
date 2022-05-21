using Conduit.Net.Connection;
using Conduit.Net.Protocols;

namespace Conduit.Server.Protocols
{
    public class Status : AutoProtocol<Status>
    {
        public Status(State state, IConnection connection) : base(state, connection) { }

        public void Handle()
        {

        }
    }
}