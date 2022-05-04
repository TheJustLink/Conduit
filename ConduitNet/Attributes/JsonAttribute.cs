using Conduit.Net.Data;

namespace Conduit.Net.Attributes
{
    public class JsonAttribute : AsAttribute
    {
        public JsonAttribute() : base(Json.TypeHash) { }
    }
}