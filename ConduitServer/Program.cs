using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World! From Conduit Core");

        TcpListener tcpListener = new TcpListener(IPAddress.Any, 25565);
        tcpListener.Start();
        Console.WriteLine("Server started");

        var client = tcpListener.AcceptTcpClient();
        Console.WriteLine("Someone Connected");
        var stream = client.GetStream();

        Console.WriteLine("[Read First Packet]");
        Console.WriteLine(ReadVarInt(stream));
        Console.WriteLine(ReadVarInt(stream));
        Console.WriteLine(ReadVarInt(stream));
        Console.WriteLine(ReadString(stream));
        Console.WriteLine(ReadPort(stream));
        Console.WriteLine(ReadVarInt(stream));
        Console.WriteLine("[Read Second Packet]");
        Console.WriteLine(ReadVarInt(stream));
        Console.WriteLine(ReadVarInt(stream));
        Console.WriteLine(ReadString(stream));
    }

    static int ReadVarInt(Stream stream)
    {
        var value = 0;
        var size = 0;
        int b;
        while (((b = stream.ReadByte()) & 0x80) == 0x80)
        {
            value |= (b & 0x7F) << (size++ * 7);
            if (size > 5)
            {
                throw new IOException("VarInt too long. Hehe that's punny.");
            }
        }
        return value | ((b & 0x7F) << (size * 7));
    }
    static string ReadString(Stream stream)
    {
        var length = ReadVarInt(stream);
        var stringValue = Read(length, stream);

        return Encoding.UTF8.GetString(stringValue);
    }
    static ushort ReadUShort(Stream stream)
    {
        var da = Read(2, stream);
        return NetworkToHostOrder(BitConverter.ToUInt16(da, 0));
    }//read port
    static byte[] Read(int length, Stream stream)
    {
        var buffered = new List<byte>();
        for(int i = 0; i < length; i++)
        {
            buffered.Add((byte)stream.ReadByte());
        }
        return buffered.ToArray();
    }
    static ushort NetworkToHostOrder(ushort network)
    {
        var net = BitConverter.GetBytes(network);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(net);
        return BitConverter.ToUInt16(net, 0);
    }
    static ushort ReadPort(Stream stream)
    {
        var bytes = Read(2, stream);
        int port = bytes[1] | (bytes[0] << 8);

        return (ushort)port;
    }
}