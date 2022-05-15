using System.Runtime.CompilerServices;

using Conduit.Net.Packets;
using Conduit.Net.Packets.Handshake;
using Conduit.Net.Reflection;

namespace Conduit.Net.Protocols
{
    public class HandshakeProtocol : IProtocol
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Handle(Packet packet) => Dispatcher<HandshakeProtocol>.Action(this, packet);

        public void Handle(Handshake handshake)
        {
            
        }
    }
}