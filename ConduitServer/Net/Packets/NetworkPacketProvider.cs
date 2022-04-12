using System.IO;

using ConduitServer.Serialization.Packets;

namespace ConduitServer.Net.Packets
{
    class NetworkPacketProvider : IPacketProvider
    {
        private readonly Stream _stream;
        private readonly IPacketDeserializer _deserializer;

        public NetworkPacketProvider(Stream stream, IPacketDeserializer deserializer)
        {
            _stream = stream;
            _deserializer = deserializer;
        }

        public T Read<T>() where T : Packet, new()
        {
            return _deserializer.Deserialize<T>(_stream);
        }
    }
}