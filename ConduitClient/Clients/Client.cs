using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;

using Conduit.Net.Data;
using Conduit.Net.IO.Packet;
using Conduit.Net.Packets;
using Conduit.Net.Packets.Handshake;
using Conduit.Net.Packets.Login;
using Conduit.Net.Packets.Play;
using Conduit.Net.Packets.Status;

using Disconnect = Conduit.Net.Packets.Login.Disconnect;

namespace Conduit.Client.Clients
{
    abstract class Client : IClient
    {
        public abstract string Host { get; }
        public abstract int Port { get; }
        public abstract bool IsConnected { get; }

        private readonly IReader _packetReader;
        private readonly IWriter _packetWriter;

        private readonly ReaderFactory _packetReaderFactory;
        private readonly WriterFactory _packetWriterFactory;
        
        protected Client(ReaderFactory packetReaderFactory, WriterFactory packetWriterFactory)
        {
            _packetReaderFactory = packetReaderFactory;
            _packetWriterFactory = packetWriterFactory;

            _packetReader = packetReaderFactory.Create();
            _packetWriter = packetWriterFactory.Create();
        }

        public void CheckServerState()
        {
            SendHandshake(ClientState.Status);
            _packetWriter.Write(new Request());

            var server = _packetReader.Read<Response>().Server;
            var ping = GetPing();

            Console.WriteLine("Server info:\n" + JsonSerializer.Serialize(server, new JsonSerializerOptions { IncludeFields = true }));
            Console.WriteLine("Ping ms " + ping.TotalMilliseconds);

            Disconnect();
        }
        public void JoinGame(string username)
        {
            Console.WriteLine("Start join game");

            SendHandshake(ClientState.Login);
            Login(username);

            var joinGame = _packetReader.Read<JoinGame>();
            Console.WriteLine("End join game");

            ThreadLoop();
        }
        private void ThreadLoop()
        {
            //var message = new ChatMessage { Message = "/reg 1234 1234" };
            //var message = new ChatMessage { Message = "/server Lobby" };
            //_packetWriter.Write(message);

            var messages = new string[]
            {
                    "ABOBUS в массы?",
                    "Что такое ABOBUS?",
                    "Где живёт ABOBUS?",
                    "Я знаю кто ABOBUS!",
                    "ABOBUS это законно?",
                    "Привет, я новичок, из клана ABOBUS",
                    "Кто-то хочет ABOBUS?",
                    "Да -_-",
                    "Да",
                    "Да!",
                    "Нет",
                    "Нет)",
                    "Да кто пишет этих ABOBUSов?!",
                    "Я гей",
                    "Я не гей",
                    "Я люблю Сеперу!!!!",
                    "Качанов мой качан!",
                    "Я люблю члены, а вы?",
                    "Привет!",
                    "Как у кого дела, ребятки?",
                    "Как у кого дела, ребята?",
                    "ABOBUS - ВЕЛИКАЯ НАЦИЯ!!!",
                    "ABOBUS - НАЦИЯ ДЕБИЛОВ!",
                    "Ты быкуешь?",
                    "Я люблю ABOBUS",
                    "Мой повелитель ABOB-ии!",
                    "У нас страна возможностей, ABOBUS Лэндия!",
                    "Пахнет жаренным...",
                    "Ща всё починим",
                    "Я не виноват",
                    "Здесь дурно пахнет",
                    "-_-",
                    "0_0",
                    "Кто это?",
                    "Да ты соска",
                    "Где мои консервы?",
                    "Скоро я попаду в ABOBA land?",
                    "Где качан, хочу есть",
                    "Наливай",
                    "Ну что, поехали)",
                    "Я уважаю всех, но ты лох",
                    "Где-то я потерялся..",
                    "Кто-то их знает?",
                    "Я хочу ABOBUS",
                    "abobus",
                    "AbObUs",
                    "4B0B4S",
                    "Я за абобусов",
                    "Свободу абобо-головым!",
                    "Abobus-ы тоже люди!!!",
                    "КОГДА ВЫ ПРИЗНАЕТЕ КЛАН АБОБУСОВ?",
                    "АБОБУСЫ - ЛЮДИ!",
                    "Кто-то здесь ABOBUS?",
                    "Как достали эти абобусы",
                    "Я уважаю абобусов... Всегда...",
                    "ABOBUS FOREVER"
            };

            while (true)
            {
                var packet = _packetReader.Read();
                Console.WriteLine($"IN [{packet.Id}]");

                switch (packet.Id)
                {
                    case 0x21:
                        var keepAlive = _packetReader.Read<KeepAlive>(packet);
                        _packetWriter.Write(keepAlive.ToResponse());
                        break;
                    case 0x18:
                        var pluginMessage = _packetReader.Read<PluginMessage>(packet);
                        Console.WriteLine($"Plugin message [{pluginMessage.Channel}]({pluginMessage.Data})");
                        break;
                    case 0x38:
                        //var position = _packetReader.Read<PlayerPositionAndRotation>();
                        //Console.WriteLine($"X:{position.X} Y:{position.Y} Z:{position.Z} Yaw:{position.Yaw} Pitch:{position.Pitch} TeleportId:{position.TeleportId} DismountVehicle:{position.DismountVehicle}");
                        break;
                    case 0x1A:
                        var disconnected = _packetReader.Read<Disconnect>(packet);
                        Console.WriteLine("Reason - " + disconnected.Reason.Text);
                        break;
                }
                var message = new ChatMessage { Message = messages[Random.Shared.Next(0, messages.Length)] };
                _packetWriter.Write(message);

                Thread.Sleep(1);
            }
        }

