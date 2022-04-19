using System.IO;
using System.Security.Cryptography;
using System.Text;

using Conduit.Net.IO.RawPacket;

namespace Conduit.Net.IO.Packet
{
    public class ReaderFactory
    {
        private readonly Stream _inputStream;

        public ReaderFactory(Stream inputStream)
        {
            _inputStream = inputStream;
        }

        public void AddCompression(IReader reader)
        {
            var binaryReader = reader.RawPacketReader.BinaryReader;
            reader.RawPacketReader = new CompressedReader(binaryReader);
        }
        public void AddEncryption(IReader reader, byte[] key)
        {
            var aes = Aes.Create();

            aes.Mode = CipherMode.CFB;
            aes.Padding = PaddingMode.None;
            
            aes.FeedbackSize = 8;
            aes.KeySize = 128;
            aes.BlockSize = 128;

            aes.Key = key;
            aes.IV = key;
            
            var decryptor = aes.CreateDecryptor();
            var cryptoStream = new CryptoStream(_inputStream, decryptor, CryptoStreamMode.Read);
            reader.RawPacketReader.BinaryReader = new Binary.Reader(cryptoStream, Encoding.UTF8, true);
        }

        public IReader Create() => new Reader(_inputStream);
    }
}