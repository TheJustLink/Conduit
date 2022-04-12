using System;
using System.Diagnostics;
using System.Threading;
using ConduitServer.Clients;
using ConduitServer.Net.Packets;
using ConduitServer.Net.Packets.Handshake;
using ConduitServer.Net.Packets.Login;
using ConduitServer.Net.Packets.Status;

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
            switch (_state)
            {
                case ClientState.Handshaking: HandshakingState(); break;
                case ClientState.Status: StatusState(); break;
                case ClientState.Login: LoginState(); break;
                case ClientState.Play:
                default:
                case ClientState.Disconnected: Disconnect(); break;
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
            var loginStart = _packetProvider.Read<LoginStart>();

            Console.WriteLine("Get packet:");
            Console.WriteLine($"[{loginStart.Id}](length={loginStart.Length})");
            Console.WriteLine("Username=" + loginStart.Username);

            var loginSuccess = new LoginSuccess
            {
                Guid = Guid.NewGuid(),
                Username = loginStart.Username
            };
            _packetSender.Send(loginSuccess);

            _state = ClientState.Play;
        }
        
        protected abstract void Disconnect();
    }
}