using ConduitServer.Net.Packets;

namespace ConduitServer.Serialization.Packets
{
    class PacketSerializer : IPacketSerializer
    {
        public byte[] Serialize<T>(T packet) where T : IPacket
        {
            var type = typeof(T);

            throw new System.NotImplementedException();
        }
    }
}