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
        public int SharedSecretLength { get; set; }
        [BigData(1)]
        public byte[] SharedSecret { get; set; }

        [VarInt]
        [BigDataLength(2)]
        public int VerifyTokenLength { get; set; }
        [BigData(2)]
        public byte[] VerifyToken { get; set; }

        public LoginEncryptionResponse()
        {
            Id = 0x01;
        }

        protected override void OnClear()
        {
            SharedSecretLength = 0;
            SharedSecret = null;
            VerifyTokenLength = 0;
            VerifyToken = null;
        }
    }
}
