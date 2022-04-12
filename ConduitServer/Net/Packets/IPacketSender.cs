namespace ConduitServer.Net.Packets
{
    interface IPacketSender
    {
        void Send<T>(T packet) where T : Packet;
    }
}