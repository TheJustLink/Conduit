﻿using Conduit.Network.Protocol.Serializable;
using Conduit.Network.Protocol.Serializable.Logging;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Hosting.ClientWorkers
{
    public class Loginer : ClientWorker
    {
        public Loginer (ClientMaintainer cm) : base(cm)
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
                        MemoryStream ms = ReadToStream(onlyheader.Length - 1);
                        OnLoginStart(ms);
                        break;
                    }
                case 0x01:
                    {
                        
                        break;
                    }
            }

        }

        private void OnLoginStart(MemoryStream ms)
        {
            var loginstart = new LoginStart();
            ClientMaintainer.Protocol.SLoginStart.DeserializeLess(ms, loginstart);
            Console.WriteLine("Username=" + loginstart.Username);

            var loginSuccess = new LoginSuccess()
            {
                Guid = Guid.NewGuid(),
                Username = loginstart.Username
            };

            ClientMaintainer.Protocol.SLoginSuccess.Serialize(ClientMaintainer.VClient.NetworkStream, loginSuccess);

            ClientMaintainer.ChangeState(ClientState.Play);
        }
    }
}
