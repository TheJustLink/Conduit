namespace Conduit.Net.Packets.Play.Serverbound
{
    public sealed class ChatMessage : Packet
    {
        public string Message;

        public ChatMessage() => Id = 3;
    }
}