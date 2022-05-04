using System;
using System.Threading;

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
            
            TestLocalJoinGame(58542);

            // KachanAnnihilator(100);

            Console.ReadKey(true);
            Console.ReadKey(true);
            Console.ReadKey(true);
        }
        private static void InitializeConsole()
        {
            Console.Title = "Conduit Minecraft Client";
        }

        private static void TestLocalJoinGame(int port)
        {
            var client = CreateLocalhostClient(port);
            client.JoinGame("Cucumber");
        }
        private static IClient CreateLocalhostClient(int port)
        {
            return CreateClient("127.0.0.1", port);
        }
        private static void KachanAnnihilator(int countBots, int cooldown = 2000)
        {
            for (var i = 0; i < countBots; i++)
            {
                var thread = new Thread(ClientThreadLoop);
                thread.IsBackground = true;
                thread.Start();

                Thread.Sleep(cooldown);
            }
        }
        private static void ClientThreadLoop()
        {
            var host = "95.216.93.67";
            var port = 9999;

            var randomPrefixes = new[]
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
                "Piper",
                "SEPERRA",
                "Seperra",
                "KACHANOV",
                "D_U_X_A",
                "D_U_D_K_A",
                "Joper",
                "Player"
            };

            var client = CreateClient(host, port);
            client.JoinGame(randomPrefixes[Random.Shared.Next(0, randomPrefixes.Length)] + Random.Shared.Next(0, 10000));
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