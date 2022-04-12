using System.IO;

using ConduitServer.Serialization.Packets;

namespace ConduitServer.Net.Packets
{
    class NetworkPacketSender : IPacketSender
    {
        private readonly Stream _stream;
        private readonly IPacketSerializer _serializer;

        public NetworkPacketSender(Stream stream, IPacketSerializer serializer)
        {
            _stream = stream;
            _serializer = serializer;
        }

        public void Send<T>(T packet) where T : Packet
        {
            _serializer.Serialize(_stream, packet);
        }
    }
}