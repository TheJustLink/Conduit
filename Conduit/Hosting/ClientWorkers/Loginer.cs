using Conduit.Network.Protocol.Serializable;
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

        public override void Handling()
        {
            WaitToAvailable();

            Packet onlyheader = new Packet();
            ClientMaintainer.Protocol.SPacket.Deserialize(ClientMaintainer.VClient.NetworkStream, onlyheader);

            switch (onlyheader.Id)
            {
                default:
                    {
                        OnLoginStart(onlyheader.Length - 1);
                        break;
                    }
                case 0x01:
                    {
                        OnEncryptionResponse(onlyheader.Length - 1);
                        break;
                    }
            }

        }

        private void OnEncryptionResponse(int len)
        {
            MemoryStream ms = ReadToStream(len);

            var lea = new LoginEncryptionResponse();
            ClientMaintainer.Protocol.SLoginEncryptionResponse.DeserializeLess(ms, lea);

        }

        private void OnLoginStart(int len)
        {
            MemoryStream ms = ReadToStream(len);
            var loginstart = new LoginStart();
            ClientMaintainer.Protocol.SLoginStart.DeserializeLess(ms, loginstart);
            Console.WriteLine("Username=" + loginstart.Username);

            var loginSuccess = new LoginSuccess()
            {
                Guid = Guid.NewGuid(),
                Username = loginstart.Username
            };

            ClientMaintainer.Protocol.SLoginSuccess.Serialize(ClientMaintainer.VClient.NetworkStream, loginSuccess);

            ClientMaintainer.ChangeState(NetworkState.Play); // тут кароч рано писать пероход на игру но можно и так а так надо понять как робит шифрование
        }
    }
}
