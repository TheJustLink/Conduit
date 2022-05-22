using System;

using Conduit.Net.Connection;

namespace Conduit.Server.Services.Listeners
{
    interface IConnectionListener : IService
    {
        public event Action<IConnection> Connected;
    }
}