﻿namespace Conduit.Net.Packets.Play
{
    public class ChatMessage : Packet
    {
        public string Message;

        public ChatMessage() => Id = 3;
    }
}