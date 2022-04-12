using Conduit.Network.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play.Server
{
    public sealed class Statistic
    {
        [VarInt]
        public int CategoryID { get; set; }
        [VarInt]
        public int StatisticID { get; set; }
        [VarInt]
        public int Value { get; set; }
    }
}
