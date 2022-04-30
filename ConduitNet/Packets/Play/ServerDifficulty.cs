namespace Conduit.Net.Packets.Play
{
    public class ServerDifficulty : Packet
    {
        public byte Difficulty; //value between 0 and 3
        public bool IsLocked;

        public ServerDifficulty() => Id = 0x0E;
    }
}
