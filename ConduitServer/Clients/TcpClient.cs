using Conduit.Net.IO.Packet;
using Conduit.Net.Packets;

namespace Conduit.Server.Clients
{
    class TcpClient : Client
    {
        private readonly System.Net.Sockets.TcpClient _client;

        public TcpClient(System.Net.Sockets.TcpClient client, IPacketProvider packetProvider, IPacketSender packetSender)
            : base(packetProvider, packetSender)
        {
            _client = client;
        }
        
        protected override void Disconnect()
        {
            _client.Close();
        }
    }
}