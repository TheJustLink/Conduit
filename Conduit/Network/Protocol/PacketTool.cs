using Conduit.Network.Protocol.Pooling;
using Conduit.Network.Protocol.Serializable;
using Conduit.Network.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol
{
    public sealed class PacketTool<TPacket> where TPacket : Packet, new()
    {
        public PacketPool<TPacket> PacketPool { get; set; }
        public Serializator<TPacket> Serializator { get; set; }

        public PacketTool()
        {
            Serializator = new Serializator<TPacket>();
            PacketPool = new PacketPool<TPacket>();
        }
    }
}
