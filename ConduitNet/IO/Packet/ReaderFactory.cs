using System.IO;

namespace Conduit.Net.IO.Packet
{
    public class ReaderFactory : IReaderFactory
    {
        private readonly Stream _inputStream;

        public ReaderFactory(Stream inputStream)
        {
            _inputStream = inputStream;
        }

        public IReader Create()
        {
            return new Reader(_inputStream, true);
        }
        public IReader CreateWithCompression()
        {
            var rawPacketReader = new RawPacket.CompressedReader(_inputStream, true);
            var packetReader = new Reader(rawPacketReader);

            return packetReader;
        }
    }
}