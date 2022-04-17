using System;
using System.Threading;

using Conduit.Client.Clients;
using Conduit.Net.IO.Packet;

using RawTcpClient = System.Net.Sockets.TcpClient;

namespace Conduit.Client
{
    static class Program
    {
        private static string s_lastId = "0";

        private static void Main(string[] args)
        {
            InitializeConsole();

            //var host = "127.0.0.1";
            //var port = 666;

            //var client = CreateClient(host, port);
            //client.CheckServerState();

            for (int i = 0; i < 100; i++)
            {
                s_lastId = "0x" + i.ToString();

                var thread = new Thread(ClientThreadLoop);
                thread.IsBackground = true;
                thread.Start();

                Thread.Sleep(5000);
            }

            Console.ReadKey(true);
            Console.ReadKey(true);
            Console.ReadKey(true);
        }
        private static void InitializeConsole()
        {
            Console.Title = "Conduit Minecraft Client";
        }

        private static void ClientThreadLoop()
        {
            var host = "95.216.93.67";
            var port = 9999;

            var randomPrefixes = new string[]
            {
                "Aboba",
                "Jopa",
                "Popa",
                "Pepsi",
                "Shmepsi",
                "Navalny",
                "Bob",
                "Steve",
                "Havalnik",
                "Abobus",
                "Nachalnik",
                "Djem",
                "Smetana",
                "Pipi",
                "Popi",
                "Dragonchik",
                "Super",
                "Duper",
                "Puper",
                "Piper"
            };

            var client = CreateClient(host, port);
            client.JoinGame(randomPrefixes[Random.Shared.Next(0, randomPrefixes.Length)] + s_lastId);
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