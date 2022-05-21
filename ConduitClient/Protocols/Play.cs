using Conduit.Net.Protocols;
using Conduit.Net.Connection;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Client.Protocols
{
    public class Play : ClientAutoProtocol<Play, PlayFlow>
    {
        public Play(State state, IConnection connection) : base(state, connection) { }
    }
}