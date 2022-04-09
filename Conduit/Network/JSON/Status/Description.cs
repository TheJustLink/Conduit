using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Status
{
    public sealed class Description
    {
        [JsonPropertyName("text")] public string Text { get; set; }
    }
}
