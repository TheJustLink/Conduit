namespace Conduit.Net.Packets.Login.Clientbound
{
    public class EncryptionRequest : Packet
    {
        public string ServerId;
        public byte[] PublicKey;
        public byte[] VerifyToken;
    }
}