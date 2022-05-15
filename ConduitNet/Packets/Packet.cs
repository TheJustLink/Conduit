using Conduit.Net.Attributes;

namespace Conduit.Net.Packets
{
    public class Packet
    {
        [VarInt] public int Length;
        [VarInt] public byte Id;
    }
}