using System;

namespace ConduitServer.Services.Listeners
{
    class TCPClientListener : Service
    {
        public event Action<Client>? Connected;
    }
}