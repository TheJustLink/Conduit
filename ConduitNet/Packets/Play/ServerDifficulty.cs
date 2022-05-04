namespace Conduit.Net.Packets.Play
{
    public class ServerDifficulty : Packet
    {
        public byte Difficulty;
        public bool IsLocked;

        public ServerDifficulty() => Id = 0x0E;
    }
}