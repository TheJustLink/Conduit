namespace Conduit.Net.Packets.Play.Clientbound
{
    public sealed class Disconnect : Packets.Disconnect
    {
        public Disconnect() => Id = 0x1A;
    }
}