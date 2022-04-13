using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Conduit.Configurable
{
    public sealed class NetworkOptions
    {
        [JsonPropertyName("timePerConnect")]
        public int TimePerConnect { get; set; } // seconds

        [JsonPropertyName("port")]
        public int Port { get; set; } // main connect networkport
        [JsonPropertyName("TimeLimitConnection")]
        public int TimeLimitConnection { get; set; }

        public NetworkOptions()
        {
            TimePerConnect = 0;
            Port = 0;
        }
    }
}
