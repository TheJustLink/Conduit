using System.IO;
using System.Text;

namespace Conduit.Net.IO.RawPacket
{
    public class Writer : IWriter
    {
        private readonly Binary.Writer _binaryWriter;

        public Writer(Stream stream, bool leaveOpen = false) : this(new Binary.Writer(stream, Encoding.UTF8, leaveOpen)) { }
        public Writer(Binary.Writer binaryWriter)
        {
            _binaryWriter = binaryWriter;
        }

        public void Write(Packets.RawPacket rawPacket)
        {
            _binaryWriter.Write7BitEncodedInt(rawPacket.Length);
            _binaryWriter.Write7BitEncodedInt(rawPacket.Id);
            _binaryWriter.Write(rawPacket.Data);
        }

        public void Dispose()
        {
            _binaryWriter?.Dispose();
        }
    }
}