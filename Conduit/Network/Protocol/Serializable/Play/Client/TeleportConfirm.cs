using Conduit.Network.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play.Client
{
    public sealed class TeleportConfirm : Packet
    {
        [VarInt]
        public int TeleportID;
    }
}
