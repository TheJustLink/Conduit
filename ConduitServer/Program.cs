using System;
using System.Buffers.Binary;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConduitServer
{
    static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Server started");

            var listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();

            var client = listener.AcceptTcpClient();
            Console.WriteLine($"Client {client.Client.LocalEndPoint} connected");

            Thread.Sleep(2000);

            var countBytes = client.Available;
            Console.WriteLine($"Received {countBytes} bytes");

            ReceiveClient(client);
        }
        private static void ReceiveClient(TcpClient client)
        {
            var stream = client.GetStream();

            ReadPacket(stream);

            client.Close();
        }

        private static void ReadPacket(Stream stream)
        {
            var reader = new BinaryReader(stream);
            var writer = new BinaryWriter(stream);

            //var countInPacket = ReadVarInt(stream);
            //var packetId = ReadVarInt(stream);
            //var protocolVersion = ReadVarInt(stream);
            //var serverAddress = ReadVarString(stream);
            //var serverPort = ReadUShort(stream);
            //var nextState = ReadVarInt(stream);
            var countInPacket = reader.Read7BitEncodedInt();
            var packetId = reader.Read7BitEncodedInt();
            var protocolVersion = reader.Read7BitEncodedInt();
            var serverAddress = reader.ReadString();
            var serverPort = BinaryPrimitives.ReverseEndianness(reader.ReadUInt16());
            var nextState = reader.Read7BitEncodedInt();

            Console.WriteLine("Get packet:");
            Console.WriteLine($"[{packetId}](length={countInPacket})");
            Console.WriteLine("HandShake");
            Console.WriteLine("ProtocolVerision=" + protocolVersion);
            Console.WriteLine("ServerAddress=" + serverAddress);
            Console.WriteLine("ServerPort=" + serverPort);
            Console.WriteLine("NextState=" + nextState);

            if (nextState == 2) // Login
            {
                countInPacket = ReadVarInt(stream);
                packetId = ReadVarInt(stream);
                var username = ReadVarString(stream);

                Console.WriteLine();
                Console.WriteLine("Get packet:");
                Console.WriteLine($"[{packetId}](length={countInPacket})");
                Console.WriteLine("Username=" + username);

                WriteVarInt(stream, 1 + 16 + Encoding.UTF8.GetBytes(username).Length);
                WriteVarInt(stream, 2);
                WriteUUID(stream, Guid.NewGuid());
                WriteVarString(stream, username);
            }
            else if (nextState == 1) // Status
            {
                countInPacket = ReadVarInt(stream);
                packetId = ReadVarInt(stream);

                Console.WriteLine();
                Console.WriteLine("Get packet:");
                Console.WriteLine($"[{packetId}](length={countInPacket})");
                Console.WriteLine("Request");

                var statusText2 = @"{""version"": {""name"": ""Hell server 1.18"",""protocol"": 757},""players"": {""max"": 666,""online"": 99}}";
                WriteVarInt(stream, 1 + 1 + Encoding.UTF8.GetBytes(statusText2).Length);
                WriteVarInt(stream, 0);
                WriteVarString(stream, statusText2);

                countInPacket = ReadVarInt(stream);
                packetId = ReadVarInt(stream);
                var pingPayload = ReadLong(stream);

                Console.WriteLine();
                Console.WriteLine("Get packet:");
                Console.WriteLine($"[{packetId}](length={countInPacket})");
                Console.WriteLine("Ping");
                Console.WriteLine("Payload=" + pingPayload);

                WriteVarInt(stream, 1 + 8);
                WriteVarInt(stream, 1);
                WriteLong(stream, pingPayload);
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