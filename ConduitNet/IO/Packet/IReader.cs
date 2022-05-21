using System;

using Conduit.Net.Data;

namespace Conduit.Net.IO.Packet
{
    public interface IReader : IDisposable
    {
        void ChangePacketMap(TypeMap packetMap);
        void ChangeRawPacketReader(RawPacket.IReader reader);

        Packets.Packet Read();
    }
}