using System.IO;

using ConduitServer.Net.Packets;

namespace ConduitServer.Serialization.Packets
{
    interface IPacketDeserializer
    {
        T Deserialize<T>(Stream stream) where T : Packet, new();
    }
}