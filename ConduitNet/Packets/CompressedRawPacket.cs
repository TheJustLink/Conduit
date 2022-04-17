using Conduit.Net.Attributes;

namespace Conduit.Net.Packets
{
    public class CompressedRawPacket
    {
        [VarInt]
        public int Length;
        [VarInt]
        public int UncompressedLength;
        public byte[] CompressedData;
    }
}