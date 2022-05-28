using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;

namespace Conduit.Net.IO.RawPacket
{
    public class CompressedWriter : IWriter
    {
        public Binary.Writer BinaryWriter { get; set; }

        private readonly int _compressionTreshold;
        private readonly CompressionLevel _compressionLevel;

        public CompressedWriter(Stream stream, int compressionTreshold, CompressionLevel compressionLevel = CompressionLevel.Optimal, bool leaveOpen = false)
            : this(new Binary.Writer(stream, Encoding.UTF8, leaveOpen), compressionTreshold, compressionLevel) { }
        public CompressedWriter(Binary.Writer binaryWriter, int compressionTreshold, CompressionLevel compressionLevel = CompressionLevel.Optimal)
            : this(compressionTreshold, compressionLevel)
        {
            BinaryWriter = binaryWriter;
        }
        public CompressedWriter(int compressionTreshold, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            _compressionTreshold = compressionTreshold;
            _compressionLevel = compressionLevel;
        }

        public void Dispose()
        {
            BinaryWriter?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void Write(Packets.RawPacket rawPacket)
        {
            if (rawPacket.Length < _compressionTreshold)
                WriteUncompressed(rawPacket);
            else WriteCompressed(rawPacket);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private void WriteUncompressed(Packets.RawPacket rawPacket)
        {
            BinaryWriter.Write7BitEncodedInt(rawPacket.Length + 1);
            BinaryWriter.Write7BitEncodedInt(0);
            BinaryWriter.Write7BitEncodedInt(rawPacket.Id);
            BinaryWriter.Write(rawPacket.Data);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private void WriteCompressed(Packets.RawPacket rawPacket)
        {
            var compressedMemory = new MemoryStream();
            using var compressedDataWriter = new Binary.Writer(new ZLibStream(compressedMemory, _compressionLevel));

            compressedDataWriter.Write7BitEncodedInt(rawPacket.Id);
            compressedDataWriter.Write(rawPacket.Data);
            compressedDataWriter.Flush();

            var compressedData = compressedMemory.ToArray();

            var uncompressedLengthMemory = new MemoryStream(4);
            using var uncompressedLengthWriter = new Binary.Writer(uncompressedLengthMemory);

            uncompressedLengthWriter.Write7BitEncodedInt(rawPacket.Length);
            var uncompressedLengthData = uncompressedLengthMemory.ToArray();

            BinaryWriter.Write7BitEncodedInt(uncompressedLengthData.Length + compressedData.Length);
            BinaryWriter.Write(uncompressedLengthData);
            BinaryWriter.Write(compressedData);
        }
    }
}