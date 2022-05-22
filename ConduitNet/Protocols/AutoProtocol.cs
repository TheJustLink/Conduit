using System;

using Conduit.Net.Packets;
using Conduit.Net.Reflection;

namespace Conduit.Net.Protocols
{
    public abstract class AutoProtocol<T> : Protocol
        where T : AutoProtocol<T>
    {
        public override void Handle(Packet packet)
        {
            if (Dispatcher<T>.ContainsAction(packet.GetType().GetHashCode()))
            {
                Console.WriteLine($"Dispatch {packet.GetType().Name} to {typeof(T).Name}");
                Dispatcher<T>.Action(this as T, packet);
            }
            else
            {
                Console.WriteLine($"Not found handler for {packet.GetType().Name} in {typeof(T).Name}");
            }
        }
    }
}