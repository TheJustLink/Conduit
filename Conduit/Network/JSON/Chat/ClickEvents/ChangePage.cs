using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Chat.ClickEvents
{
    public sealed class ChangePage : ClickEvent
    {
        [JsonPropertyName("value")] public int Page { get; set; }

        public ChangePage() => Action = "change_page";
    }
}
