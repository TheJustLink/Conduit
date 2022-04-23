using System.IO;
using System.IO.Compression;
using System.Text;

using Conduit.Net.IO.RawPacket;
using Conduit.Net.IO.Encryption;

namespace Conduit.Net.IO.Packet
{
    public class WriterFactory
    {
        private readonly Stream _outputStream;

        public WriterFactory(Stream outputStream)
        {
            _outputStream = outputStream;
        }

        public void AddCompression(IWriter writer, int treshold, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            var binaryWriter = writer.RawPacketWriter.BinaryWriter;

            writer.RawPacketWriter = new CompressedWriter(binaryWriter, treshold, compressionLevel);
        }
        public void AddEncryption(IWriter writer, byte[] key)
        {
            var encryptionStream = Aes.CreateEncryptionStream(_outputStream, key);

            writer.RawPacketWriter.BinaryWriter = new Binary.Writer(encryptionStream, Encoding.UTF8, true);
        }

        public IWriter Create() => new Writer(_outputStream);
    }
}