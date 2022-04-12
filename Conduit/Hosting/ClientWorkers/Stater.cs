using Conduit.Network.Protocol.Serializable;
using Conduit.Network.Protocol.Serializable.Status;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Hosting.ClientWorkers
{
    public sealed class Stater : ClientWorker
    {
        public bool Pinged;
        public Stater(ClientHandler cm) : base(cm)
        {
        }

        public override void Handling()
        {
            WaitToAvailable();

            var ptools = ClientMaintainer.Protocol;

            RawPacket raw = ptools.SRawPacket.PacketPool.Get();
            ptools.SRawPacket.Serializator.DeserializeBigDataOffset(ClientMaintainer.VClient.RemoteStream, raw);

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

            ptools.SRawPacket.PacketPool.Return(raw);

            if (Pinged)
                ShutdownClient();
        }

        private void OnStatus()
        {
            //Console.WriteLine("Requested status");

            var response = ClientMaintainer.Protocol.SResponse.PacketPool.Get();
            response.Json = ClientMaintainer.VClient.ServerInstance.Status.LastStatus;

            Stopwatch sw = Stopwatch.StartNew();
            ClientMaintainer.Protocol.SResponse.Serializator.Serialize(ClientMaintainer.VClient.RemoteStream, response);
            sw.Stop();

            ClientMaintainer.Protocol.SResponse.PacketPool.Return(response);

            //Console.WriteLine($"Serialized response for {sw.Elapsed.TotalMilliseconds}ms");
        }
        private void OnPing(byte[] data)
        {
            MemoryStream ms = new(data);

            var ping = new Ping();
            ClientMaintainer.Protocol.SPing.Serializator.DeserializeLess(ms, ping);
            //Console.WriteLine("Requested ping");
            ClientMaintainer.Protocol.SPing.Serializator.Serialize(ClientMaintainer.VClient.RemoteStream, ping);
            ms.Close();
            Pinged = true;
        }
    }
}
