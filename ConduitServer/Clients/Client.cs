using System;
using System.Diagnostics;
using System.Threading;

using ConduitServer.Clients;
using ConduitServer.Net.Packets;
using ConduitServer.Net.Packets.Handshake;
using ConduitServer.Net.Packets.Login;
using ConduitServer.Net.Packets.Play;
using ConduitServer.Net.Packets.Status;

using LoginDisconnect = ConduitServer.Net.Packets.Login.Disconnect;

namespace ConduitServer
{
    abstract class Client : IClient
    {
        private ClientState _state;

        private readonly IPacketProvider _packetProvider;
        private readonly IPacketSender _packetSender;

        private int _protocolVersion;

        protected Client(IPacketProvider packetProvider, IPacketSender packetSender)
        {
            _packetProvider = packetProvider;
            _packetSender = packetSender;
        }
        
        public void Tick()
        {
            while (true)
            {
                switch (_state)
                {
                    case ClientState.Handshaking: HandshakingState(); break;
                    case ClientState.Status: StatusState(); break;
                    case ClientState.Login: LoginState(); break;
                    case ClientState.Play: PlayState(); break;
                    default:
                    case ClientState.Disconnected: Disconnect(); return;
                }
                Thread.Sleep(1);
            }
        }

        private void HandshakingState()
        {
            var handshake = _packetProvider.Read<Handshake>();

            Console.WriteLine("Get packet:");
            Console.WriteLine($"[{handshake.Id}](length={handshake.Length})");
            Console.WriteLine("HandShake");
            Console.WriteLine("ProtocolVerision=" + handshake.ProtocolVersion);
            Console.WriteLine("ServerAddress=" + handshake.ServerAddress);
            Console.WriteLine("ServerPort=" + handshake.ServerPort);
            Console.WriteLine("NextState=" + handshake.NextState);

            _protocolVersion = handshake.ProtocolVersion;
            _state = handshake.NextState;
        }
        private void StatusState()
        {
            _packetProvider.Read<Request>();

            Console.WriteLine();
            Console.WriteLine("Get packet:");
            Console.WriteLine("Request");

            var statusText = @"{""version"": {""name"": ""Hell server 1.18"",""protocol"": " + _protocolVersion + @"},""players"": {""max"": 666,""online"": 99}}";
            var response = new Response
            {
                Json = statusText
            };

            var sw = Stopwatch.StartNew();
            _packetSender.Send(response);
            sw.Stop();

            Console.WriteLine("Serialization Time: " + sw.Elapsed.TotalMilliseconds);

            var ping = _packetProvider.Read<Ping>();
            _packetSender.Send(ping);

            Console.WriteLine();
            Console.WriteLine("Get packet:");
            Console.WriteLine($"[{ping.Id}](length={ping.Length})");
            Console.WriteLine("Ping");
            Console.WriteLine("Payload=" + ping.Payload);

            _state = ClientState.Disconnected;
        }
        private void LoginState()
        {
            var loginStart = _packetProvider.Read<Start>();

            Console.WriteLine("Get packet:");
            Console.WriteLine($"[{loginStart.Id}](length={loginStart.Length})");
            Console.WriteLine("Username=" + loginStart.Username);

            var loginSuccess = new Success
            {
                Guid = Guid.NewGuid(),
                Username = loginStart.Username
            };
            _packetSender.Send(loginSuccess);

            // If login failed - disconnect
            //var disconnect = new LoginDisconnect
            //{
            //    Reason = new Chat { Text  = "ABOBUS!" }
            //};
            //_packetSender.Send(disconnect);

            _state = ClientState.Play;
        }

        private void PlayState()
        {
            var joinGame = new JoinGame()
            {
                EntityId = 0,
                IsHardcore = false,
                Gamemode = Gamemode.Adventure,
                PreviousGamemode = -1,
                WorldCount = 1,
                DimensionNames = new [] { "minecraft:overworld" },
                // DimCodec
                // Dim
                HashedSeed = 0,
                MaxPlayers = 100,
                ViewDistance = 2,
                SimulationDistance = 2,
                EnableRespawnScreen = true,
                IsDebug = true,
                IsFlat = true
            };
            _packetSender.Send(joinGame);
        }

        protected abstract void Disconnect();
    }
}