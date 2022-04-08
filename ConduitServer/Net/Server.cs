using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConduitServer.Net
{
    internal class Server
    {
        private TcpListener _listener;
        private bool _isRunning;
        private ConcurrentDictionary<Client, Client> _clients;

        public Server(string ip, int port)
        {
            //IPAddress adress = IPAddress.Parse(_ip)
            _listener = new TcpListener(IPAddress.Any, port);
            _clients = new ConcurrentDictionary<Client, Client>();
        }

        public void Start()
        {
            _isRunning = true;

            Thread acceptThread = new Thread(AcceptLoop);
            Thread processThread = new Thread(ProcessingLoop);

            acceptThread.Start();
            processThread.Start();
        }
        public void Stop()
        {
            _isRunning = false;
        }

        private void AcceptLoop()
        {
            _listener.Start();

            while (_isRunning)
            {
                if (_listener.Pending())
                {
                    Console.WriteLine("Someone connected");

                    var tcpClient = _listener.AcceptTcpClient();
                    var client = new Client(tcpClient);

                    client.Disconnected = OnDisconnected;
                    _clients.TryAdd(client, client);
                }
                else
                    Thread.Sleep(1);
            }
        }
        private void ProcessingLoop()
        {
            while(_isRunning)
            {
                if (_clients.Count <= 0) continue;

                foreach (var client in _clients)
                {
                    client.Value.ReadPacket();
                }
            }
        }

        private void OnDisconnected(Client client)
        {
            _clients.TryRemove(client, out Client value);
        }
    }
}
