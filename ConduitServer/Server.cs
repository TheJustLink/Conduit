using System;
using System.Threading;

using Conduit.Server.Clients;
using Conduit.Server.Services.Listeners;

namespace Conduit.Server
{
    class Server
    {
        private readonly IClientListener _listener;

        public Server(IClientListener listener)
        {
            _listener = listener;
            _listener.Connected += OnClientConnected;
        }

        private void OnClientConnected(IClient client)
        {
            Console.WriteLine($"{client.UserAgent} Connected");

            var thread = new Thread(client.Tick) { IsBackground = true };
            thread.Start();
        }

        public void Start() => _listener.Start();
        public void Stop() => _listener.Stop();
    }
}