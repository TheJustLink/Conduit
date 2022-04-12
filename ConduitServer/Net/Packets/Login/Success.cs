using System;

namespace ConduitServer.Net.Packets.Login
{
    class Success : Packet
    {
        public Guid Guid;
        public string Username;

        public Success() => Id = 2;
    }
}