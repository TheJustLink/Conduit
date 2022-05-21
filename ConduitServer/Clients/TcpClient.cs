using Conduit.Net.Extensions;
using Conduit.Net.IO.Packet;

using RawTcpClient = System.Net.Sockets.TcpClient;

namespace Conduit.Server.Clients
{
    class TcpClient : Client
    {
        private readonly RawTcpClient _client;
        private readonly string _internalUserAgent;

        public TcpClient(RawTcpClient client, ReaderFactory packetReaderFactory, WriterFactory packetWriterFactory)
            : base(packetReaderFactory, packetWriterFactory)
        {
            _client = client;
            _internalUserAgent = _client.GetFormatRemoteEndPoint();
        }

        public override bool Connected => _client.IsConnected();
        protected override string GetInternalUserAgent() => _internalUserAgent;

        protected override void Disconnect()
        {
            _client.Close();
        }
    }
}