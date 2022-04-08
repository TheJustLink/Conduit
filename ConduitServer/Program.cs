using System;

namespace ConduitServer
{
    static class Program
    {
        private static void Main(string[] args)
        {
            InitializeConsole();
            
            var server = new Server(666);
            server.Start();
        }
        private static void InitializeConsole()
        {
            Console.Title = "Conduit Minecraft Server";
        }
    }
}