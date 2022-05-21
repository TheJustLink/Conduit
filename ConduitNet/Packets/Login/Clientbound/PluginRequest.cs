using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Login.Clientbound
{
    public class PluginRequest : Packet
    {
        [VarInt] public int MessageId;

        public string Channel;
        public byte[] Data;
    }
}