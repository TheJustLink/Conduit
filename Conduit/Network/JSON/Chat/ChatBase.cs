using Conduit.Network.JSON.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Chat
{
    public sealed class ChatBase : JsonObject<ChatBase>
    {
        [JsonPropertyName("text")] public string Text { get; set; }
        [JsonPropertyName("bold")] public bool Bold { get; set; }

        [JsonPropertyName("extra")] public List<string> Extra { get; set; }

        public ChatBase()
        {
            Extra = new List<string>();
        }
    }
}
