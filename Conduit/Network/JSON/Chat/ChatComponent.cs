using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Chat
{
    public sealed class ChatComponent
    {
        [JsonPropertyName("text")] public string Text { get; set; }
        [JsonPropertyName("bold")] public bool Bold { get; set; }
        [JsonPropertyName("italic")] public bool Italic { get; set; }
        [JsonPropertyName("underlined")] public bool Underlined { get; set; }
        [JsonPropertyName("strikethrough")] public bool Strikethrough { get; set; }
        [JsonPropertyName("obfuscated")] public bool Obfuscated { get; set; }
        [JsonPropertyName("font")] public string Font { get; set; }
        [JsonPropertyName("color")] public string Color { get; set; }
        [JsonPropertyName("insertion")] public string Insertion { get; set; }
        [JsonPropertyName("clickEvent")] public ClickEvent ClickEvent { get; set; }
    }
}
