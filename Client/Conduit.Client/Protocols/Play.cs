using System;
using System.Text.Json;

using Conduit.Net.Protocols;
using Conduit.Net.Protocols.Flow;
using Conduit.Net.Packets.Play.Clientbound;

namespace Conduit.Client.Protocols
{
    public class Play : ClientAutoProtocol<Play, PlayFlow>
    {
        public void Handle(JoinGame joinGame)
        {
            Console.WriteLine($"Join game received:\n{JsonSerializer.Serialize(joinGame, new JsonSerializerOptions { IncludeFields = true })}");
        }
    }
}