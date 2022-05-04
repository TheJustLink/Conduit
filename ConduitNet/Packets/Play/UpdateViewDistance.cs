using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class UpdateViewDistance : Packet
    {
        [VarInt] public int ViewDistance;

        public UpdateViewDistance() => Id = 0x4A;
    }
}