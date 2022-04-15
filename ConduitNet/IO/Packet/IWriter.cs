namespace Conduit.Net.IO.Packet
{
    public interface IWriter
    {
        void Write(Packets.Packet packet);
        void Write(Packets.RawPacket rawPacket);
    }
}