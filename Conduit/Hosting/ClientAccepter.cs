﻿using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Hosting
{
    public sealed class ClientAccepter
    {
        public Server Server { get; private set; }
        public TcpListener TcpListener { get; private set; }

        public bool IsWorking { get; private set; } 
        private Thread NativeThread;
        private ManualResetEventSlim MRES;

        public ClientAccepter(Server s)
        {
            Server = s;
            MRES = new ManualResetEventSlim(false);
        }

        public void Start()
        {
            if (!IsWorking)
            {
                IsWorking = true;
                //ThreadPool.QueueUserWorkItem(AcceptWaiter);
                NativeThread = new Thread(AcceptWaiter);
                NativeThread.Start();
            }
        }
        public void Stop()
        {
            IsWorking = false;
        }

        private void AcceptWaiter(object obj)
        {
            TcpListener = new TcpListener(System.Net.IPAddress.Any, Server.ServerOptions.NetworkOptions.Port);
            TcpListener.Start(3);

            while (IsWorking)
            {
                Thread.Sleep(Server.ServerOptions.NetworkOptions.TimePerConnect);
                if (!TcpListener.Pending()) continue;

                GuidUnsafe guid = Guid.NewGuid();

                var cl = TcpListener.AcceptTcpClient();
                var vclient = new VClient();
                vclient.Setup(guid, cl, Server);

                Server.ClientsManager.Add(guid, vclient);

                vclient.Virtualize();
                //MRES.Reset();

                //TcpListener.BeginAcceptTcpClient(new AsyncCallback(OnAccept), null);

                //MRES.Wait(); // lock for future connection
            }
            IsWorking = false;
        }

        /*
        private void OnAccept(IAsyncResult ar)
        {
            if (!IsWorking)
                return;

            TcpClient client = TcpListener.EndAcceptTcpClient(ar);

            MRES.Set(); // unlock for next accept

            var vclient = new VClient(Guid.NewGuid(), client, Server);
            
            vclient.Virtualize();
        }
        */
    }
}
