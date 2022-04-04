using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;

static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Server started");

        var listener = new TcpListener(IPAddress.Any, 666);
        listener.Start();

        var client = listener.AcceptTcpClient();
        Console.WriteLine($"Client {client.Client.LocalEndPoint} connected");

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
        var countInPacket = ReadVarInt(stream);
        var packetId = ReadVarInt(stream);
        var protocolVersion = ReadVarInt(stream);
        var serverAddress = ReadVarString(stream);
        var serverPort = ReadUShort(stream);
        var nextState = ReadVarInt(stream);
        
        Console.WriteLine("Get packet:");
        Console.WriteLine($"[{packetId}](length={countInPacket})");
        Console.WriteLine("HandShake");
        Console.WriteLine("ProtocolVerision=" + protocolVersion);
        Console.WriteLine("ServerAddress=" + serverAddress);
        Console.WriteLine("ServerPort=" + serverPort);
        Console.WriteLine("NextState=" + nextState);

        Console.WriteLine();

        if (nextState == 2)
        {
            countInPacket = ReadVarInt(stream);
            packetId = ReadVarInt(stream);
            var username = ReadVarString(stream);

            Console.WriteLine("Get packet:");
            Console.WriteLine($"[{packetId}](length={countInPacket})");
            Console.WriteLine("Username=" + username);
        }
    }

    private static byte[] ReceiveData(TcpClient client)
    {
        using var networkStream = client.GetStream();
        var buffer = new byte[client.Available];
        networkStream.Read(buffer);

        Console.WriteLine("Received message: ");
        foreach (var data in buffer)
            Console.Write(data + ", ");

        var message = Encoding.UTF8.GetString(buffer);
        Console.WriteLine($"({message})");

        return buffer;
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