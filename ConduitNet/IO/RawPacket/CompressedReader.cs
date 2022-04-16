using System.IO;
using System.IO.Compression;
using System.Text;

namespace Conduit.Net.IO.RawPacket
{
    public class CompressedReader : IReader
    {
        private readonly Binary.Reader _binaryReader;
        private readonly Binary.Reader _compressedBinaryReader;

        public CompressedReader(Stream stream, bool leaveOpen = false)
            : this(new Binary.Reader(stream, Encoding.UTF8, leaveOpen),
                   new Binary.Reader(new GZipStream(stream, CompressionMode.Decompress, leaveOpen), Encoding.UTF8, leaveOpen)) { }
        public CompressedReader(Binary.Reader binaryReader, Binary.Reader compressedBinaryReader)
        {
            _binaryReader = binaryReader;
            _compressedBinaryReader = compressedBinaryReader;
        }

        public Packets.RawPacket Read()
        {
            var length = _binaryReader.Read7BitEncodedInt();
            var dataLength = _binaryReader.Read7BitEncodedInt();

            int id;
            byte[] data;

            if (dataLength == 0)
            {
                id = _binaryReader.Read7BitEncodedInt();
                data = _binaryReader.ReadBytes(length - 2);
            }
            else
            {
                var compressedData = _binaryReader.ReadBytes(length - 1);
                using var memoryStream = new MemoryStream(compressedData);
                using var compressedBinaryReader = new Binary.Reader(new GZipStream(memoryStream, CompressionMode.Decompress), Encoding.UTF8);

                id = compressedBinaryReader.Read7BitEncodedInt();
                data = compressedBinaryReader.ReadBytes(dataLength - 1);
                //id = _compressedBinaryReader.Read7BitEncodedInt();
                //data = _compressedBinaryReader.ReadBytes(dataLength - 1);
            }

            return new Packets.RawPacket { Length = length, Id = id, Data = data };
        }

        public void Dispose()
        {
            _binaryReader?.Dispose();
            _compressedBinaryReader?.Dispose();
        }
    }
}