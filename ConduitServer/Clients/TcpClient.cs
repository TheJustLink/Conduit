using Conduit.Net.IO.Packet;

using RawTcpClient = System.Net.Sockets.TcpClient;

namespace Conduit.Server.Clients
{
    class TcpClient : Client
    {
        private readonly RawTcpClient _client;

        public TcpClient(RawTcpClient client, IReaderFactory packetReaderFactory, IWriterFactory packetWriterFactory)
            : base(packetReaderFactory, packetWriterFactory)
        {
            _client = client;
        }
        
        protected override void Disconnect()
        {
            _client.Close();
        }
    }
}