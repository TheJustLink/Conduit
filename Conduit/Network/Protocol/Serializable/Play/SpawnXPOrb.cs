using Conduit.Network.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play
{
    public sealed class SpawnXPOrb : Packet
    {
        [VarInt]
        public int EntityID;
        public double X;
        public double Y;
        public double Z;
        public short Count;
        public SpawnXPOrb()
        {
            Id = 0x01;
        }
    }
}
