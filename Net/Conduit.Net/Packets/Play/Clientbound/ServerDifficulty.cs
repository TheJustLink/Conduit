namespace Conduit.Net.Packets.Play.Clientbound
{
    public sealed class ServerDifficulty : Packet
    {
        public byte Difficulty;
        public bool IsLocked;
    }
}