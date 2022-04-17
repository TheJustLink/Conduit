namespace Conduit.Net.IO.CompressedRawPacket
{
    public class Reader
    {
        private readonly Binary.Reader _binaryReader;

        public Reader(Binary.Reader binaryReader)
        {
            _binaryReader = binaryReader;
        }

        public Packets.CompressedRawPacket Read()
        {
            return new Packets.CompressedRawPacket
            {
                Length = _binaryReader.Read7BitEncodedInt(),
                UncompressedLength = _binaryReader.Read7BitEncodedInt(),
                CompressedData = _binaryReader.ReadBytes(0)// TODO
            };
        }
    }
}