using System;
using System.Security.Cryptography;

using Conduit.Net;
using Conduit.Net.Protocols;
using Conduit.Net.Extensions;
using Conduit.Net.Connection;
using Conduit.Net.Packets.Login.Clientbound;
using Conduit.Net.Packets.Login.Serverbound;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Client.Protocols
{
    public class Login : ClientAutoProtocol<Login, LoginFlow>, IStarteable
    {
        public Login(State state, IConnection connection) : base(state, connection) { }

        public void Start() => Connection.Send(new Start());

        public void Handle(Disconnect disconnect)
        {
            Console.WriteLine("Disconnected, reason - " + disconnect.Reason);
            Connection.Disconnect();
        }
        public void Handle(EncryptionRequest encryption)
        {
            var sharedKey = Random.Shared.NextBytes(16);

            using var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(encryption.PublicKey, out _);

            var encryptedSharedKey = rsa.Encrypt(sharedKey, RSAEncryptionPadding.Pkcs1);
            var encryptedVerifyToken = rsa.Encrypt(encryption.VerifyToken, RSAEncryptionPadding.Pkcs1);

            Connection.Send(new EncryptionResponse
            {
                SharedSecret = encryptedSharedKey,
                VerifyToken = encryptedVerifyToken
            });
            Connection.AddEncryption(sharedKey);
        }
        public void Handle(Success success) => State.Switch(new Play(State, Connection));
        public void Handle(SetCompression compression) => Connection.AddCompression(compression.Treshold);
        public void Handle(PluginRequest pluginRequest) => throw new NotImplementedException();
    }
}