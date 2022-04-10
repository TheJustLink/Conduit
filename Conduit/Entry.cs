using Conduit.Controllable.Status;
using Conduit.Hosting;
using Conduit.Minecraft;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Conduit
{
    public sealed class Entry
    {
        public Server Server { get; private set; }

        public Entry()
        {
            ThreadPoolTool.Setup();
            Server = new Server();
            Server.ServerIntergrate.Setup(new MCServer());

            var sgen = new StatusGenerator();
            sgen.Invoke(); 
            Server.Status = sgen;
        }
    }
}
