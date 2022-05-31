using System;
using System.Threading;

using Conduit.Net.Connection;
using Conduit.Server.Protocols;
using Conduit.Server.Services.Listeners;

namespace Conduit.Server
{
    class Server
    {
        private readonly IConnectionListener _listener;

        public Server(IConnectionListener listener)
        {
            _listener = listener;
            _listener.Connected += OnConnection;
        }

        private void OnConnection(IConnection connection)
        {
            // Console.WriteLine($"{connection.RemotePoint} Connected");

            var remote = Remote.CreateWith<Handshake>(connection);
            var thread = new Thread(remote => ClientLoop((Remote)remote)) { IsBackground = true };
            
            thread.Start(remote);
        }
        private void ClientLoop(Remote remote)
        {
            while (remote.Connection.Connected)
            {
                remote.Tick();

                //if (remote.Connection.HasData == false)
                //    Thread.Sleep(1);
            }

            Console.WriteLine($"Client {remote.Connection.RemotePoint} disconnected");
        }

        public void Start() => _listener.Start();
        public void Stop() => _listener.Stop();
    }
}