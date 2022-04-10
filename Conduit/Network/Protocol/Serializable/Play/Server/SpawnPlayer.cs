using Conduit.Network.Serialization.Attributes;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play.Server
{
    public sealed class SpawnPlayer : Packet
    {
        [VarInt]
        public int EntityID;
        public GuidUnsafe UUID;
        public double X;
        public double Y;
        public double Z;
        public double Yaw;
        public double Pitch;

        public SpawnPlayer() => Id = 0x04;
    }
}
