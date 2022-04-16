using System;

namespace Conduit.Net.IO.RawPacket
{
    public interface IWriter : IDisposable
    {
        void Write(Packets.RawPacket rawPacket);
    }
}