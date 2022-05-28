using Conduit.Net.Data;
using Conduit.Net.Reflection;

namespace Conduit.Net.Attributes
{
    public class VarIntAttribute : AsAttribute
    {
        public VarIntAttribute() : base(Object<VarInt>.HashCode) { }
    }
}