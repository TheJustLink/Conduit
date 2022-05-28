using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;

namespace Conduit.Net.IO.RawPacket
{
    public class CompressedReader : IReader
    {
        public Binary.Reader Binary { get; set; }

        public CompressedReader(Stream stream, bool leaveOpen = false)
            : this(new Binary.Reader(stream, Encoding.UTF8, leaveOpen)) { }
        public CompressedReader(Binary.Reader binaryReader)
        {
            Binary = binaryReader;
        }
        public CompressedReader() { }

        public void Dispose()
        {
            Binary?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public Packets.RawPacket Read()
        {
            var length = Binary.Read7BitEncodedInt();
            var packetData = Binary.ReadBytes(length);

            using var packetReader = new Binary.Reader(packetData);
            var uncompressedLength = packetReader.Read7BitEncodedInt();

            return uncompressedLength == 0
                 ? ReadUncompressed(packetReader, length)
                 : ReadCompressed(packetReader, length, uncompressedLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
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