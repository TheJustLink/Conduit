namespace Conduit.Net.Packets.Play
{
    public sealed class Disconnect : Packets.Disconnect
    {
        public Disconnect() => Id = 0x1A;
    }
}