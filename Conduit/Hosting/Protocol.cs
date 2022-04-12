using Conduit.Network.Protocol;
using Conduit.Network.Protocol.Serializable;
using Conduit.Network.Protocol.Serializable.Logging;
using Conduit.Network.Protocol.Serializable.Play.Server;
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
        //public PacketTool<Packet> SPacket { get; private set; }

        public PacketTool<Handshake> SHandshake { get; private set; }
        
        public PacketTool<Ping> SPing { get; private set; }
        public PacketTool<Request> SRequest { get; private set; }
        public PacketTool<Response> SResponse { get; private set; }
        public PacketTool<RawPacket> SRawPacket { get; private set; }

        public PacketTool<LoginStart> SLoginStart { get; private set; }
        public PacketTool<LoginSuccess> SLoginSuccess { get; private set; }
        public PacketTool<LoginEncryptionResponse> SLoginEncryptionResponse { get; private set; }
        public PacketTool<LoginEncryptionRequest> SLoginEncryptionRequest { get; private set; }

        public PacketTool<SpawnPlayer> SSpawnPlayer { get; private set; }
        public Protocol()
        {
            //SPacket = new PacketTool<Packet>();

            SHandshake = new PacketTool<Handshake>();

            SPing = new PacketTool<Ping>();

            SRequest = new PacketTool<Request>();

            SResponse = new PacketTool<Response>();

            SRawPacket = new PacketTool<RawPacket>() 
            { Serializator = new Serializator<RawPacket>(true) };

            SLoginStart = new PacketTool<LoginStart>();

            SLoginSuccess = new PacketTool<LoginSuccess>();

            SLoginEncryptionResponse = new PacketTool<LoginEncryptionResponse>() 
            { Serializator = new Serializator<LoginEncryptionResponse>(true)};

            SLoginEncryptionRequest = new PacketTool<LoginEncryptionRequest>() 
            { Serializator = new Serializator<LoginEncryptionRequest>(true)};
            
            SSpawnPlayer = new PacketTool<SpawnPlayer>();
        }
    }
}
