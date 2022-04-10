using Conduit.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Minecraft.Game.Objects
{
    public class Player : Entity
    {
        private VClient vClient;

        public string Username;

        public Player(VClient vCl)
        {
            vClient = vCl;
        }

        public void Spawn()
        {

        }
    }
}
