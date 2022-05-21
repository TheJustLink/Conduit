using System;

using Conduit.Net.Connection;
using Conduit.Net.Data;
using Conduit.Net.Protocols;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Server.Protocols
{
    public class Handshake : ServerAutoProtocol<Handshake, HandshakeFlow>
    {
        public Handshake(State state, IConnection connection) : base(state, connection) { }

        public void Handle(Net.Packets.Handshake.Handshake handshake)
        {
            switch (handshake.Intention)
            {
                case ConnectIntention.Status: State.Switch(new Status(State, Connection)); break;
                case ConnectIntention.Login: State.Switch(new Login(State, Connection)); break;
                default: throw new ArgumentOutOfRangeException(nameof(handshake.Intention), handshake.Intention, "Unknown connect intention");
            }
        }
    }
}