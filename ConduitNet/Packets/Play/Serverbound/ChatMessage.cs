namespace Conduit.Net.Packets.Play
{
    public sealed class ChatMessage : Packet
    {
        public string Message;

        public ChatMessage() => Id = 3;
    }
}