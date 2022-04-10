using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Chat
{
    public abstract class ClickEvent
    {
        [JsonPropertyName("action")] public string Action { get; set; }
    }
}
