using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Minecraft.Game
{
    public abstract class World
    {
        public GuidUnsafe UUID { get; private set; }

        private ObjectsManager ObjectsManager;

        public void SetUUID(GuidUnsafe guid)
        {
            UUID = guid;
        }
    }
}
