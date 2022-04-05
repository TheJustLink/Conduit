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
        Console.WriteLine("Packet Length: " + ReadVarInt(stream));
        Console.WriteLine("Packet ID: " + ReadVarInt(stream));
        Console.WriteLine("Protocol Version: " + ReadVarInt(stream));
        Console.WriteLine("Server Adress: " + ReadString(stream));
        Console.WriteLine("Server Port: " + ReadPort(stream));
        Console.WriteLine("Next State: " + ReadVarInt(stream));

        Console.WriteLine("[Read Second Packet]");
        Console.WriteLine("Packet Length: " + ReadVarInt(stream));
        Console.WriteLine("Packet ID: " + ReadVarInt(stream));
        //Console.WriteLine("Username: " + ReadString(stream));
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
                throw new IOException("VarInt too long.");
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
    static ushort ReadPort(Stream stream)
    {
        var bytes = Read(2, stream);
        int port = bytes[1] | (bytes[0] << 8);

        return (ushort)port;
    }
    static byte[] Read(int length, Stream stream)
    {
        var buffered = new List<byte>();
        for (int i = 0; i < length; i++)
        {
            buffered.Add((byte)stream.ReadByte());
        }
        return buffered.ToArray();
    }

    static void WriteVarInt(int integer, out List<byte> buffer)
    {
        buffer = new List<byte>();
        while ((integer & -128) != 0)
        {
            buffer.Add((byte)(integer & 127 | 128));
            integer = (int)(((uint)integer) >> 7);
        }
        buffer.Add((byte)integer);
    }
    static int ReadVarInt(List<byte> buffer)
    {
        var value = 0;
        var size = 0;
        int b;
        while (((b = buffer[size]) & 0x80) == 0x80)
        {
            value |= (b & 0x7F) << (size++ * 7);
            if (size > 5)
            {
                throw new IOException("VarInt too long.");
            }
        }
        return value | ((b & 0x7F) << (size * 7));
    }
    static void WriteString(string str, out List<byte> buffer)
    {
        buffer = new List<byte>();
        byte[] data = Encoding.UTF8.GetBytes(str);
        WriteVarInt(data.Length, out buffer);
        foreach(var b in data)
        {
            buffer.Add(b);
        }
    }
}