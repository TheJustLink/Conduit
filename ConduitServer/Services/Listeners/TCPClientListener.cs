using System;
using System.Net;
using System.Net.Sockets;

using ConduitServer.Clients;
using ConduitServer.Net.Packets;
using ConduitServer.Serialization.Packets;

using TcpClient = ConduitServer.Clients.TcpClient;
using RawTcpClient = System.Net.Sockets.TcpClient;

namespace ConduitServer.Services.Listeners
{
    class TcpClientListener : Service, IClientListener
    {
        public event Action<IClient>? Connected;

        private readonly TcpListener _listener;
        private readonly IPacketDeserializer _deserializer;
        private readonly IPacketSerializer _serializer;

        public TcpClientListener(int tickRate, int port, IPacketDeserializer deserializer, IPacketSerializer serializer)
            : base(tickRate)
        {
            _listener = new TcpListener(IPAddress.Any, port);

            _deserializer = deserializer;
            _serializer = serializer;
        }

        public override void Start()
        {
            _listener.Start();
            base.Start();
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
            var packetProvider = new NetworkPacketProvider(stream, _deserializer);
            var packetSender = new NetworkPacketSender(stream, _serializer);

            return new TcpClient(rawTcpClient, packetProvider, packetSender);
        }
    }
}