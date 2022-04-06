using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ConduitServer
{
    internal class Server : ILog
    {
        private readonly short _port;
        private readonly Queue<string> _messages;

        internal Server(short port)
        {
            _port = port;
            _messages = new Queue<string>();
        }

        public void Start()
        {
            _messages.Enqueue("Hello, World! From conduit");

            var tcpListener = new TcpListener(IPAddress.Any, _port);
            tcpListener.Start();

            var client = tcpListener.AcceptTcpClient();
            _messages.Enqueue("Connected " + client.Client);

            var clientStream = client.GetStream();
            var buffer = new byte[client.Available];

            clientStream.Read(buffer);
            Console.WriteLine(buffer);
        }

        public bool HasMessages()
        {
            return _messages.Count > 0;
        }

        public string[] Messages()
        {
            var messages = _messages.ToArray();
            _messages.Clear();
            return messages;
        }
    }
}