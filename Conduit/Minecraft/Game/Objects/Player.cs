using Conduit.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Minecraft.Game.Objects
{
    public class Player : AdvancedEntity
    {
        private VClient vClient;

        public string Username;
        public ulong Gamemode; // 0: survival | minecraft reserved
                               // 1: creative
                               // 2: adventure
                               // 3: spectator
        

        public Player(VClient vCl)
        {
            vClient = vCl;
        }

        public void Spawn()
        {

        }
    }
}
