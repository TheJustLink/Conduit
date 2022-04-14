using System;

namespace Conduit.Net.Packets.Login
{
    public class Success : Packet
    {
        public Guid Guid;
        public string Username;

        public Success() => Id = 2;
    }
}