using Conduit.Net.Attributes;
using Conduit.Net.Data;

namespace Conduit.Net.Packets
{
    public abstract class Disconnect : Packet
    {
        [Json] public Message Reason;
    }
}