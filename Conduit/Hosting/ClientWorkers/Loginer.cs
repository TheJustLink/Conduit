using Conduit.Network.JSON.Chat;
using Conduit.Network.Protocol.Serializable;
using Conduit.Network.Protocol.Serializable.Logging;
using Conduit.Network.Protocol.Serializable.Status;
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
        public Loginer (ClientHandler cm) : base(cm)
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
                        OnLoginStart(raw.Data);
                        break;
                    }
                case 0x01:
                    {
                        OnEncryptionResponse(raw.Data);
                        break;
                    }
            }
        }

        private void OnEncryptionResponse(byte[] data)
        {
            MemoryStream ms = new(data);

            var lea = new LoginEncryptionResponse();
            ClientMaintainer.Protocol.SLoginEncryptionResponse.DeserializeLess(ms, lea);
            ms.Close();
        }

        private void OnLoginStart(byte[] data)
        {
            MemoryStream ms = new(data);
            var loginstart = new LoginStart();
            ClientMaintainer.Protocol.SLoginStart.DeserializeLess(ms, loginstart);
            ms.Close();
            Console.WriteLine("Username=" + loginstart.Username);


            if (!ClientMaintainer.VClient.ServerInstance.ServerIntergrate.HandleState(out ChatBase cb))
            {
                var response = new Response()
                {
                    Json = cb.Serialize(),
                };
                ClientMaintainer.Protocol.SResponse.Serialize(ClientMaintainer.VClient.RemoteStream, response);
                return;
            }

            var loginSuccess = new LoginSuccess()
            {
                Guid = Guid.NewGuid(),
                Username = loginstart.Username
            };

            ClientMaintainer.Protocol.SLoginSuccess.Serialize(ClientMaintainer.VClient.RemoteStream, loginSuccess);

            ClientMaintainer.ChangeState(NetworkState.Play); // тут кароч рано писать пероход на игру но можно и так а так надо понять как робит шифрование
        }
    }
}
