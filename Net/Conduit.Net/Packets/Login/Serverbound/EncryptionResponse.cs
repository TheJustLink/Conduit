namespace Conduit.Net.Packets.Login.Serverbound
{
    public class EncryptionResponse : Packet
    {
        public byte[] SharedSecret;
        public byte[] VerifyToken;
    }
}