using System;

using ConduitServer.Clients;

namespace ConduitServer.Services.Listeners
{
    interface IClientListener : IService
    {
        public event Action<IClient>? Connected;
    }
}