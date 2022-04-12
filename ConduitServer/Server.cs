using System.Collections.Generic;
using System.Threading;

using ConduitServer.Clients;
using ConduitServer.Services.Listeners;

namespace ConduitServer
{
    class Server
    {
        private bool _isRunning;

        private readonly List<IClient> _clients;
        private readonly IClientListener _listener;

        public Server(IClientListener listener)
        {
            _clients = new List<IClient>();

            _listener = listener;
            _listener.Connected += OnClientConnected;
        }

        private void OnClientConnected(IClient client)
        {
            // _clients.Add(client);

            var thread = new Thread(client.Tick);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Start()
        {
            _isRunning = true;

            _listener.Start();
        }
        public void Stop()
        {
            _isRunning = false;

            _listener.Stop();
        }
    }
}