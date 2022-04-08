using System;
using System.Diagnostics;
using System.Net.Sockets;
using ConduitServer.Net.Packets.Handshake;
using ConduitServer.Net.Packets.Login;
using ConduitServer.Net.Packets.Status;
using ConduitServer.Serialization.Packets;

namespace ConduitServer
{
    internal class Client
    {
        public Action<Client> Disconnected;

        private TcpClient _tcpClient;

        public Client(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
        }

        public void ReadPacket()
        {
            var stream = _tcpClient.GetStream();
            if (!stream.DataAvailable) return;

            var deserializer = new PacketDeserializer();
            var serializer = new PacketSerializer();

            var sw = Stopwatch.StartNew();
            var handshake = deserializer.Deserialize<Handshake>(stream);
            sw.Stop();

            Console.WriteLine("Deserialization ms = " + sw.ElapsedMilliseconds);
            Console.WriteLine("Get packet:");
            Console.WriteLine($"[{handshake.Id}](length={handshake.Length})");
            Console.WriteLine("HandShake");
            Console.WriteLine("ProtocolVerision=" + handshake.ProtocolVersion);
            Console.WriteLine("ServerAddress=" + handshake.ServerAddress);
            Console.WriteLine("ServerPort=" + handshake.ServerPort);
            Console.WriteLine("NextState=" + handshake.NextState);

            if (handshake.NextState == 2) // Login
            {
                sw.Restart();
                var loginStart = deserializer.Deserialize<LoginStart>(stream);
                sw.Stop();

                Console.WriteLine();
                Console.WriteLine("Deserialization ms = " + sw.ElapsedMilliseconds);
                Console.WriteLine("Get packet:");
                Console.WriteLine($"[{loginStart.Id}](length={loginStart.Length})");
                Console.WriteLine("Username=" + loginStart.Username);

                var loginSuccess = new LoginSuccess()
                {
                    Guid = Guid.NewGuid(),
                    Username = loginStart.Username
                };

                sw.Restart();
                serializer.Serialize(stream, loginSuccess);
                sw.Stop();

                Console.WriteLine();
                Console.WriteLine("Serialization ms = " + sw.ElapsedMilliseconds);
            }
            else if (handshake.NextState == 1) // Status
            {
                sw.Restart();
                deserializer.Deserialize<Request>(stream);
                sw.Stop();

                Console.WriteLine();
                Console.WriteLine("Deserialization ms = " + sw.ElapsedMilliseconds);
                Console.WriteLine("Get packet:");
                Console.WriteLine("Request");

                var statusText = @"{""version"": {""name"": ""Hell server 1.18"",""protocol"": 757},""players"": {""max"": 666,""online"": 99}}";
                var response = new Response()
                {
                    Json = statusText
                };

                sw.Restart();
                serializer.Serialize(stream, response);
                sw.Stop();
                Console.WriteLine();
                Console.WriteLine("Serialization ms = " + sw.ElapsedMilliseconds);

                var ping = deserializer.Deserialize<Ping>(stream);

                Console.WriteLine();
                Console.WriteLine("Get packet:");
                Console.WriteLine($"[{ping.Id}](length={ping.Length})");
                Console.WriteLine("Ping");
                Console.WriteLine("Payload=" + ping.Payload);

                serializer.Serialize(stream, ping);
                Disconnected?.Invoke(this);
            }
        }
    }
}
