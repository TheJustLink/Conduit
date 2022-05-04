using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Login
{
    public class PluginResponse : Packet
    {
        [VarInt] public int MessageId;

        public bool Successful;
        public byte[] Data;

        public PluginResponse() => Id = 2;
    }
}