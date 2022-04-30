using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class EntityAnimation : Packet
    {
        [VarInt]
        public int EntityId;
        public byte Animation;

        public EntityAnimation() => Id = 0x06;
    }
}
