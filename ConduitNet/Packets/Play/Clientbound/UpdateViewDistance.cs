using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public sealed class UpdateViewDistance : Packet
    {
        /// <summary>
        /// Value between 2 and 32
        /// </summary>
        [VarInt] public int ViewDistance;

        public UpdateViewDistance() => Id = 0x4A;
    }
}