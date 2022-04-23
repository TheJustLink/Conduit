using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Login
{
    public class PluginRequest : Packet
    {
        [VarInt]
        public int MessageId;
        public string Channel;
        public byte[] Data;

        public PluginRequest() => Id = 4;
    }
}