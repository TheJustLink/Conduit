namespace ConduitServer.Net.Packets
{
    abstract class Disconnect : Packet
    {
        public Chat Reason;
    }
}