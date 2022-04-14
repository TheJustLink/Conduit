using System.Net;

using Conduit.Net.IO.Packet;

using RawTcpClient = System.Net.Sockets.TcpClient;

namespace Conduit.Client.Clients
{
    class TcpClient : Client
    {
        public override string Host => ((IPEndPoint) _rawClient.Client.RemoteEndPoint).Address.ToString();
        public override int Port => ((IPEndPoint)_rawClient.Client.RemoteEndPoint).Port;
        public override bool IsConnected => _rawClient.Connected;

        private readonly RawTcpClient _rawClient;

        public TcpClient(RawTcpClient rawClient, IPacketProvider packetProvider, IPacketSender packetSender)
            : base(packetProvider, packetSender)
        {
            _rawClient = rawClient;
        }

        protected override void DisconnectInternal()
        {
            _rawClient.Close();
        }
    }
}