using Conduit.Network.Serialization.Attributes;
using Conduit.Typing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play.Client
{
    public sealed class ClientStatus : Packet
    {
        [VarInt]
        public int Status { get; set; }
        public ActionID ActionID => (ActionID)Status;

        public ClientStatus() => Id = 0x04;

        protected override void OnClear()
        {
            Status = 0;
        }
    }
}
