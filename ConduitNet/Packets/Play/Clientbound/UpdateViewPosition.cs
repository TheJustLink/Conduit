using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play.Clientbound
{
    // SetChunkCache in Mojang edition
    public sealed class UpdateViewPosition : Packet
    {
        [VarInt] public int ChunkX;
        [VarInt] public int ChunkZ;

        public UpdateViewPosition() => Id = 0x49;
    }
}