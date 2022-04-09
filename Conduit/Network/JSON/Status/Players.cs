using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Status
{
    public sealed class Players
    {
        [JsonPropertyName("max")]       public ulong Max { get; set; }
        [JsonPropertyName("online")]    public ulong Online { get; set; }
    }
}
