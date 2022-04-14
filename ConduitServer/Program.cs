using System;

using Conduit.Net.Serialization;
using Conduit.Server.Services.Listeners;

namespace Conduit.Server
{
    static class Program
    {
        private static void Main(string[] args)
        {
            InitializeConsole();

            var deserializer = new PacketDeserializer();
            var serializer = new PacketSerializer();

            var listener = new TcpClientListener(100, 666, deserializer, serializer);
            var server = new Server(listener);
            
            server.Start();
            Console.WriteLine("Started");
            Console.ReadKey(true);
            Console.WriteLine("Stop...");
            server.Stop();
        }
        private static void InitializeConsole()
        {
            Console.Title = "Conduit Minecraft Server";
        }
    }
}