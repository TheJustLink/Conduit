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
        public void Dispose()
        {
            _binaryReader?.Dispose();
            _compressedBinaryReader?.Dispose();
        }

        public Packets.RawPacket Read()
        {
            var length = _binaryReader.Read7BitEncodedInt();
            var packetData = _binaryReader.ReadBytes(length);

            using var packetReader = new Binary.Reader(packetData);
            var uncompressedLength = packetReader.Read7BitEncodedInt();

            return uncompressedLength == 0
                 ? ReadUncompressed(packetReader, length)
                 : ReadCompressed(packetReader, length, uncompressedLength);
        }

        private static Packets.RawPacket ReadUncompressed(Binary.Reader packetReader, int packetLength)
        {
            var id = packetReader.Read7BitEncodedInt();
            var dataLength = packetLength - (int)packetReader.BaseStream.Position;
            var data = packetReader.ReadBytes(dataLength);

            return new Packets.RawPacket
            {
                Length = packetLength - 1,
                Id = id,
                Data = data
            };
        }
        private static Packets.RawPacket ReadCompressed(Binary.Reader packetReader, int packetLength, int uncompressedLength)
        {
            var compressedDataLength = packetLength - (int)packetReader.BaseStream.Position;
            var compressedData = packetReader.ReadBytes(compressedDataLength);

            using var decompressor = new ZLibStream(new MemoryStream(compressedData), CompressionMode.Decompress);
            var uncompressedData = new byte[uncompressedLength];
            decompressor.Read(uncompressedData);
            
            using var dataReader = new Binary.Reader(uncompressedData);

            var id = dataReader.Read7BitEncodedInt();
            var dataLength = uncompressedLength - (int)dataReader.BaseStream.Position;
            var data = dataReader.ReadBytes(dataLength);

            return new Packets.RawPacket
            {
                Length = uncompressedLength,
                Id = id,
                Data = data
            };
        }
    }
}