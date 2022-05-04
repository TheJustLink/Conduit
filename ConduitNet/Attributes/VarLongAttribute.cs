using Conduit.Net.Data;

namespace Conduit.Net.Attributes
{
    public class VarLongAttribute : AsAttribute
    {
        public VarLongAttribute() : base(VarLong.TypeHash) { }
    }
}