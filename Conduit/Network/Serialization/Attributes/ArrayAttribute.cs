using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Serialization.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ArrayAttribute : Attribute, IIDAtribute
    {
        public ulong Id { get; private set; }

        public ArrayAttribute(ulong id)
        {
            Id = id;
        }
    }
}
