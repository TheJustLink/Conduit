using Conduit.Net.Protocols;
using Conduit.Net.Connection;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Server.Protocols
{
    public class Play : ServerAutoProtocol<Play, PlayFlow>
    {
        public Play(State state, IConnection connection) : base(state, connection) { }
    }
}