using ConduitServer.Net.Packets;

namespace ConduitServer.Serialization.Packets
{
    interface IPacketSerializer
    {
        byte[] Serialize<T>(T packet) where T : Packet;
    }
}