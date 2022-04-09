using Conduit.Network.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Logging
{
    public sealed class LoginEncryptionRequest : Packet
    {
        public string ServerID;
        [VarInt]
        public int PublicKeyLength;
        public byte[] PublicKey;
        [VarInt]
        public int VerifyTokenLength;
        public byte[] VerifyToken;

        public LoginEncryptionRequest()
        {
            Id = 0x01;
        }
    }
}
