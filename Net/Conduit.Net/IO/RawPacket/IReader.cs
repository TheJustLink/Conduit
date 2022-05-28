using System;

namespace Conduit.Net.IO.RawPacket
{
    public interface IReader : IDisposable
    {
        Binary.Reader Binary { get; set; }
        
        public Packets.RawPacket Read();
    }
}