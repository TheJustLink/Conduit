using Conduit.Network.Protocol.Serializable;
using Conduit.Network.Protocol.Serializable.Status;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Hosting.ClientWorkers
{
    public sealed class Stater : ClientWorker
    {
        public bool Pinged;
        public Stater(ClientMaintainer cm) : base(cm)
        {
        }

        public override void Maintain()
        {
            WaitToAvailable();

            Packet onlyheader = new Packet();
            ClientMaintainer.Protocol.SPacket.Deserialize(ClientMaintainer.VClient.NetworkStream, onlyheader, out MemoryStream readed);

            switch (onlyheader.Id)
            {
                default:
                    {
                        MaintainStatus();
                        break;
                    }
                case 0x01:
                    {
                        MaintainPing(readed);
                        break;
                    }
            }

            if (Pinged)
                ShutdownClient();
        }
        private void MaintainStatus()
        {
            Console.WriteLine("Requested status");

            var response = new Response()
            {
                Json = ClientMaintainer.VClient.ServerInstance.Status.GetInfo().Serialize(),
            };

            ClientMaintainer.Protocol.SResponse.Serialize(ClientMaintainer.VClient.NetworkStream, response);
        }
        private void MaintainPing(Stream stream)
        {
            var ping = new Ping();
            var cstream = new ConnectedStreams(stream, ClientMaintainer.VClient.NetworkStream, ClientMaintainer.VClient.TcpClient.Client.Available);
            ClientMaintainer.Protocol.SPing.Deserialize(cstream, ping);
            Console.WriteLine("Requested ping");
            ClientMaintainer.Protocol.SPing.Serialize(ClientMaintainer.VClient.NetworkStream, ping);
            Pinged = true;
        }
    }
}
