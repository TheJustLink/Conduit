using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Serialization
{
    public abstract class JsonObject<T> where T : JsonObject<T>
    {
        private Type TType;
        private PropertyInfo[] Properties;
        public JsonObject()
        {
            TType = typeof(T);
            Properties = TType.GetProperties(BindingFlags.Public);
        }

        public string Serialize()
        {
            var th = (T)this;
            return JsonSerializer.Serialize(th);
        }
        public void Deserialize(string json)
        {
            var th = JsonSerializer.Deserialize<T>(json);

            For.ForFixedArrayIncrease(Properties, (finfo) =>
            {
                finfo.SetValue(this, finfo.GetValue(th));
            });
        }
    }
}
