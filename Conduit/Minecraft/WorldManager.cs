using Conduit.Minecraft.Game;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Minecraft
{
    public sealed class WorldManager
    {
        private Dictionary<GuidUnsafe, World> _Worlds;

        public WorldManager()
        {
            _Worlds = new Dictionary<GuidUnsafe, World>();
        }

        public void AllocateWorld(World world)
        {
            GuidUnsafe guid = Guid.NewGuid();
            world.SetUUID(guid);
            _Worlds.Add(guid, world);
        }
    }
}
