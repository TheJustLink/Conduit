using Conduit.Net.Attributes;

namespace Conduit.Net.Packets
{
    public class RawPacket
    {
        [VarInt] public int Length;
        [VarInt] public int Id;

        public byte[] Data;
    }
}