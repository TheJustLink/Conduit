using System.IO;

using ConduitServer.Net.Packets;

namespace ConduitServer.Serialization.Packets
{
    interface IPacketSerializer
    {
        byte[] Serialize<T>(T packet) where T : Packet;
        void Serialize<T>(Stream output, T packet) where T : Packet;
    }
}