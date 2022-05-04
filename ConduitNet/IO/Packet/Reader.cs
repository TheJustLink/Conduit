using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Conduit.Net.IO.Packet.Serialization;

namespace Conduit.Net.IO.Packet
{
    public class Reader : IReader
    {
        public RawPacket.IReader RawPacketReader { get; set; }

        public Reader(Stream stream, bool leaveOpen = false) : this(new Binary.Reader(stream, Encoding.UTF8, leaveOpen)) { }
        public Reader(Binary.Reader binaryReader) : this(new RawPacket.Reader(binaryReader)) { }
        public Reader(RawPacket.IReader rawPacketReader)
        {
            RawPacketReader = rawPacketReader;
        }
        public Reader() { }

        public void Dispose()
        {
            RawPacketReader?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public T Read<T>() where T : Packets.Packet, new()
        {
            return Read<T>(Read());
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public T Read<T>(Packets.RawPacket packet) where T : Packets.Packet, new()
        {
            return Deserializer.Deserialize<T>(packet);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public Packets.RawPacket Read()
        {
            return RawPacketReader.Read();
        }
    }
}