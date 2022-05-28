using Conduit.Net.Attributes;
using Conduit.Net.Data.Status;

namespace Conduit.Net.Packets.Status
{
    public class Response : Packet
    {
        [Json] public Server Server;
    }
}