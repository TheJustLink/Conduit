using System.IO.Compression;

namespace Conduit.Net.IO.Packet
{
    public interface IWriterFactory
    {
        IWriter Create();
        IWriter CreateWithCompression(int treshold, CompressionLevel compressionLevel = CompressionLevel.Optimal);
    }
}