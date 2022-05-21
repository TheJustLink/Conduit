using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Login.Clientbound
{
    public class SetCompression : Packet
    {
        [VarInt] public int Treshold;
    }
}