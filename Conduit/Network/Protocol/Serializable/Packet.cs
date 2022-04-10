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
        [BigDataLength(0)] // typically id 0
        [BigDataOffset(-1)] // because packet length include ID field of packet, -1 byte of length & we get size of packet without ID field
        [VarInt]
        public int Length;
        [VarInt]
        public int Id;

        public bool IsValidLength => Length > 0;
    }
}
