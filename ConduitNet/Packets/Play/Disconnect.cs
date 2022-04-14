namespace Conduit.Net.Packets.Play
{
    public class Disconnect : Packets.Disconnect
    {
        public Disconnect() => Id = 0x1A;
    }
}