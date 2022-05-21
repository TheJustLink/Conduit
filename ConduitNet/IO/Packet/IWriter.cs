using System;

using Conduit.Net.Data;

namespace Conduit.Net.IO.Packet
{
    public interface IWriter : IDisposable
    {
        void ChangeRawPacketWriter(RawPacket.IWriter writer);
        void ChangePacketMap(TypeMap packetMap);

        void Write(Packets.Packet packet);
    }
}