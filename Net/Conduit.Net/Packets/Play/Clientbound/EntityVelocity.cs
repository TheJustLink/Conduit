using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play.Clientbound
{
    public sealed class EntityVelocity : Packet
    {
        [VarInt] public int EntityId;

        public short VelocityX;
        public short VelocityY;
        public short VelocityZ;
    }
}