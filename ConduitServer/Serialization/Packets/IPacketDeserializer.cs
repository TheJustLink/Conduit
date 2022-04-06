using ConduitServer.Net.Packets;

namespace ConduitServer.Serialization.Packets
{
    interface IPacketDeserializer
    {
        T Deserialize<T>(RawPacket rawPacket);
    }
}