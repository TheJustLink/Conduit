using Conduit.Intergration.Chat;
using Conduit.Minecraft;
using Conduit.Network.JSON.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Intergration
{
    public sealed class ServerIntergrate
    {
        public ChatIntegrate ChatIntegrate;

        public MCServer MCServer { get; private set; }

        public bool IsPlayable { get; private set; }

        public ServerIntergrate()
        {
            ChatIntegrate = new ChatIntegrate();
        }

        public bool HandleState(out string message)
        {
            if (IsPlayable)
            {
                message = null;
                return true;
            }
            else
            {
                message = ChatIntegrate.Messages.ServerNotAvailable.LastJson;
                return false;
            }
        }

        public void Setup(MCServer server)
        {
            if (server is not null)
            {
                MCServer = server;
                IsPlayable = true;
            }
        }
    }
}
