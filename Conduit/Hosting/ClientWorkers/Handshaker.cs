using Conduit.Network.Protocol.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Hosting.ClientWorkers
{
    public class Handshaker : ClientWorker
    {
        public bool IsHandshaked { get; private set; }
        public Handshaker(ClientMaintainer cm) : base(cm)
        {
        }

        public override void Maintain()
        {
            WaitToAvailable();

            Console.WriteLine("Handshaking...");

            var handshake = new Handshake();
            ClientMaintainer.Protocol.SHandshake.Deserialize(ClientMaintainer.VClient.NetworkStream, handshake);
            if (!handshake.IsValidLength)
                return;

            Console.WriteLine("Handshaked!");

            switch (handshake.NextStateEnum)
            {
                case NextState.Status:
                    {
                        ClientMaintainer.ChangeState(ClientState.Status);
                        break;
                    }
                case NextState.Login:
                    {
                        ClientMaintainer.ChangeState(ClientState.Login);
                        break;
                    }
            }
        }
    }
}
