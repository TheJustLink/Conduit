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
            if (!ClientHandler.VClient.RemoteStream.DataAvailable)
                return;

            var ptools = ClientHandler.Protocol;

            RawPacket raw = ptools.SRawPacket.PacketPool.Get();
            ptools.SRawPacket.Serializator.DeserializeBigDataOffset(ClientHandler.VClient.RemoteStream, raw);

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

            var response = ClientHandler.Protocol.SResponse.PacketPool.Get();
            response.Json = ClientHandler.VClient.ServerInstance.Status.LastStatus;

            //Stopwatch sw = Stopwatch.StartNew();
            ClientHandler.Protocol.SResponse.Serializator.Serialize(ClientHandler.VClient.RemoteStream, response);
            //sw.Stop();

            ClientHandler.Protocol.SResponse.PacketPool.Return(response);

            //Console.WriteLine($"Serialized response for {sw.Elapsed.TotalMilliseconds}ms");
        }
        private void OnPing(byte[] data)
        {
            using var ms = new MemoryStream(data);

            var ping = ClientHandler.Protocol.SPing.PacketPool.Get();
            ClientHandler.Protocol.SPing.Serializator.DeserializeLess(ms, ping);
            //Console.WriteLine("Requested ping");
            ClientHandler.Protocol.SPing.Serializator.Serialize(ClientHandler.VClient.RemoteStream, ping);
            ClientHandler.Protocol.SPing.PacketPool.Return(ping);

            Pinged = true;
        } 
    }
}
