namespace ConduitServer.Net.Packets
{
    class RawPacket : IPacket
    {
        public int Id { get; }
        public byte[] Data { get; }

        public RawPacket(int id, byte[] data)
        {
            Id = id;
            Data = data;
        }
    }
}