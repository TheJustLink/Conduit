using Conduit.Configurable;
using Conduit.Controllable.Status;
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
        public IStatus Status { get; set; }
        public Server()
        {
            ClientsManager = new ClientsManager();
            ClientAccepter = new ClientAccepter(this);
            Protocol = new Protocol();
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

        public bool DisconnectManaged(ulong id)
        {
            
            throw new NotImplementedException();
            //return true;
        }
        public bool ShutdownClient(GuidUnsafe id)
        {
            if (!ClientsManager.ContainsClient(id))
                return false;

            ClientsManager.Remove(id);
            return true;
        }
        public bool DisconnectUnmanaged(GuidUnsafe id)
        {
            bool state = ClientsManager.TryGetClient(id, out VClient vClient);
            if (!state)
                return false;

            vClient.Disconnect();

            ClientsManager.Remove(id);

            return true;
        }
    }
}
