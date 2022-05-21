using Conduit.Net.Data;
using Conduit.Net.Packets.Login.Clientbound;
using Conduit.Net.Packets.Login.Serverbound;

namespace Conduit.Net.Protocols.Flow
{
    public class LoginFlow : ProtocolFlow<LoginFlow>
    {
        private static readonly TypeMap s_clientboundMap = new(
            typeof(Disconnect),
            typeof(EncryptionRequest),
            typeof(Success),
            typeof(SetCompression),
            typeof(PluginRequest)
        );
        private static readonly TypeMap s_serverboundMap = new(
            typeof(Start),
            typeof(EncryptionResponse),
            typeof(PluginResponse)
        );

        public LoginFlow() : base(s_clientboundMap, s_serverboundMap) { }
    }
}