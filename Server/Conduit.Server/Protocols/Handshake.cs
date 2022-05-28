using System;

using Conduit.Net.Data;
using Conduit.Net.Protocols;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Server.Protocols
{
    public class Handshake : ServerAutoProtocol<Handshake, HandshakeFlow>
    {
        public void Handle(Net.Packets.Handshake.Handshake handshake)
        {
            switch (handshake.Intention)
            {
                case ConnectIntention.Status: State.Switch<Status>(); break;
                case ConnectIntention.Login: State.Switch<Login>(); break;

                default: throw new ArgumentOutOfRangeException(nameof(handshake.Intention), handshake.Intention, "Unknown connect intention");
            }
        }
    }
}