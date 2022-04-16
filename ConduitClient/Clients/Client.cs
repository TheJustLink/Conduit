using System;
using System.Diagnostics;
using System.Text.Json;

using Conduit.Net.Data;
using Conduit.Net.IO.Packet;
using Conduit.Net.Packets.Handshake;
using Conduit.Net.Packets.Login;
using Conduit.Net.Packets.Play;
using Conduit.Net.Packets.Status;
using fNbt;
using Disconnect = Conduit.Net.Packets.Login.Disconnect;

namespace Conduit.Client.Clients
{
    abstract class Client : IClient
    {
        public abstract string Host { get; }
        public abstract int Port { get; }
        public abstract bool IsConnected { get; }

        private IReader _packetReader;
        private IWriter _packetWriter;

        private readonly IReaderFactory _packetReaderFactory;
        private readonly IWriterFactory _packetWriterFactory;
        
        protected Client(IReaderFactory packetReaderFactory, IWriterFactory packetWriterFactory)
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
            SendHandshake(ClientState.Login);
            Login(username);

            var joinGame = _packetReader.Read<JoinGame>();
            var dimensionCodecFile = new NbtFile(joinGame.DimensionCodec);
            var dimensionFile = new NbtFile(joinGame.Dimension);

            dimensionCodecFile.SaveToFile("DimensionCodec.nbt", NbtCompression.None);
            dimensionFile.SaveToFile("Dimension.nbt", NbtCompression.None);

            Disconnect();
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
                switch (packet.Id)
                {
                    case 0:
                        Console.WriteLine("DISCONNECTED");
                        var disconnect = _packetReader.Read<Disconnect>(packet);
                        Console.WriteLine("Disconnected, reason - " + disconnect);
                        return;
                    case 2:
                        Console.WriteLine("SUCCESS");
                        var success = _packetReader.Read<Success>(packet);
                        Console.WriteLine($"Login success, username - [{success.Username}], guid - [{success.Guid}]");
                        return;
                    case 3:
                        Console.WriteLine("COMPRESSION");
                        var compression = _packetReader.Read<SetCompression>(packet);
                        Console.WriteLine("Compression - " + compression.Treshold);

                        _packetReader = _packetReaderFactory.CreateWithCompression();
                        _packetWriter = _packetWriterFactory.CreateWithCompression(compression.Treshold);
                        break;
                }
            }
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