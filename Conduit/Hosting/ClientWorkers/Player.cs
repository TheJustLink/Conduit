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
        public Player (ClientMaintainer cm) : base(cm)
        {
        }

        public override void Handling()
        {
            Packet onlyheader = new Packet();
            ClientMaintainer.Protocol.SPacket.Deserialize(ClientMaintainer.VClient.NetworkStream, onlyheader);
            
            switch (onlyheader.Id)
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


        }
    }
}
