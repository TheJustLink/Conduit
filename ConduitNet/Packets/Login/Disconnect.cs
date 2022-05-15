using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Login
{
    [Packet(0x00)]
    public class Disconnect : Packets.Disconnect { }
}