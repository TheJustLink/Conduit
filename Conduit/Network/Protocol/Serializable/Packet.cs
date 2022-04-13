using Conduit.Network.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable
{
    public abstract class Packet
    {
        [BigDataLength(0)] // typically id 0
        [BigDataOffset(-1)] // because packet length include ID field of packet, -1 byte of length & we get size of packet without ID field
        [VarInt]
        public int Length { get; set; }
        [VarInt]
        public int Id { get; set; }

        [Ignore]
        public bool IsValidLength => Length > 0;

        public void Clear()
        {
            Length = 0;
            OnClear();
        }

        protected abstract void OnClear();
    }
}
