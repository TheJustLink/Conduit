using Conduit.Minecraft.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Minecraft
{
    public sealed class MCServer
    {
        public WorldManager WorldManager;
        public PlayersManager PlayersManager;
        public ResourceManager ResourceManager;

        public MCServer()
        {
            ResourceManager = new ResourceManager();
            WorldManager = new WorldManager();
            PlayersManager = new PlayersManager();
        }

        public bool SetupResourcesFromFiles(string wpath)
        {
            if (!Directory.Exists(wpath))
                return false;

            return ResourceManager.AllocateResources(wpath);
        }

        public void Start()
        {
            PlayersManager.Start();
        }
    }
}
