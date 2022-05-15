using System.Runtime.CompilerServices;

using Conduit.Net.Packets;
using Conduit.Net.Reflection;
using Conduit.Net.Packets.Play.Clientbound;

namespace Conduit.Net.Protocols
{
    public class PlayProtocol : IProtocol
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Handle(Packet packet) => Dispatcher<PlayProtocol>.Action(this, packet);

        public void Handle(JoinGame joinGame)
        {

        }
    }
}