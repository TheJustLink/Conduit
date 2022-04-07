using System;

namespace ConduitServer.Net.Packets.Login
{
    class LoginSuccess : Packet
    {
        public Guid Guid;
        public string Username;

        public LoginSuccess() => Id = 2;
    }
}