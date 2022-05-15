using Conduit.Net.Packets;

namespace Conduit.Net.Protocols
{
    public interface IProtocol
    {
        void Handle(Packet packet);
    }
}