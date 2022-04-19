using System;

namespace Conduit.Net.IO.Packet
{
    public interface IWriter : IDisposable
    {
        RawPacket.IWriter RawPacketWriter { get; set; }

        void Write(Packets.Packet packet);
        void Write(Packets.RawPacket rawPacket);
    }
}