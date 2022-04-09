using Conduit.Network.Serialization.Attributes;
using Conduit.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play.Client
{
    public sealed class QueryBlockNBT : Packet
    {
        [VarInt]
        public int TransactionID;
        public Position Location;

        public QueryBlockNBT() => Id = 0x01;
    }
}
