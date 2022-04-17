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
                   new Binary.Reader(new ZLibStream(stream, CompressionMode.Decompress, leaveOpen), Encoding.UTF8, leaveOpen)) { }
        public CompressedReader(Binary.Reader binaryReader, Binary.Reader compressedBinaryReader)
        {
            _binaryReader = binaryReader;
            _compressedBinaryReader = compressedBinaryReader;
        }

        public Packets.RawPacket Read()
        {
            var length = _binaryReader.Read7BitEncodedInt();
            var packetData = _binaryReader.ReadBytes(length);

            using var packetReader = new Binary.Reader(packetData);

            var pos1 = (int)packetReader.BaseStream.Position;
            var uncompressedDataLength = packetReader.Read7BitEncodedInt();
            var uncompressedDataBytes = (int)packetReader.BaseStream.Position - pos1;

            var compressedDataLength = length - uncompressedDataBytes;

            int id;
            byte[] data;

            if (uncompressedDataLength == 0)
            {
                id = packetReader.Read7BitEncodedInt();
                var uncompressedAndIdBytesCount = (int)packetReader.BaseStream.Position - pos1;

                data = packetReader.ReadBytes(length - uncompressedAndIdBytesCount);
            }
            else
            {
                var compressedData = packetReader.ReadBytes(compressedDataLength);
                using var memoryStream = new MemoryStream(compressedData);
                using var compressedBinaryReader = new Binary.Reader(new ZLibStream(memoryStream, CompressionMode.Decompress), Encoding.UTF8);
                
                id = compressedBinaryReader.Read7BitEncodedInt();
                data = compressedBinaryReader.ReadBytes(uncompressedDataLength - 1);
                length = uncompressedDataLength;
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