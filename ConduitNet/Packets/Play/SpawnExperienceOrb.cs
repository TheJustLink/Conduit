using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class SpawnExperienceOrb : Packet
    {
        [VarInt]
        public int EntityId;
        public double X;
        public double Y;
        public double Z;
        public short Count;

        public SpawnExperienceOrb() => Id = 0x01;
    }
}
