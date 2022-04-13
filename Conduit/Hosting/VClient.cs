using Conduit.Network.Protocol.StreamingData;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Hosting
{
    public sealed class VClient
    {
        public GuidUnsafe Id { get; private set; }
        public ClientHandler ClientMaintainer { get; private set; }

        public TcpClient TcpClient { get; private set; }
        private NetworkStream NetworkStream;
        public RemoteStream RemoteStream { get; private set; }
        public Server ServerInstance { get; private set; }
        public Stopwatch Mesure { get; private set; }
        public bool IsConnected { get; set; }
        public Thread DedicatedThread { get; private set; } // replace for pool implementation in future (big mistake without pool for downgrade performance)

        public VClient()
        {
            
        }

        public void Setup(GuidUnsafe id, TcpClient tcpClient, Server server)
        {
            Id = id;
            TcpClient = tcpClient;
            ServerInstance = server;
            NetworkStream = tcpClient.GetStream();
            RemoteStream = new RemoteStream(NetworkStream);
            ClientMaintainer = new ClientHandler(this);
            Mesure = new Stopwatch();
        }

        public void Virtualize()
        {
            //ThreadPool.QueueUserWorkItem(Maintenance);
            DedicatedThread = new Thread(Maintenance);
            DedicatedThread.Start();
        }
        public void Maintenance(object state)
        {
            IsConnected = true;
            //try
            //{
            CheckConnection();
            while (IsConnected)
            {
                ClientMaintainer.HandleClient();
                Mesure.Restart();
            }
            //}
            //catch (Exception ex)
            //{
            //Console.WriteLine(ex.Message);
            //}
            //finally
            //{
                Shutdown();
                IsConnected = false;
            //}
        }

        private async void CheckConnection()
        {
            while (IsConnected)
            {
                if (Mesure.ElapsedMilliseconds > ServerInstance.ServerOptions.NetworkOptions.TimeLimitConnection)
                {
                    IsConnected = false;
                    break;
                }
                await Task.Delay(1);
            }
        }

        public void Shutdown()
        {
            Disconnect();
            ServerInstance.ShutdownClient(Id);
        }
        public void Disconnect()
        {
            IsConnected = false;
            if (NetworkStream is not null)
            {
                NetworkStream.Close();
                NetworkStream = null;
            }
            if (TcpClient is not null)
            {
                TcpClient.Close();
                TcpClient = null;
            }
        }
    }
}
