using System.IO;

namespace Conduit.Net.Data
{
    public interface ISerializable
    {
        void Read(Stream stream);
        void Write(Stream stream);
    }
}