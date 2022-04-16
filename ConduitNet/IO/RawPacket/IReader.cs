using System;

namespace Conduit.Net.IO.RawPacket
{
    public interface IReader : IDisposable
    {
        public Packets.RawPacket Read();
    }
}