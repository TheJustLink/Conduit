using RawTcpClient = System.Net.Sockets.TcpClient;

using IPacketReader = Conduit.Net.IO.Packet.IReader;
using IPacketWriter = Conduit.Net.IO.Packet.IWriter;

namespace Conduit.Server.Clients
{
    class TcpClient : Client
    {
        private readonly RawTcpClient _client;

        public TcpClient(RawTcpClient client, IPacketReader packetReader, IPacketWriter packetWriter)
            : base(packetReader, packetWriter)
        {
            _client = client;
        }
        
        protected override void Disconnect()
        {
            _client.Close();
        }
    }
}