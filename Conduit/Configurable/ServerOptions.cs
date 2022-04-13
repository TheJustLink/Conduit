using Conduit.Network.JSON.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Conduit.Configurable
{
    public sealed class ServerOptions : JsonObject<ServerOptions>
    {
        [JsonPropertyName("network")]
        public NetworkOptions NetworkOptions { get; set; }

        [JsonPropertyName("resources")]
        public ResourcesOptions ResourcesOptions { get; set; }
        
        public ServerOptions()
        {
            NetworkOptions = new NetworkOptions();
            ResourcesOptions = new ResourcesOptions();
        }
    }
}
