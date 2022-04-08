using System;
using System.Net;
using System.Net.Sockets;

namespace ConduitServer.Services.Listeners
{
    class TCPClientListener : Service, IClientListener
    {
        public event Action<Client>? Connected;

        private TcpListener _listener;

        public TCPClientListener(int tickRate, int listenPort):base(tickRate)
        {
            _listener = new TcpListener(IPAddress.Any, listenPort);
        }

        public override void Start()
        {
            _listener.Start();
            base.Start();
        }
        protected override void Tick()
        {
            if (_listener.Pending())
            {
                var tcpClient = _listener.AcceptTcpClient();
                var client = new Client(tcpClient);
                Connected?.Invoke(client);
            }
        }
    }
}