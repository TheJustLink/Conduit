using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ConduitServer;

static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World! From Conduit Core");

        Field field = new Field();
        Type type = field.GetType();
        var fields = type.GetFields();
        foreach(var f in fields)
        {
            Console.WriteLine(f.ToString());
        }
        Console.WriteLine("End");

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
        var nextState = ReadVarInt(stream);
        Console.WriteLine("Next State: " + nextState);

        if(nextState == 1)
        {
            Console.WriteLine("[Read Second Packet]");
            Console.WriteLine("Packet Length: " + ReadVarInt(stream));
            Console.WriteLine("Packet ID: " + ReadVarInt(stream));
            var data = new List<byte>();
            var result = new List<byte>();
            WriteVarInt(0, data);
            WriteString(@"{""version"": {""name"": ""1.18"",""protocol"": 757},""players"": {""max"": 666,""online"": 1200}}", data);
            WriteVarInt(data.Count, result);

            foreach (var b in data)
            {
                result.Add(b);
            }
            stream.Write(result.ToArray(), 0, result.Count);

            Console.WriteLine("[Read Third Packet]");
            Console.WriteLine("Packet Length: " + ReadVarInt(stream));
            Console.WriteLine("Packet ID: " + ReadVarInt(stream));
            var value = ReadLong(stream);
            Console.WriteLine("Long: " + value);

            var pongData = new List<byte>();
            var pongResult = new List<byte>();
            WriteVarInt(1, pongData);
            WriteLong(value, pongData);
            WriteVarInt(pongData.Count, pongResult);

            foreach (var b in pongData)
            {
                pongResult.Add(b);
            }
            stream.Write(pongResult.ToArray(), 0, pongResult.Count);
        }
        else if(nextState == 2)
        {
            Console.WriteLine("[Read Second Packet]");
            Console.WriteLine("Packet Length: " + ReadVarInt(stream));
            Console.WriteLine("Packet ID: " + ReadVarInt(stream));
            var username = ReadString(stream);
            Console.WriteLine("Username: " + username);
            var guid = Guid.NewGuid().ToByteArray();

            var resultLogin = new List<byte>();
            var loginData = new List<byte>();
            WriteVarInt(2, loginData);
            foreach(var b in guid)
            {
                loginData.Add(b);
            }
            WriteString(username, loginData);
            WriteVarInt(loginData.Count, resultLogin);
            foreach(var b in loginData)
            {
                resultLogin.Add(b);
            }

            stream.Write(resultLogin.ToArray(), 0, resultLogin.Count);
        }

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
    static long ReadLong(Stream stream)
    {
        var l = Read(8, stream);
        return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(l, 0));
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

    static void WriteVarInt(int integer, List<byte> buffer)
    {
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
    static void WriteString(string str, List<byte> buffer)
    {
        byte[] data = Encoding.UTF8.GetBytes(str);
        WriteVarInt(data.Length, buffer);
        foreach (var b in data)
        {
            buffer.Add(b);
        }
    }
    static void WriteLong(long data, List<byte> buffer)
    {
        Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data)), buffer);
    }
    static void Write(byte[] data, List<byte> buffer)
    {
        foreach (var i in data)
        {
            buffer.Add(i);
        }
    }
}