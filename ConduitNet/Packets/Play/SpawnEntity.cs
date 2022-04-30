using System;
using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class SpawnEntity : Packet
    {
        [VarInt]
        public int EntityId;
        public Guid ObjectUUID;
        [VarInt]
        public int Type;
        public double X;
        public double Y;
        public double Z;
        public byte Pitch;
        public byte Yaw;
        public int Data;
        public short VelocityX;
        public short VelocityY;
        public short VelocityZ;

        public SpawnEntity() => Id = 0x00;
    }
}
