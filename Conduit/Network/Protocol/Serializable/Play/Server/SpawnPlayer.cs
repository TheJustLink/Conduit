using Conduit.Network.Serialization.Attributes;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play.Server
{
    public sealed class SpawnPlayer : Packet
    {
        [VarInt]
        public int EntityID { get; set; }
        public GuidUnsafe UUID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public SpawnPlayer() => Id = 0x04;

        protected override void OnClear()
        {
            EntityID = 0;
            UUID = default;
            X = 0;
            Y = 0;
            Z = 0;
            Yaw = 0;
            Pitch = 0;
        }
    }
}
