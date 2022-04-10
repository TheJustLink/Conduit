using Conduit.Controllable.Status;
using Conduit.Hosting;
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
            var sgen = new StatusGenerator();
            Server.Status = sgen;
            sgen.Invoke();
        }
    }
}
