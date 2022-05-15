using System.Runtime.CompilerServices;

using Conduit.Net.Packets;
using Conduit.Net.Packets.Login;
using Conduit.Net.Reflection;

namespace Conduit.Net.Protocols
{
    public class LoginProtocol : IProtocol
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Handle(Packet packet) => Dispatcher<LoginProtocol>.Action(this, packet);

        public void Handle(Start loginStart)
        {

        }
    }
}