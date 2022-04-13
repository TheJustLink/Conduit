using Conduit.Network.Protocol.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Hosting.ClientWorkers
{
    public class Handshaker : ClientWorker
    {
        public bool IsHandshaked { get; private set; }
        public Handshaker(ClientHandler cm) : base(cm)
        {
        }
        public override void Handling()
        {
            if (!ClientHandler.VClient.RemoteStream.DataAvailable)
                return;

            //Console.WriteLine("Handshaking...");

            var handshake = ClientHandler.Protocol.SHandshake.PacketPool.Get();
            ClientHandler.Protocol.SHandshake.Serializator.Deserialize(ClientHandler.VClient.RemoteStream, handshake);
            //if (!handshake.IsValidLength)
            //    return;

            //Console.WriteLine("Handshaked!");

            switch (handshake.NextStateEnum)
            {
                case NextState.Status:
                    {
                        ClientHandler.ChangeState(NetworkState.Status);
                        break;
                    }
                case NextState.Login:
                    {
                        ClientHandler.ChangeState(NetworkState.Login);
                        break;
                    }
            }
            ClientHandler.Protocol.SHandshake.PacketPool.Return(handshake);
        }
    }
}
