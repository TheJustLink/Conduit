using Conduit.Network.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable
{
    public class Packet
    {
        [BigDataLength(0)]
        [BigDataOffset(-1)]
        [VarInt]
        public int Length;
        [VarInt]
        public int Id;

        public bool IsValidLength => Length > 0;
    }
}
