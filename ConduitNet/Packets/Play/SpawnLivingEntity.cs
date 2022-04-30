using System;
using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class SpawnLivingEntity : Packet
    {
        [VarInt]
        public int EntityId;
        public Guid EntityUUID;
        [VarInt]
        public int Type;
        public double X;
        public double Y;
        public double Z;
        public byte Yaw;
        public byte Pitch;
        public short VelocityX;
        public short VelocityY;
        public short VelocityZ;

        public SpawnLivingEntity() => Id = 0x02;
    }
}
