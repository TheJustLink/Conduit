using Conduit.Network.Protocol.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Hosting.ClientWorkers
{
    public sealed class Player : ClientWorker
    {
        public Player (ClientHandler cm) : base(cm)
        {
        }

        public override void Handling()
        {
            var ptools = ClientHandler.Protocol;

            RawPacket raw = ptools.SRawPacket.PacketPool.Get();
            ptools.SRawPacket.Serializator.Deserialize(ClientHandler.VClient.RemoteStream, raw);

            switch (raw.Id)
            {
                default:
                    {
                        //OnStatus();
                        break;
                    }
                case 0x01:
                    {
                        //OnPing(onlyheader.Length - 1);
                        break;
                    }
            }

            ptools.SRawPacket.PacketPool.Return(raw);
        }
    }
}
