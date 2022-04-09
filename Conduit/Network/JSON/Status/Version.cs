using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Status
{
    public sealed class Version
    {
        [JsonPropertyName("name")]      public string Name { get; set; }
        [JsonPropertyName("protocol")]  public int ProtocolVersion { get; set; }
    }
}
