using System;
using System.Net;
using System.Net.Sockets;

using Conduit.Net.Connection;

namespace Conduit.Server.Services.Listeners
{
    class TcpConnectionListener : Service, IConnectionListener
    {
        public event Action<IConnection> Connected;

        private readonly TcpListener _listener;
        private readonly int _maxQueue;

        public TcpConnectionListener(int tickRate, int port, int maxQueue = 0)
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
            base.Stop();
            _listener.Stop();
        }

        protected override void Tick()
        {
            if (!_listener.Pending()) return;

            var tcpClient = _listener.AcceptTcpClient();
            var connection = new TCPConnection(tcpClient);
            
            Connected?.Invoke(connection);
        }
    }
}