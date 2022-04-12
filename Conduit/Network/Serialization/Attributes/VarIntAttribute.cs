using System;
using System.Collections.Generic;
using System.Text;

namespace Conduit.Network.Serialization.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class VarIntAttribute : Attribute { }
}
