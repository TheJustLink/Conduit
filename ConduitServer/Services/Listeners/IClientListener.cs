using System;

using Conduit.Server.Clients;

namespace Conduit.Server.Services.Listeners
{
    interface IClientListener : IService
    {
        public event Action<IClient>? Connected;
    }
}