using Conduit.Net.Data;
using Conduit.Net.Reflection;

namespace Conduit.Net.Attributes
{
    public class VarLongAttribute : AsAttribute
    {
        public VarLongAttribute() : base(Object<VarLong>.HashCode) { }
    }
}