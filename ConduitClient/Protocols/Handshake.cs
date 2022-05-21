using Conduit.Net;
using Conduit.Net.Data;
using Conduit.Net.Protocols;
using Conduit.Net.Connection;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Client.Protocols
{
    public class Handshake : ClientProtocol<LoginFlow>, IStarteable
    {
        private const int ProtocolVersion = 757;

        private readonly ConnectIntention _intention;

        public Handshake(State state, IConnection connection, ConnectIntention intention) : base(state, connection) =>
            _intention = intention;
        
        public void Start()
        {
            Connection.Send(new Net.Packets.Handshake.Handshake
            {
                ProtocolVersion = ProtocolVersion,
                ServerAddress = Connection.RemotePoint.Host,
                ServerPort = Connection.LocalPoint.Port,
                Intention = _intention
            });

            State.Switch(_intention switch
            {
                ConnectIntention.Status => new Status(State, Connection),
                ConnectIntention.Login => new Login(State, Connection)
            });
        }
    }
}