        public void Disconnect()
        {
            if (IsConnected)
                DisconnectInternal();
        }

        private void Login(string username)
        {
            _packetWriter.Write(new Start { Username = username });

            while (true)
            {
                var packet = _packetReader.Read();
                Console.WriteLine($"[{packet.Id}]");
                switch (packet.Id)
                {
                    case 0:
                        ReadLoginDisconnect(packet);
                        return;
                    case 1:
                        ReadEncryptionRequest(packet);
                        break;
                    case 2:
                        ReadLoginSuccess(packet);
                        return;
                    case 3:
                        ReadSetCompression(packet);
                        break;
                    case 4:
                        ReadPluginRequest(packet);
                        break;
                }
            }
        }
        private void ReadLoginDisconnect(RawPacket packet)
        {
            Console.WriteLine("DISCONNECTED");
            var disconnect = _packetReader.Read<Disconnect>(packet);
            Console.WriteLine("Disconnected, reason - " + disconnect.Reason.Text);
        }
        private void ReadLoginSuccess(RawPacket packet)
        {
            Console.WriteLine("SUCCESS");
            var success = _packetReader.Read<Success>(packet);
            Console.WriteLine($"Login success, username - [{success.Username}], guid - [{success.Guid}]");
        }
        private void ReadSetCompression(RawPacket packet)
        {
            Console.WriteLine("COMPRESSION");
            var compression = _packetReader.Read<SetCompression>(packet);
            Console.WriteLine("Compression - " + compression.Treshold);

            _packetReaderFactory.AddCompression(_packetReader);
            _packetWriterFactory.AddCompression(_packetWriter, compression.Treshold);
        }
        private void ReadEncryptionRequest(RawPacket packet)
        {
            var encryption = _packetReader.Read<EncryptionRequest>(packet);

            var sharedKey = new byte[16];
            Random.Shared.NextBytes(sharedKey);

            using var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(encryption.PublicKey, out _);

            var encryptedSharedKey = rsa.Encrypt(sharedKey, RSAEncryptionPadding.Pkcs1);
            var encryptedVerifyToken = rsa.Encrypt(encryption.VerifyToken, RSAEncryptionPadding.Pkcs1);

            var encryptionResponse = new EncryptionResponse
            {
                SharedSecret = encryptedSharedKey,
                VerifyToken = encryptedVerifyToken
            };
            _packetWriter.Write(encryptionResponse);

            _packetReaderFactory.AddEncryption(_packetReader, sharedKey);
            _packetWriterFactory.AddEncryption(_packetWriter, sharedKey);

            Console.WriteLine("Setup encryption");
        }
        private void ReadPluginRequest(RawPacket packet)
        {
            Console.WriteLine("PLUGIN REQUEST");
            var request = _packetReader.Read<PluginRequest>(packet);
            var response = new PluginResponse()
            {
                MessageId = request.MessageId,
                Successful = true,
                Data = new byte[0]
            };
            _packetWriter.Write(response);
            Console.WriteLine("Response sended");
        }

        private TimeSpan GetPing()
        {
            var payload = Random.Shared.NextInt64(long.MinValue, long.MaxValue);
            _packetWriter.Write(new Ping { Payload = payload });

            var stopwatch = Stopwatch.StartNew();
            var ping = _packetReader.Read<Ping>();
            stopwatch.Stop();

            if (ping.Payload != payload)
                Console.WriteLine($"Ping payload {payload} != server payload {ping.Payload}");

            return stopwatch.Elapsed;
        }
        private void SendHandshake(ClientState nextState)
        {
            var handshake = new Handshake
            {
                ProtocolVersion = 757,
                ServerAddress = Host,
                ServerPort = (ushort)Port,
                NextState = nextState
            };
            _packetWriter.Write(handshake);
        }

        protected abstract void DisconnectInternal();
    }
}