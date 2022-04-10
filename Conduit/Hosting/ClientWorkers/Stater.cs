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

        public override void Handling()
        {
            WaitToAvailable();

            RawPacket raw = new RawPacket();
            ClientMaintainer.Protocol.SRawPacket.DeserializeBigDataOffset(ClientMaintainer.VClient.RemoteStream, raw);

            switch (raw.Id)
            {
                default:
                    {
                        OnStatus();
                        break;
                    }
                case 0x01:
                    {
                        OnPing(raw.Data);
                        break;
                    }
            }

            if (Pinged)
                ShutdownClient();
        }
        private void OnStatus()
        {
            Console.WriteLine("Requested status");

            var response = new Response()
            {
                Json = ClientMaintainer.VClient.ServerInstance.Status.GetInfo(),
            };

            Stopwatch sw = Stopwatch.StartNew();
            ClientMaintainer.Protocol.SResponse.Serialize(ClientMaintainer.VClient.RemoteStream, response);
            sw.Stop();
            Console.WriteLine($"Serialized response for {sw.Elapsed.TotalMilliseconds}ms");
        }
        private void OnPing(byte[] data)
        {
            MemoryStream ms = new(data);

            var ping = new Ping();
            ClientMaintainer.Protocol.SPing.DeserializeLess(ms, ping);
            Console.WriteLine("Requested ping");
            ClientMaintainer.Protocol.SPing.Serialize(ClientMaintainer.VClient.RemoteStream, ping);
            ms.Close();
            Pinged = true;
        }
    }
}
