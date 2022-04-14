using System;
using System.Diagnostics;
using System.Text.Json;

using Conduit.Net.Data;
using Conduit.Net.IO.Packet;
using Conduit.Net.Packets.Handshake;
using Conduit.Net.Packets.Login;
using Conduit.Net.Packets.Status;

namespace Conduit.Client.Clients
{
    abstract class Client : IClient
    {
        public abstract string Host { get; }
        public abstract int Port { get; }
        public abstract bool IsConnected { get; }

        private readonly IPacketProvider _packetProvider;
        private readonly IPacketSender _packetSender;
        
        protected Client(IPacketProvider packetProvider, IPacketSender packetSender)
        {
            _packetProvider = packetProvider;
            _packetSender = packetSender;
        }

        public void CheckServerState()
        {
            SendHandshake(ClientState.Status);
            _packetSender.Send(new Request());

            var server = _packetProvider.Read<Response>().Server;
            var ping = GetPing();

            Console.WriteLine("Server info:\n" + JsonSerializer.Serialize(server, new JsonSerializerOptions { IncludeFields = true }));
            Console.WriteLine("Ping ms " + ping.TotalMilliseconds);

            Disconnect();
        }
        public void JoinGame(string username)
        {
            SendHandshake(ClientState.Login);
            Login(username);
        }
        public void Disconnect()
        {
            if (IsConnected)
                DisconnectInternal();
        }

        private void Login(string username)
        {
            _packetSender.Send(new Start { Username = username });
            var compression = _packetProvider.Read<SetCompression>();
            Console.WriteLine("Compression - " + compression.Treshold);

            var success = _packetProvider.Read<Success>();
            Console.WriteLine($"Login success, username - [{success.Username}], guid - [{success.Guid}]");
        }
        private TimeSpan GetPing()
        {
            var payload = Random.Shared.NextInt64(long.MinValue, long.MaxValue);
            _packetSender.Send(new Ping { Payload = payload });

            var stopwatch = Stopwatch.StartNew();
            var ping = _packetProvider.Read<Ping>();
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
            _packetSender.Send(handshake);
        }

        protected abstract void DisconnectInternal();
    }
}