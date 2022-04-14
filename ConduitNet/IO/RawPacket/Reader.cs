using System;

namespace Conduit.Net.IO.RawPacket
{
    public class Reader : IDisposable
    {
        private readonly Binary.Reader _binaryReader;

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