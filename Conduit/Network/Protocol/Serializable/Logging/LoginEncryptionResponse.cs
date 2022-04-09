using Conduit.Network.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Logging
{
    public sealed class LoginEncryptionResponse : Packet
    {
        [VarInt]
        public int SharedSecretLength;
        public byte[] SharedSecret;
        [VarInt]
        public int VerifyTokenLength;
        public byte[] VerifyToken;

        public LoginEncryptionResponse()
        {
            Id = 0x01;
        }
    }
}
