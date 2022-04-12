using System.IO;

using ConduitServer.Net.Packets;

namespace ConduitServer.Serialization.Packets
{
    interface IPacketSerializer
    {
        void Serialize<T>(Stream output, T packet) where T : Packet;
    }
}