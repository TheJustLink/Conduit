using System;

using Conduit.Net;
using Conduit.Net.Connection;
using Conduit.Net.Packets.Status;
using Conduit.Net.Protocols;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Client.Protocols
{
    public class Status : ClientAutoProtocol<Status, StatusFlow>, IStarteable
    {
        private static readonly long s_pingPayload = Random.Shared.NextInt64();

        private DateTime _pingSendTime;

        public Status(State state, IConnection connection) : base(state, connection) { }
        
        public void Start() => Connection.Send(new Request());

        public void Handle(Response response)
        {
            Connection.Send(new Ping { Payload = s_pingPayload });

            _pingSendTime = DateTime.Now;

            throw new NotImplementedException();
        }
        public void Handle(Ping pingResponse)
        {
            var latency = DateTime.Now - _pingSendTime;

            if (pingResponse.Payload == s_pingPayload)
                throw new NotImplementedException();
            
            Connection.Disconnect();

            throw new NotImplementedException();
        }
    }
}