using Conduit.Network.JSON.Chat;
using Conduit.Network.Protocol.Serializable;
using Conduit.Network.Protocol.Serializable.Logging;
using Conduit.Network.Protocol.Serializable.Play.Server;
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
            if (!ClientHandler.VClient.RemoteStream.DataAvailable)
                return;

            RawPacket raw = ClientHandler.Protocol.SRawPacket.PacketPool.Get();
            ClientHandler.Protocol.SRawPacket.Serializator.DeserializeBigDataOffset(ClientHandler.VClient.RemoteStream, raw);

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

            ClientHandler.Protocol.SRawPacket.PacketPool.Return(raw);
        }

        private void OnEncryptionResponse(byte[] data)
        {
            MemoryStream ms = new(data);

            var lea = new LoginEncryptionResponse();
            ClientHandler.Protocol.SLoginEncryptionResponse.Serializator.DeserializeLess(ms, lea);
            ms.Close();
        }

        private void OnLoginStart(byte[] data)
        {
            MemoryStream ms = new(data);
            var loginstart = new LoginStart();
            ClientHandler.Protocol.SLoginStart.Serializator.DeserializeLess(ms, loginstart);
            ms.Close();
            //Console.WriteLine("Username=" + loginstart.Username);

            if (!ClientHandler.VClient.ServerInstance.ServerIntergrate.HandleState(out string mes))
            {
                var response = new Response()
                {
                    Json = mes,
                };
                ClientHandler.Protocol.SResponse.Serializator.Serialize(ClientHandler.VClient.RemoteStream, response);

                ShutdownClient();
                return;
            }

            var loginSuccess = new LoginSuccess()
            {
                UUID = Guid.NewGuid(),
                Username = loginstart.Username
            };

            ClientHandler.Protocol.SLoginSuccess.Serializator.Serialize(ClientHandler.VClient.RemoteStream, loginSuccess);

            ClientHandler.ChangeState(NetworkState.Play); // тут кароч рано писать пероход на игру но можно и так а так надо понять как робит шифрование
        }
    }
}
