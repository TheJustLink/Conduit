using System.IO;
using System.Text;

using Conduit.Net.IO.Encryption;
using Conduit.Net.IO.RawPacket;

namespace Conduit.Net.IO.Packet
{
    public class ReaderFactory
    {
        private readonly Stream _inputStream;

        public ReaderFactory(Stream inputStream) => _inputStream = inputStream;

        public void AddCompression(IReader reader)
        {
            var binaryReader = reader.Raw.Binary;
            reader.Raw = new CompressedReader(binaryReader);
        }
        public void AddEncryption(IReader reader, byte[] key)
        {
            var decryptionStream = Aes.CreateDecryptionStream(_inputStream, key);

            reader.Raw.Binary = new Binary.Reader(decryptionStream, Encoding.UTF8, true);
        }

        public IReader Create() => new Reader(_inputStream);
    }
}