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
        public int PublicKeyLength { get; set; }
        [BigData(1)]
        public byte[] PublicKey { get; set; }

        [VarInt]
        [BigDataLength(2)]
        public int VerifyTokenLength { get; set; }
        [BigData(2)]
        public byte[] VerifyToken { get; set; }

        public LoginEncryptionRequest()
        {
            Id = 0x01;
        }

        protected override void OnClear()
        {
            ServerID = null;
            PublicKeyLength = 0;
            PublicKey = null;
            VerifyTokenLength = 0;
            VerifyToken = null;
        }
    }
}
