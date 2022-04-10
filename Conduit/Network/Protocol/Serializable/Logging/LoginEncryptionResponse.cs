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
        [BigDataLength(1)]
        public int SharedSecretLength;
        [BigData(1)]
        public byte[] SharedSecret;
        
        [VarInt]
        [BigDataLength(2)]
        public int VerifyTokenLength;
        [BigData(2)]
        public byte[] VerifyToken;

        public LoginEncryptionResponse()
        {
            Id = 0x01;
        }
    }
}
