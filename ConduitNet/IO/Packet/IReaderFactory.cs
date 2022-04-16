namespace Conduit.Net.IO.Packet
{
    public interface IReaderFactory
    {
        IReader Create();
        IReader CreateWithCompression();
    }
}