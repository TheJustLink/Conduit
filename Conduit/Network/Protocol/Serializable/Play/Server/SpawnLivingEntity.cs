using Conduit.Network.Serialization.Attributes;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play.Server
{
    public sealed class SpawnLivingEntity : Packet
    {
        [VarInt]
        public int EntityID { get; set; }
        public GuidUnsafe ObjectUUID { get; set; }
        [VarInt]
        public int Type { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float HeadYaw { get; set; }
        public short VelocityX { get; set; }
        public short VelocityY { get; set; }
        public short VelocityZ { get; set; }

        public SpawnLivingEntity()
        {
            Id = 0x02;
        }

        protected override void OnClear()
        {
            EntityID = 0;
            ObjectUUID = default;
            Type = 0;
            X = 0;
            Y = 0;
            Z = 0;
            Pitch = 0;
            Yaw = 0;
            HeadYaw = 0;
            VelocityX = 0;
            VelocityY = 0;
            VelocityZ = 0;
        }
    }
}
