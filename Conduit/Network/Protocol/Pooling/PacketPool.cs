using Conduit.Network.Protocol.Serializable;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Pooling
{
    public sealed class PacketPool<TPacket> where TPacket : Packet, new()
    {
        private readonly ConcurrentBag<TPacket> _objects;

        public PacketPool()
        {
            _objects = new ConcurrentBag<TPacket>();
        }

        public TPacket Get() => _objects.TryTake(out TPacket item) ? item : new TPacket();

        public void Return(TPacket item) 
        {
            item.Clear();
            _objects.Add(item);
        }
    }
}
