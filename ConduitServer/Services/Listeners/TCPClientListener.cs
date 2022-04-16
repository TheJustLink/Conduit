using System;
using System.Net;
using System.Net.Sockets;

using Conduit.Server.Clients;

using TcpClient = Conduit.Server.Clients.TcpClient;
using RawTcpClient = System.Net.Sockets.TcpClient;

namespace Conduit.Server.Services.Listeners
{
    class TcpClientListener : Service, IClientListener
    {
        public event Action<IClient>? Connected;

        private readonly TcpListener _listener;
        private readonly int _maxQueue;

        public TcpClientListener(int tickRate, int port, int maxQueue = 0)
            : base(tickRate)
        {
            _maxQueue = maxQueue;
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public override void Start()
        {
            if (_maxQueue > 0)
                _listener.Start(_maxQueue);
            else _listener.Start();

            base.Start();
        }
        public override void Stop()
        {
            _listener.Stop();
            base.Stop();
        }

        protected override void Tick()
        {
            if (!_listener.Pending()) return;

            var rawTcpClient = _listener.AcceptTcpClient();
            var client = CreateClient(rawTcpClient);
            
            Connected?.Invoke(client);
        }

        private IClient CreateClient(RawTcpClient rawTcpClient)
        {
            var stream = rawTcpClient.GetStream();

            var packetReaderFactory = new Net.IO.Packet.ReaderFactory(stream);
            var packetWriterFactory = new Net.IO.Packet.WriterFactory(stream);

            return new TcpClient(rawTcpClient, packetReaderFactory, packetWriterFactory);
        }
    }
}