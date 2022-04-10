﻿using Conduit.Network.Protocol.Serializable;
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

        public override void Handling()
        {
            WaitToAvailable();

            Console.WriteLine("Handshaking...");

            Handshake handshake = new();
            ClientMaintainer.Protocol.SHandshake.Deserialize(ClientMaintainer.VClient.RemoteStream, handshake);
            if (!handshake.IsValidLength)
                return;

            Console.WriteLine("Handshaked!");

            switch (handshake.NextStateEnum)
            {
                case NextState.Status:
                    {
                        ClientMaintainer.ChangeState(NetworkState.Status);
                        break;
                    }
                case NextState.Login:
                    {
                        ClientMaintainer.ChangeState(NetworkState.Login);
                        break;
                    }
            }
        }
    }
}
