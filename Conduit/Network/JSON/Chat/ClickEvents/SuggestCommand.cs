using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Chat.ClickEvents
{
    public sealed class SuggestCommand : ClickEvent
    {
        [JsonPropertyName("value")] public string Value { get; set; }

        public SuggestCommand() => Action = "suggest_command";
    }
}
