using System;
using System.Threading;
using System.Net.Sockets;

using Conduit.Net.Data;
using Conduit.Net.Connection;
using Conduit.Client.Protocols;

namespace Conduit.Client
{
    static class Program
    {   
        private static void Main(string[] args)
        {
            InitializeConsole();
            
            TestLocalJoinGame(1516);

            // KachanAnnihilator(100);

            Console.ReadKey(true);
            Console.ReadKey(true);
            Console.ReadKey(true);
        }
        private static void InitializeConsole() =>
            Console.Title = "Conduit Minecraft Client";

        private static void TestLocalJoinGame(int port) =>
            CreateRemote(CreateLocalhostConnection(port), ConnectIntention.Login);
        private static IConnection CreateLocalhostConnection(int port) =>
            CreateConnection("127.0.0.1", port);
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

            var connection = CreateConnection(host, port);
            var remote = CreateRemote(connection, ConnectIntention.Login);
            var username = randomPrefixes[Random.Shared.Next(0, randomPrefixes.Length)] + Random.Shared.Next(0, 10000);
        }

        private static Remote CreateRemote(IConnection connection, ConnectIntention intention) =>
            new(connection, new Handshake(intention));
        private static IConnection CreateConnection(string host, int port = 25565) =>
            new TCPConnection(new TcpClient(host, port));
    }
}