using System;
using ConduitServer.Net;

namespace ConduitServer
{
    static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Server started");

            Server server = new Server(666);
            server.Start();
        }
    }
}