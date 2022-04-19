namespace Conduit.Net.Packets.Login
{
    public class EncryptionResponse : Packet
    {
        public byte[] SharedSecret;
        public byte[] VerifyToken;

        public EncryptionResponse() => Id = 1;
    }
}