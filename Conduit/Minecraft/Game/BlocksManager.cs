using Conduit.Minecraft.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Minecraft.Game
{
    public sealed class BlocksManager
    {
        public Dictionary<ulong, Block> Blocks { get; private set; }

        public BlocksManager()
        {

        }
    }
}
