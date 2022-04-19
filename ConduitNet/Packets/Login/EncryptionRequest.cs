namespace Conduit.Net.Packets.Login
{
    public class EncryptionRequest : Packet
    {
        public string ServerId;
        public byte[] PublicKey;
        public byte[] VerifyToken;
        
        public EncryptionRequest() => Id = 1;
    }
}