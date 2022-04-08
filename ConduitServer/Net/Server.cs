using System;
using System.Collections.Generic;
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
        private List<Client> _clients;

        public Server(string ip, int port)
        {
            //IPAddress adress = IPAddress.Parse(_ip)
            _listener = new TcpListener(IPAddress.Any, port);
            _clients = new List<Client>();
        }

        public void Start()
        {
            _isRunning = true;

            Thread acceptThread = new Thread(AcceptLoop);

            acceptThread.Start();
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
                    var client = new Client(tcpClient.GetStream());

                    _clients.Add(client);
                }
                else
                {
                    foreach (var client in _clients)
                        client.ReadPacket();
                    Thread.Sleep(1);
                }
            }
        }
    }
}
