using ConduitServer.Net.Packets;

namespace ConduitServer.Serialization.Packets
{
    class PacketDeserializer : IPacketDeserializer
    {
        public T Deserialize<T>(RawPacket rawPacket)
        {
            var type = typeof(T);

            throw new System.NotImplementedException();
        }
    }
}