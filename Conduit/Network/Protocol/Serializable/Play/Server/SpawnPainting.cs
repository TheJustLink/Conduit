using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play.Server
{
    public sealed class SpawnPainting : Packet
    {
        public int EntityID;
        public GuidUnsafe UUID;

        public SpawnPainting()
        {
            Id = 0x03;
        }

        protected override void OnClear()
        {
            EntityID = 0;
            UUID = default;
        }
    }
}
