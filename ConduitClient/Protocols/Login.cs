using System;
using System.Text;
using System.Security.Cryptography;

using Conduit.Net;
using Conduit.Net.Packets;
using Conduit.Net.Extensions;
using Conduit.Net.Protocols;
using Conduit.Net.Protocols.Flow;
using Conduit.Net.Packets.Login.Clientbound;
using Conduit.Net.Packets.Login.Serverbound;

namespace Conduit.Client.Protocols
{
    public class Login : ClientAutoProtocol<Login, LoginFlow>, IStarteable
    {
        public void Start() => Connection.Send(new Start { Username = "Cucumber" });

        public void Handle(Disconnect disconnect)
        {
            Console.WriteLine($"Disconnected, reason - {disconnect.Reason}");
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

            Console.WriteLine("Encryption enabled");
        }
        public void Handle(Success success)
        {
            Console.WriteLine($"Login success, username - [{success.Username}], guid - [{success.Guid}]");

            State.Switch<Play>();
        }
        public void Handle(SetCompression compression)
        {
            Connection.AddCompression(compression.Treshold);

            Console.WriteLine($"Compression enabled, treshold = {compression.Treshold}");
        }
        public void Handle(PluginRequest request)
        {
            Console.WriteLine($"Plugin request - [{request.Channel}] : [{Encoding.UTF8.GetString(request.Data)}]");

            Connection.Send(new PluginResponse
            {
                MessageId = request.MessageId,
                Successful = true,
                Data = Array.Empty<byte>()
            });
        }
    }
}