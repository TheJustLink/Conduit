using System;

namespace ConduitServer.Services.Listeners
{
    interface IClientListener : IService
    {
        public event Action<Client>? Connected;
    }
}