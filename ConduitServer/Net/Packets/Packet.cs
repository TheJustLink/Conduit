using ConduitServer.Serialization.Attributes;

namespace ConduitServer.Net.Packets
{
    class Packet
    {
        [VarInt]
        public int Id;
    }
}