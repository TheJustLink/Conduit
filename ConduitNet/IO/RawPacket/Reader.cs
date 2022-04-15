using System;
using System.IO;
using System.Text;

namespace Conduit.Net.IO.RawPacket
{
    public class Reader : IDisposable
    {
        private readonly Binary.Reader _binaryReader;

        public Reader(Stream stream, bool leaveOpen = false) : this(new Binary.Reader(stream, Encoding.UTF8, leaveOpen)) { }
        public Reader(Binary.Reader binaryReader)
        {
            _binaryReader = binaryReader;
        }

        public Packets.RawPacket Read()
        {
            var length = _binaryReader.Read7BitEncodedInt();
            var id = _binaryReader.Read7BitEncodedInt();
            var data = _binaryReader.ReadBytes(length - 1);

            return new Packets.RawPacket { Length = length, Id = id, Data = data };
        }

        public void Dispose()
        {
            _binaryReader?.Dispose();
        }
    }
}