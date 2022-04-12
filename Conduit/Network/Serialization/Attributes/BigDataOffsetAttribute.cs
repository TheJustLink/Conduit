using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Serialization.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BigDataOffsetAttribute : Attribute
    {
        public int Offset;
        public BigDataOffsetAttribute(int offset) => Offset = offset;
    }
}
