namespace Conduit.Net.Packets.Play
{
    public class KeepAliveRequest : KeepAlive
    {
        public KeepAliveRequest() => Id = 0x21;
    }
}