namespace Conduit.Net.IO.Packet
{
    public interface IPacketProvider
    {
        T Read<T>() where T : Packets.Packet, new();
    }
}