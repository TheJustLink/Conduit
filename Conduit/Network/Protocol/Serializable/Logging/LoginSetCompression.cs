using Conduit.Network.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Logging
{
    public sealed class LoginSetCompression : Packet
    {
        [VarInt]
        public int Threshold { get; set; }

        protected override void OnClear()
        {
            Threshold = 0;
        }
    }
}
