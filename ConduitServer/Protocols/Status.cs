using Conduit.Net.Data.Status;
using Conduit.Net.Data;
using Conduit.Net.Packets.Status;
using Conduit.Net.Protocols;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Server.Protocols
{
    public class Status : ServerAutoProtocol<Status, StatusFlow>
    {
        public void Handle(Request request)
        {
            var server = new Net.Data.Status.Server
            {
                Version = new Version { Name = "1.18", Protocol = 757 },
                Description = new Message { Text = "Minecraft hell server" },
                Players = new Players { Max = 666, Online = 66 }
            };
            var response = new Response { Server = server };

            Connection.Send(response);
        }
        public void Handle(Ping ping) => Connection.Send(ping);
    }
}