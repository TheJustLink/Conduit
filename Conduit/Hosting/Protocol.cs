using Conduit.Network.Protocol.Serializable;
using Conduit.Network.Protocol.Serializable.Logging;
using Conduit.Network.Protocol.Serializable.Status;
using Conduit.Network.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Hosting
{
    public sealed class Protocol
    {
        public Serializator<Packet> SPacket { get; private set; }

        public Serializator<Handshake> SHandshake { get; private set; }
        
        public Serializator<Ping> SPing { get; private set; }
        public Serializator<Request> SRequest { get; private set; }
        public Serializator<Response> SResponse { get; private set; }
        public Serializator<RawPacket> SRawPacket { get; private set; }

        public Serializator<LoginStart> SLoginStart { get; private set; }
        public Serializator<LoginSuccess> SLoginSuccess { get; private set; }
        public Serializator<LoginEncryptionResponse> SLoginEncryptionResponse { get; private set; }
        public Serializator<LoginEncryptionRequest> SLoginEncryptionRequest { get; private set; }

        public Protocol()
        {
            SPacket = new Serializator<Packet>();

            SHandshake = new Serializator<Handshake>();

            SPing = new Serializator<Ping>();
            SRequest = new Serializator<Request>();
            SResponse = new Serializator<Response>();
            SRawPacket = new Serializator<RawPacket>(true);

            SLoginStart = new Serializator<LoginStart>();
            SLoginSuccess = new Serializator<LoginSuccess>();
            SLoginEncryptionResponse = new Serializator<LoginEncryptionResponse>(true);
            SLoginEncryptionRequest = new Serializator<LoginEncryptionRequest>(true);
        }
    }
}
