using System.Runtime.CompilerServices;

using Conduit.Net.Packets;
using Conduit.Net.Reflection;
using Conduit.Net.Packets.Status;

namespace Conduit.Net.Protocols
{
    public class StatusProtocol : IProtocol
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Handle(Packet packet) => Dispatcher<StatusProtocol>.Action(this, packet);

        public void Handle(Request request)
        {

        }
        public void Handle(Ping ping)
        {

        }
    }
}