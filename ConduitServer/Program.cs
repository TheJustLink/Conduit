using System;

using Conduit.Server.Services.Listeners;

namespace Conduit.Server
{
    static class Program
    {
        private static void Main(string[] args)
        {
            InitializeConsole();

            var listener = new TcpClientListener(100, 666);
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