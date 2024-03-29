﻿using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Conduit.Net.IO.RawPacket
{
    public class Writer : IWriter
    {
        public Binary.Writer BinaryWriter { get; set; }

        public Writer(Stream stream, bool leaveOpen = false) : this(new Binary.Writer(stream, Encoding.UTF8, leaveOpen)) { }
        public Writer(Binary.Writer binaryWriter)
        {
            BinaryWriter = binaryWriter;
        }

        public void Dispose() => BinaryWriter?.Dispose();

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Write(Packets.RawPacket rawPacket)
        {
            BinaryWriter.Write7BitEncodedInt(rawPacket.Length);
            BinaryWriter.Write7BitEncodedInt(rawPacket.Id);
            BinaryWriter.Write(rawPacket.Data);
        }
    }
}