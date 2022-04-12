using Conduit.Network.Protocol.StreamingData;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

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

        public bool IsConnected { get; private set; }
        public Thread DedicatedThread { get; private set; } // replace for pool implementation in future (big mistake without pool for downgrade performance)

        public VClient(GuidUnsafe id, TcpClient tcpClient, Server server)
        {
            Id = id;
            TcpClient = tcpClient;
            ServerInstance = server;
            NetworkStream = tcpClient.GetStream();
            RemoteStream = new RemoteStream(NetworkStream);
            ClientMaintainer = new ClientHandler(this);
        }

        public void Virtualize()
        {
            ThreadPool.QueueUserWorkItem(Maintenance);
            //DedicatedThread = new Thread(Maintenance);
            //DedicatedThread.Start();
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void Maintenance(object state)
        {
            IsConnected = true;
            //try
            //{
            while (IsConnected)
            {
                ClientMaintainer.MaintaintClient();
                Thread.Sleep(1);
            }
            //}
            //catch (Exception ex)
            //{
                //Console.WriteLine(ex.Message);
            //}
            //finally
            //{
                IsConnected = false;
            //}
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
