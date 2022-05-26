using System.IO.Compression;

using Conduit.Net.Packets;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Net.Connection
{
    public interface IConnection
    {
        bool Connected { get; }
        bool HasData { get; }

        EndPoint RemotePoint { get; }
        EndPoint LocalPoint { get; }

        void AddCompression(int treshold, CompressionLevel compressionLevel = CompressionLevel.Optimal);
        void AddEncryption(byte[] key);

        void ChangePacketFlow(PacketFlow packetFlow);

        void Send(Packet packet);
        Packet Receive();

        void Disconnect();
    }
}