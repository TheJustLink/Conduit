using System;

using Conduit.Server.Services.Listeners;

namespace Conduit.Server
{
    static class Program
    {
        private static void Main(string[] args)
        {
            InitializeConsole();

            var clientListener = new TcpClientListener(0, 666);
            var server = new Server(clientListener);
            
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