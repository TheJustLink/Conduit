using System;

using ConduitServer.Services.Listeners;

namespace ConduitServer
{
    static class Program
    {
        static IClientListener listener;
        private static void Main(string[] args)
        {
            InitializeConsole();

            listener.Start();
            //listener.Connected += InitializeConsole;
            
            var server = new Server(666);
            server.Start();
        }
        private static void InitializeConsole()
        {
            Console.Title = "Conduit Minecraft Server";
        }
    }
}