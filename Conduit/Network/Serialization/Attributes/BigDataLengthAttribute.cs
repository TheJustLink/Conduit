using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Serialization.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class BigDataLengthAttribute : Attribute
    {
        public ulong Id { get; private set; }
        public BigDataLengthAttribute(ulong id) => Id = id;
    }
}
