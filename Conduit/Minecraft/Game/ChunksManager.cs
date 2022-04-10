using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Minecraft.Game
{
    public sealed class ChunksManager
    {
        public BlocksManager BlocksManager { get; private set; }

        public ChunksManager()
        {
            BlocksManager = new BlocksManager();
        }
    }
}
