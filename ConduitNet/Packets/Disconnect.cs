using Conduit.Net.Data;

namespace Conduit.Net.Packets
{
    public abstract class Disconnect : Packet
    {
        public Message Reason;
    }
}