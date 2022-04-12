using Conduit.Network.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play.Server
{
    public sealed class SpawnXPOrb : Packet
    {
        [VarInt]
        public int EntityID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public short Count { get; set; }
        public SpawnXPOrb()
        {
            Id = 0x01;
        }

        protected override void OnClear()
        {
            EntityID = 0;
            X = 0;
            Y = 0;
            Z = 0;
            Count = 0;
        }
    }
}
