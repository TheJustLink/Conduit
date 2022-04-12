using Conduit.Network.Serialization.Attributes;
using Conduit.Typing;
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
        public int TransactionID { get; set; }
        public Position Location { get; set; }

        public QueryBlockNBT() => Id = 0x01;

        protected override void OnClear()
        {
            TransactionID = 0;
            Location = default;
        }
    }
}
