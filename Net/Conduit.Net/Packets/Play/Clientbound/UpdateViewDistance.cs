using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play.Clientbound
{
    public sealed class UpdateViewDistance : Packet
    {
        /// <summary>
        /// Value between 2 and 32
        /// </summary>
        [VarInt] public int ViewDistance;
    }
}