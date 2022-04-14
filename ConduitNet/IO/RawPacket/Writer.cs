namespace Conduit.Net.IO.RawPacket
{
    public class Writer
    {
        private readonly Binary.Writer _binaryWriter;

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
    }
}