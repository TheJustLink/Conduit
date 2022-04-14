using System.IO;
using Conduit.Net.Serialization;

namespace Conduit.Net.IO.Packet
{
    public class NetworkPacketProvider : IPacketProvider
    {
        private readonly Stream _stream;
        private readonly IPacketDeserializer _deserializer;

        public NetworkPacketProvider(Stream stream, IPacketDeserializer deserializer)
        {
            _stream = stream;
            _deserializer = deserializer;
        }

        public T Read<T>() where T : Packets.Packet, new()
        {
            return _deserializer.Deserialize<T>(_stream);
        }
    }
}