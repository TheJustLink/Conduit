using Conduit.Network.JSON.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.JSON
{
    public sealed class JSONCacher<TJSON> where TJSON : JsonObject<TJSON> 
    {
        public TJSON JsonObject;
        public string LastJson { get; private set; }
        public JSONCacher(TJSON json)
        {
            JsonObject = json;
        }
        public void Cache()
        {
            LastJson = JsonObject.Serialize();
        }
    }
}
