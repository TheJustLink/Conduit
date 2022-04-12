namespace ConduitServer.Net.Packets.Disconnect
{
    class LoginDisconnect : Packet
    {
        public string Reason;

        public LoginDisconnect() => Id = 0;
    }
}