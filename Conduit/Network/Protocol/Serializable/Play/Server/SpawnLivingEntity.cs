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
        public int EntityID;
        public GuidUnsafe ObjectUUID;
        [VarInt]
        public int Type;
        public double X;
        public double Y;
        public double Z;
        public double Pitch;
        public double Yaw;
        public double HeadYaw;
        public short VelocityX;
        public short VelocityY;
        public short VelocityZ;

        public SpawnLivingEntity()
        {
            Id = 0x02;
        }
    }
}
