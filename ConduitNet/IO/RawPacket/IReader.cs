using System;

namespace Conduit.Net.IO.RawPacket
{
    public interface IReader : IDisposable
    {
        Binary.Reader BinaryReader { get; set; }

        public Packets.RawPacket Read();
    }
}