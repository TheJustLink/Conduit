using Conduit.Network.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable
{
    public class RawPacket : Packet
    {
        [BigData(0)] // connect with length of packet
        public byte[] Data;
    }
}
