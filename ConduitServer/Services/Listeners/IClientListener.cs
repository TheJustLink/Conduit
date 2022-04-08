using System;

namespace ConduitServer.Services.Listeners
{
    interface IClientListener
    {
        public event Action<Client>? Connected;
    }
}