using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Minecraft.Game.Objects
{
    public class UniqueObject : Object
    {
        public GuidUnsafe UUID;
        public Vector<double> Rotation;
    }
}
