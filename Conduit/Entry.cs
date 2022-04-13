using Conduit.Configurable;
using Conduit.Controllable.Status;
using Conduit.Hosting;
using Conduit.Minecraft;
using Conduit.Network.JSON.Serialization;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
            //Server.ServerIntergrate.Setup(new MCServer());
        }

        public bool Setup(string[] args)
        {
            string fpath = "defaultOptions.json";
            if (args is not null && args.Length > 0)
                fpath = args[0];

            var so = new ServerOptions();
            if (!File.Exists(fpath))
            {
                if (!File.Exists(fpath))
                {
                    File.WriteAllText(fpath, so.Serialize());
                }
            }

            if (!TryRead(fpath, out string json))
            {
                Console.WriteLine("Failed Config read on: " + fpath);
                return false;
            }

            if (!JsonObject<ServerOptions>.Deserialize(json, out so))
            {
                Console.WriteLine("Failed deserialize Config.");
                return false;
            }

            File.WriteAllText(fpath, so.Serialize());

            Server.Setup(so);
            Server.Start();

            return true;
        }

        private bool TryRead(string fpath, out string json)
        {
            try
            {
                json = File.ReadAllText(fpath);
                return true;
            }
            catch
            {
                json = null;
                return false;
            }
        }
    }
}
