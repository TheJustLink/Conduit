using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class UpdateViewDistance : Packet
    {
        [VarInt]
        public int ViewDistance; //value between 2 and 32

        public UpdateViewDistance() => Id = 0x4A;
    }
}
