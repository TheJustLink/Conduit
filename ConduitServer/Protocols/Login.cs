using System;
using System.Linq;

using Conduit.Net.Protocols;
using Conduit.Net.Connection;
using Conduit.Net.Extensions;
using Conduit.Net.IO.Encryption;
using Conduit.Net.Protocols.Flow;
using Conduit.Net.Packets.Login.Clientbound;
using Conduit.Net.Packets.Login.Serverbound;

namespace Conduit.Server.Protocols
{
    public class Login : ServerAutoProtocol<Login, LoginFlow>
    {
        private const bool LicenceMode = false;
        private const int CompressionTreshold = 256;

        private byte[] _verifyEncryptionToken;

        public Login(State state, IConnection connection) : base(state, connection) { }

        public void Handle(Start start)
        {
            if (LicenceMode) SendEncryptionRequest();

            Connection.Send(new SetCompression { Treshold = CompressionTreshold });
            Connection.AddCompression(CompressionTreshold);

            Connection.Send(new Success
            {
                Username = start.Username,
                Guid = start.Username.GetGuid()
            });

            State.Switch(new Play(State, Connection));
        }
        public void Handle(EncryptionResponse response)
        {
            var sharedSecret = Rsa.Decrypt(response.SharedSecret);
            var verifyTokenResponse = Rsa.Decrypt(response.VerifyToken);

            if (_verifyEncryptionToken.SequenceEqual(verifyTokenResponse) == false)
                throw new ArgumentException("Invalid verify token");

            Connection.AddEncryption(sharedSecret);
        }

        private void SendEncryptionRequest()
        {
            _verifyEncryptionToken = Random.Shared.NextBytes(4);

            Connection.Send(new EncryptionRequest
            {
                ServerId = "",
                PublicKey = Rsa.PublicKey,
                VerifyToken = _verifyEncryptionToken
            });
        }
    }
}