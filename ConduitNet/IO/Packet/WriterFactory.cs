using Conduit.Net.IO.RawPacket;

using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

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
            var aes = Aes.Create();

            aes.Mode = CipherMode.CFB;
            aes.Padding = PaddingMode.None;

            aes.FeedbackSize = 8;
            aes.KeySize = 128;
            aes.BlockSize = 128;

            aes.Key = key;
            aes.IV = key;

            var encryptor = aes.CreateEncryptor();
            var cryptoStream = new CryptoStream(_outputStream, encryptor, CryptoStreamMode.Write);
            writer.RawPacketWriter.BinaryWriter = new Binary.Writer(cryptoStream, Encoding.UTF8, true);
        }

        public IWriter Create() => new Writer(_outputStream);
    }
}