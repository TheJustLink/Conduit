using System;
using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class SpawnPlayer : Packet
    {
        [VarInt]
        public int EntityId;
        public Guid PlayerUUID;
        public double X;
        public double Y;
        public double Z;
        public byte Yaw;
        public byte Pitch;

        public SpawnPlayer() => Id = 0x04;
    }
}
