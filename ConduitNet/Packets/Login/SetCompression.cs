using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Login
{
    public class SetCompression : Packet
    {
        [VarInt] public int Treshold;

        public SetCompression() => Id = 3;
    }
}