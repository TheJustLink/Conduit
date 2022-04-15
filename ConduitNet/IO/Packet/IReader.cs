namespace Conduit.Net.IO.Packet
{
    public interface IReader
    {
        T Read<T>() where T : Packets.Packet, new();
        T Read<T>(Packets.RawPacket rawPacket) where T : Packets.Packet, new();
        Packets.RawPacket Read();
    }
}