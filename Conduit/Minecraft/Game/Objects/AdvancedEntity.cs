using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Minecraft.Game.Objects
{
    public class AdvancedEntity : Entity
    {
        public Vector<double> Pitch;
        public Vector<double> Yaw;

        public bool IsAlive => Health > 0;
    }
}
