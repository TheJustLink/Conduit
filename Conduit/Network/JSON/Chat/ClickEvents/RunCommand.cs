using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Network.JSON.Chat.ClickEvents
{
    public sealed class RunCommand : ClickEvent
    {
        [JsonPropertyName("value")] public string Command { get; set; }

        public RunCommand() => Action = "run_command";
    }
}
