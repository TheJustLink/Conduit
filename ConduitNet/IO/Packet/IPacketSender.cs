namespace Conduit.Net.IO.Packet
{
    public interface IPacketSender
    {
        void Send<T>(T packet) where T : Packets.Packet;
    }
}