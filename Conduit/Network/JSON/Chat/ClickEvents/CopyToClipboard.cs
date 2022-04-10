using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Chat.ClickEvents
{
    internal class CopyToClipboard : ClickEvent
    {
        [JsonPropertyName("value")] public string Text { get; set; }

        public CopyToClipboard() => Action = "copy_to_clipboard";
    }
}
