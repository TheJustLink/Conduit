namespace Conduit.Net.Packets.Play
{
    public class KeepAliveResponse : KeepAlive
    {
        public KeepAliveResponse() => Id = 0x0F;
    }
}