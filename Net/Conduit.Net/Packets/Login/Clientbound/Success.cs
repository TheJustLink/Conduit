using System;

namespace Conduit.Net.Packets.Login.Clientbound
{
    public class Success : Packet
    {
        public Guid Guid;
        public string Username;
    }
}