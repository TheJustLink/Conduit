using System;
using System.Buffers.Binary;
using System.IO;

using Xunit;

using Conduit.Net.IO.Binary;

namespace Conduit.Net.Tests.IO.Binary
{
    public class ReaderManualTests
    {
        [Fact]
        public void Read_Byte()
        {
            const byte data = 0x01;
            
            using var reader = new Reader(new [] { data });

            Assert.Equal(data, reader.ReadByte());
        }
        [Fact]
        public void Read_Bytes()
        {
            var data = new byte[] { 0x01, 0x02, 0x03 };
            
            using var reader = new Reader(data);
            
            Assert.Equal(data, reader.ReadBytes(3));
        }
        [Fact]
        public void Read_Int16()
        {
            const short value = 0x201;
            
            using var reader = new Reader(BitConverter.GetBytes(value));
            
            Assert.Equal(value, reader.ReadInt16());
        }
        [Fact]
        public void Read_Int32()
        {
            const int value = 0x4030201;

            using var reader = new Reader(BitConverter.GetBytes(value));
            
            Assert.Equal(value, reader.ReadInt32());
        }
        [Fact]
        public void Read_Int64()
        {
            const long value = 0x0807060504030201;

            using var reader = new Reader(BitConverter.GetBytes(value));
            
            Assert.Equal(value, reader.ReadInt64());
        }
        [Fact]
        public void Read_UInt16()
        {
            const ushort value = 0x0102;
            var bigEndianValue = BinaryPrimitives.ReadUInt16BigEndian(BitConverter.GetBytes(value));
            
            using var reader = new Reader(BitConverter.GetBytes(bigEndianValue));

            Assert.Equal(value, reader.ReadUInt16());
        }
        [Fact]
        public void Read_UInt32()
        {
            const uint value = 0x04030201;

            using var reader = new Reader(BitConverter.GetBytes(value));
            
            Assert.Equal(value, reader.ReadUInt32());
        }
        [Fact]
        public void Read_UInt64()
        {
            const ulong value = 0x0807060504030201;

            using var reader = new Reader(BitConverter.GetBytes(value));
            
            Assert.Equal(value, reader.ReadUInt64());
        }

        [Fact]
        public void Read_String()
        {
            using var memory = new MemoryStream();
            using var writer = new Writer(memory);
            using var reader = new Reader(memory);
            
            const string testString = "Hello World!";
            
            writer.Write(testString);
            memory.Position = 0;
            
            Assert.Equal(testString, reader.ReadString());
        }
    }
}