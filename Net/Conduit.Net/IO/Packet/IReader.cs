using System;

using Conduit.Net.Data;

namespace Conduit.Net.IO.Packet
{
    public interface IReader : IDisposable
    {
        RawPacket.IReader Raw { get; set; }

        void ChangePacketMap(TypeMap packetMap);

        Packets.Packet Read();
    }
}