using Conduit.Minecraft.Resources;
using System;
using System.Collections.Generic;
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

        public void Start()
        {
            PlayersManager.Start();
        }
    }
}
