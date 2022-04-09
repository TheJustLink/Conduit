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
                NativeThread = new Thread(AcceptWaiter);
                NativeThread.Start();
            }
        }
        public void Stop()
        {
            IsWorking = false;
        }

        private void AcceptWaiter()
        {
            TcpListener = new TcpListener(System.Net.IPAddress.Any, Server.ServerOptions.Port);
            TcpListener.Start();

            while (IsWorking)
            {
                MRES.Reset();

                TcpListener.BeginAcceptTcpClient(new AsyncCallback(OnAccept), null);

                MRES.Wait(); // lock for future connection
            }
            IsWorking = false;
        }

        private void OnAccept(IAsyncResult ar)
        {
            if (!IsWorking)
                return;

            TcpClient client = TcpListener.EndAcceptTcpClient(ar);

            MRES.Set(); // unlock for next accept

            var vclient = new VClient(Guid.NewGuid(), client, Server);
            
            vclient.Virtualize();
        }
    }
}
