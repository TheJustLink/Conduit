using Conduit.Network.JSON.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Status
{
    public sealed class StatusInfo : JsonObject<StatusInfo>
    {
        [JsonPropertyName("version")]       public Version Version { get; set; }
        [JsonPropertyName("players")]       public Players Players { get; set; }
        [JsonPropertyName("description")]   public Description Description { get; set; }
    }
}
