using System.IO;
using System.IO.Compression;

namespace Conduit.Net.IO.Packet
{
    public class WriterFactory : IWriterFactory
    {
        private readonly Stream _outputStream;

        public WriterFactory(Stream outputStream)
        {
            _outputStream = outputStream;
        }

        public IWriter Create()
        {
            return new Writer(_outputStream, true);
        }
        public IWriter CreateWithCompression(int treshold, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            var rawPacketWriter = new RawPacket.CompressedWriter(_outputStream, treshold, compressionLevel, true);

            return new Writer(rawPacketWriter);
        }
    }
}