using System;

namespace Conduit.Net.IO.RawPacket
{
    public interface IWriter : IDisposable
    {
        Binary.Writer BinaryWriter { get; set; }

        void Write(Packets.RawPacket rawPacket);
    }
}