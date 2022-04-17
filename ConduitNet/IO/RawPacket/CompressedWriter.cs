using Conduit.Net.Packets;

using System.IO;
using System.IO.Compression;
using System.Text;

namespace Conduit.Net.IO.RawPacket
{
    public class CompressedWriter : IWriter
    {
        private readonly Binary.Writer _binaryWriter;
        private readonly int _compressionTreshold;
        private readonly CompressionLevel _compressionLevel;

        public CompressedWriter(Stream stream, int compressionTreshold, CompressionLevel compressionLevel, bool leaveOpen = false)
            : this(new Binary.Writer(stream, Encoding.UTF8, leaveOpen), compressionTreshold, compressionLevel) { }
        public CompressedWriter(Binary.Writer binaryWriter, int compressionTreshold, CompressionLevel compressionLevel)
        {
            _compressionTreshold = compressionTreshold;
            _compressionLevel = compressionLevel;

            _binaryWriter = binaryWriter;
        }

        public void Write(Packets.RawPacket rawPacket)
        {
            if (rawPacket.Length < _compressionTreshold)
                WriteUncompressed(rawPacket);
            else WriteCompressed(rawPacket);
        }

        private void WriteUncompressed(Packets.RawPacket rawPacket)
        {
            _binaryWriter.Write7BitEncodedInt(rawPacket.Length + 1);
            _binaryWriter.Write7BitEncodedInt(0);
            _binaryWriter.Write7BitEncodedInt(rawPacket.Id);
            _binaryWriter.Write(rawPacket.Data);
        }
        private void WriteCompressed(Packets.RawPacket rawPacket)
        {
            var compressedMemory = new MemoryStream();
            using var compressedDataWriter = new Binary.Writer(new ZLibStream(compressedMemory, CompressionLevel.Optimal));

            compressedDataWriter.Write7BitEncodedInt(rawPacket.Id);
            compressedDataWriter.Write(rawPacket.Data);
            compressedDataWriter.Flush();

            var compressedData = compressedMemory.ToArray();

            var uncompressedLengthMemory = new MemoryStream(4);
            using var uncompressedLengthWriter = new Binary.Writer(uncompressedLengthMemory);

            uncompressedLengthWriter.Write7BitEncodedInt(rawPacket.Length);
            var uncompressedLengthData = uncompressedLengthMemory.ToArray();

            _binaryWriter.Write7BitEncodedInt(uncompressedLengthData.Length + compressedData.Length);
            _binaryWriter.Write(uncompressedLengthData);
            _binaryWriter.Write(compressedData);
        }

        public void Dispose()
        {
            _binaryWriter?.Dispose();
        }
    }
}