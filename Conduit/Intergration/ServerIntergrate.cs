using Conduit.Intergration.Chat;
using Conduit.Minecraft;
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

        }

        public void Setup(MCServer server)
        {

        }
    }
}
