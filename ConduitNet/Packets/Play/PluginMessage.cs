namespace Conduit.Net.Packets.Play
{
    public class PluginMessage : Packet
    {
        public string Channel;
        public byte[] Data;

        public PluginMessage() => Id = 0x18;
    }
}