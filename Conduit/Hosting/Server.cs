using Conduit.Configurable;
using Conduit.Controllable.Status;
using Conduit.Intergration;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Conduit.Hosting
{
    public sealed class Server
    {
        public ClientsManager ClientsManager { get; private set; }
        public ServerOptions ServerOptions { get; private set; }
        public Protocol Protocol { get; private set; }
        public ClientAccepter ClientAccepter { get; private set; }
        public StatusGenerator Status { get; set; }
        public ServerIntergrate ServerIntergrate { get; private set; }
        public Server()
        {
            ClientsManager = new ClientsManager();
            ClientAccepter = new ClientAccepter(this);
            Protocol = new Protocol();
            ServerIntergrate = new ServerIntergrate();

            Status = new StatusGenerator(ServerIntergrate);
            Status.Invoke();
        }

        public void Setup(ServerOptions so)
        {
            ServerOptions = so;
        }

        public void Start()
        {
            ClientsManager.Start();
            ClientAccepter.Start();
        }
        public void Stop()
        {
            ClientAccepter.Stop();
            ClientsManager.Stop();
        }

        public bool ShutdownClient(GuidUnsafe id)
        {
            if (!ClientsManager.ContainsClient(id))
                return false;

            ClientsManager.Remove(id);
            Console.WriteLine("Clients: " + ClientsManager.Count);
            return true;
        }
        public bool DisconnectUnmanaged(GuidUnsafe id)
        {
            bool state = ClientsManager.TryGetClient(id, out VClient vClient);
            if (!state)
                return false;

            vClient.Disconnect();

            ClientsManager.Remove(id);
            Console.WriteLine("Clients: " + ClientsManager.Count);
            return true;
        }
    }
}
