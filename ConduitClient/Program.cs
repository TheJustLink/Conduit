using System;

using Conduit.Client.Clients;
using Conduit.Net.IO.Packet;

using RawTcpClient = System.Net.Sockets.TcpClient;

namespace Conduit.Client
{
    static class Program
    {
        private static void Main(string[] args)
        {
            InitializeConsole();

            //var host = "95.216.93.67";
            //var port = 9999;

            var host = "127.0.0.1";
            var port = 666;

            var client = CreateClient(host, port);
            client.CheckServerState();

            client = CreateClient(host, port);
            client.JoinGame("Popa");
        }
        private static void InitializeConsole()
        {
            Console.Title = "Conduit Minecraft Client";
        }

        private static IClient CreateClient(string host, int port = 25565)
        {
            var rawClient = new RawTcpClient(host, port);
            var stream = rawClient.GetStream();

            var packetReader = new ReaderFactory(stream);
            var packetWriter = new WriterFactory(stream);

            return new TcpClient(rawClient, packetReader, packetWriter);
        }
    }
}