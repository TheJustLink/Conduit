namespace ConduitServer.Net.Packets.Play
{
    class Disconnect : Packets.Disconnect
    {
        public Disconnect() => Id = 0x1A;
    }
}