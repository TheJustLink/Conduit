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
        [BigDataLength(1)]
        public int PublicKeyLength;
        [BigData(1)]
        public byte[] PublicKey;

        [VarInt]
        [BigDataLength(2)]
        public int VerifyTokenLength;
        [BigData(2)]
        public byte[] VerifyToken;

        public LoginEncryptionRequest()
        {
            Id = 0x01;
        }
    }
}
