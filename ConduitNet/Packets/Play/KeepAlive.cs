namespace Conduit.Net.Packets.Play
{
    public class KeepAlive : Packet
    {
        public long Payload;
        
        public KeepAlive() => Id = 0x21;

        public KeepAlive ToResponse()
        {
            Id = 0x0F;

            return this;
        }
    }
}