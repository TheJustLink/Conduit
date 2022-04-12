namespace ConduitServer.Net.Packets
{
    interface IPacketProvider
    {
        T Read<T>() where T : Packet, new();
    }
}