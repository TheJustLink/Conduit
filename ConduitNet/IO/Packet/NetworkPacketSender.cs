using System.IO;
using Conduit.Net.Serialization;

namespace Conduit.Net.IO.Packet
{
    public class NetworkPacketSender : IPacketSender
    {
        private readonly Stream _stream;
        private readonly IPacketSerializer _serializer;

        public NetworkPacketSender(Stream stream, IPacketSerializer serializer)
        {
            _stream = stream;
            _serializer = serializer;
        }

        public void Send<T>(T packet) where T : Packets.Packet
        {
            _serializer.Serialize(_stream, packet);
        }
    }
}