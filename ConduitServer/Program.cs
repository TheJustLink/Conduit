using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

using ConduitServer.Net.Packets.Handshake;
using ConduitServer.Net.Packets.Login;
using ConduitServer.Net.Packets.Status;
using ConduitServer.Serialization.Packets;
using ConduitServer.Net;

namespace ConduitServer
{
    static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Server started");

            Server server = new Server(666);
            server.Start();

            /*
            var listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();

            var client = listener.AcceptTcpClient();
            Console.WriteLine($"Client {client.Client.LocalEndPoint} connected");

            var countBytes = client.Available;
            Console.WriteLine($"Received {countBytes} bytes");

            ReceiveClient(client);
            */
        }

        private static void ReceiveClient(TcpClient client)
        {
            using var stream = client.GetStream();

            ReadPacket(stream);

            client.Close();
        }

        private static void ReadPacket(Stream stream)
        {
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
            }
        }

        private static void WriteVarString(Stream stream, string data)
        {
            var stringData = Encoding.UTF8.GetBytes(data);

            WriteVarInt(stream, stringData.Length);
            stream.Write(stringData);
        }
        private static void WriteVarInt(Stream stream, int number)
        {
            const int countBitsInSegment = 7;
            const int segmentMask = 0x7F;
            const int continueBitMask = 0x80;

            while ((number & ~segmentMask) != 0)
            {
                stream.WriteByte((byte)((number & segmentMask) | continueBitMask));
                number = (int)((uint)number >> countBitsInSegment);
            }
            stream.WriteByte((byte)number);
        }
        private static void WriteVarLong(Stream stream, long number)
        {
            const int countBitsInSegment = 7;
            const int segmentMask = 0x7F;
            const int continueBitMask = 0x80;

            while ((number & ~segmentMask) != 0)
            {
                stream.WriteByte((byte)((number & segmentMask) | continueBitMask));
                number = (long)((ulong)number >> countBitsInSegment);
            }
            stream.WriteByte((byte)number);
        }

        private static void WriteUUID(Stream stream, Guid guid)
        {
            stream.Write(guid.ToByteArray());
        }
        private static void WriteLong(Stream stream, long number)
        {
            stream.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(number)));
        }

        private static ushort ReadUShort(Stream stream)
        {
            var byte1 = stream.ReadByte();
            var byte2 = stream.ReadByte();

            return (ushort)(byte2 | (byte1 << 8));
        }
        private static ushort ReadUShort2(Stream stream)
        {
            var buffer = new byte[2];
            stream.Read(buffer);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);

            return BitConverter.ToUInt16(buffer);
        }
        private static long ReadLong(Stream stream)
        {
            var buffer = new byte[8];

            stream.Read(buffer);

            return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(buffer, 0));
        }

        private static string ReadVarString(Stream stream)
        {
            var length = ReadVarInt(stream);
            var buffer = new byte[length];

            stream.Read(buffer);

            return Encoding.UTF8.GetString(buffer);
        }
        private static int ReadVarInt(Stream stream)
        {
            const int countBitsInSegment = 7;
            const int segmentMask = 0x7F;
            const int continueBitMask = 0x80;

            int value = 0;
            int size = 0;
            int numberSegment;

            while (((numberSegment = stream.ReadByte()) & continueBitMask) == continueBitMask)
            {
                value |= (numberSegment & segmentMask) << (size++ * countBitsInSegment);

                if (size > 5)
                    throw new IOException("VarInt too long");
            }

            return value | ((numberSegment & segmentMask) << (size * countBitsInSegment));
        }
        private static int ReadVarLong(Stream stream)
        {
            const int countBitsInSegment = 7;
            const int segmentMask = 0x7F;
            const int continueBitMask = 0x80;

            int value = 0;
            int size = 0;
            int numberSegment;

            while (((numberSegment = stream.ReadByte()) & continueBitMask) == continueBitMask)
            {
                value |= (numberSegment & segmentMask) << (size++ * countBitsInSegment);

                if (size > 10)
                    throw new IOException("VarLong too long");
            }

            return value | ((numberSegment & segmentMask) << (size * countBitsInSegment));
        }
    }
}