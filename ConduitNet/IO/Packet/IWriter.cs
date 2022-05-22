using System;

using Conduit.Net.Data;

namespace Conduit.Net.IO.Packet
{
    public interface IWriter : IDisposable
    {
        RawPacket.IWriter Raw { get; set; }

        void ChangePacketMap(TypeMap packetMap);

        void Write(Packets.Packet packet);
    }
}