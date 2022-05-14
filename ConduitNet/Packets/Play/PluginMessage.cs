namespace Conduit.Net.Packets.Play
{
    public abstract class PluginMessage : Packet
    {
        public string Channel;
        public byte[] Data;
    }
}