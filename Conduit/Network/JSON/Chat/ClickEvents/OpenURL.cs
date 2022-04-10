using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Chat.ClickEvents
{
    public sealed class OpenURL : ClickEvent
    {
        [JsonPropertyName("value")] public string URL { get; set; }

        public OpenURL() => Action = "open_url";
    }
}
