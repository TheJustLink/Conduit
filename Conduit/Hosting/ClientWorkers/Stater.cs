using Conduit.Network.Protocol.Serializable;
using Conduit.Network.Protocol.Serializable.Status;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            ClientMaintainer.Protocol.SPacket.Deserialize(ClientMaintainer.VClient.NetworkStream, onlyheader);

            switch (onlyheader.Id)
            {
                default:
                    {
                        MaintainStatus();
                        break;
                    }
                case 0x01:
                    {
                        MemoryStream ms = ReadToStream(onlyheader.Length - 1);
                        MaintainPing(ms);
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
                Json = ClientMaintainer.VClient.ServerInstance.Status.GetInfo(),
            };

            Stopwatch sw = Stopwatch.StartNew();
            ClientMaintainer.Protocol.SResponse.Serialize(ClientMaintainer.VClient.NetworkStream, response);
            sw.Stop();
            Console.WriteLine($"Serialized response for {sw.Elapsed.TotalMilliseconds}ms");
        }
        private void MaintainPing(MemoryStream ms)
        {
            var ping = new Ping();
            ClientMaintainer.Protocol.SPing.DeserializeLess(ms, ping);
            Console.WriteLine("Requested ping");
            ClientMaintainer.Protocol.SPing.Serialize(ClientMaintainer.VClient.NetworkStream, ping);
            Pinged = true;
        }
    }
}
