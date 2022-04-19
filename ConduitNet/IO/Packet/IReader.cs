using System;

namespace Conduit.Net.IO.Packet
{
    public interface IReader : IDisposable
    {
        RawPacket.IReader RawPacketReader { get; set; }

        T Read<T>() where T : Packets.Packet, new();
        T Read<T>(Packets.RawPacket rawPacket) where T : Packets.Packet, new();
        Packets.RawPacket Read();
    }
}