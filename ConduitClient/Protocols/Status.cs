using System;
using System.Text.Json;

using Conduit.Net;
using Conduit.Net.Packets.Status;
using Conduit.Net.Protocols;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Client.Protocols
{
    public class Status : ClientAutoProtocol<Status, StatusFlow>, IStarteable
    {
        private static readonly long s_pingPayload = Random.Shared.NextInt64();

        private DateTime _pingSendTime;

        public void Start() => Connection.Send(new Request());

        public void Handle(Response response)
        {
            Console.WriteLine("Server info:\n" + JsonSerializer.Serialize(response.Server, new JsonSerializerOptions { IncludeFields = true }));

            Connection.Send(new Ping { Payload = s_pingPayload });
            _pingSendTime = DateTime.Now;
        }
        public void Handle(Ping pingResponse)
        {
            var latency = DateTime.Now - _pingSendTime;

            if (pingResponse.Payload == s_pingPayload)
                Console.WriteLine("Ping ms " + latency.TotalMilliseconds);
            else throw new NotImplementedException();

            Connection.Disconnect();
        }
    }
}