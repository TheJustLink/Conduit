using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class EntityVelocity : Packet
    {
        [VarInt]
        public int EntityId;
        public short VelocityX;
        public short VelocityY;
        public short VelocityZ;

        public EntityVelocity() => Id = 0x4F;
    }
}
