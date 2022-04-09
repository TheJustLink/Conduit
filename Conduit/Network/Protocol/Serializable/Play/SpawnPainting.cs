using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play
{
    public sealed class SpawnPainting : Packet
    {
        public int EntityID;
        public GuidUnsafe Guid;


        public SpawnPainting()
        {
            Id = 0x03;
        }
    }
}
