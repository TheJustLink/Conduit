using System;

namespace Conduit.Net.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketAttribute : ConduitAttribute
    {
        public byte Id;

        public PacketAttribute(byte id) => Id = id;
    }
}