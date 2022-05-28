using System.IO;

using Conduit.Net.IO.Binary;

using Xunit;

namespace Conduit.Net.Tests.IO.Binary
{
    public class ReaderWithWriterTests
    {
        [Fact]
        public void Read_Byte()
        {
            using var memory = new MemoryStream();
            using var writer = new Writer(memory);
            using var reader = new Reader(memory);

            const byte value = 0x01;

            writer.Write(value);
            memory.Position = 0;

            Assert.Equal(value, reader.ReadByte());
        }
        [Fact]
        public void Read_Bytes()
        {
            using var memory = new MemoryStream();
            using var writer = new Writer(memory);
            using var reader = new Reader(memory);

            var data = new byte[] { 0x01, 0x02, 0x03 };

            writer.Write(data);
            memory.Position = 0;

            Assert.Equal(data, reader.ReadBytes(data.Length));
        }
        [Fact]
        public void Read_Int16()
        {
            using var memory = new MemoryStream();
            using var writer = new Writer(memory);
            using var reader = new Reader(memory);

            const short value = 0x0102;

            writer.Write(value);
            memory.Position = 0;

            Assert.Equal(value, reader.ReadInt16());
        }
        [Fact]
        public void Read_Int32()
        {
            using var memory = new MemoryStream();
            using var writer = new Writer(memory);
            using var reader = new Reader(memory);

            const int value = 0x01020304;

            writer.Write(value);
            memory.Position = 0;

            Assert.Equal(value, reader.ReadInt32());
        }
        [Fact]
        public void Read_Int64()
        {
            using var memory = new MemoryStream();
            using var writer = new Writer(memory);
            using var reader = new Reader(memory);

            const long value = 0x0102030405060708;

            writer.Write(value);
            memory.Position = 0;

            Assert.Equal(value, reader.ReadInt64());
        }
        [Fact]
        public void Read_UInt16()
        {
            using var memory = new MemoryStream();
            using var writer = new Writer(memory);
            using var reader = new Reader(memory);

            const ushort value = 0x0102;

            writer.Write(value);
            memory.Position = 0;

            Assert.Equal(value, reader.ReadUInt16());
        }
        [Fact]
        public void Read_UInt32()
        {
            using var memory = new MemoryStream();
            using var writer = new Writer(memory);
            using var reader = new Reader(memory);

            const uint value = 0x01020304;

            writer.Write(value);
            memory.Position = 0;

            Assert.Equal(value, reader.ReadUInt32());
        }
        [Fact]
        public void Read_UInt64()
        {
            using var memory = new MemoryStream();
            using var writer = new Writer(memory);
            using var reader = new Reader(memory);

            const ulong value = 0x0102030405060708;

            writer.Write(value);
            memory.Position = 0;

            Assert.Equal(value, reader.ReadUInt64());
        }
        [Fact]
        public void Read_Double()
        {
            using var memory = new MemoryStream();
            using var writer = new Writer(memory);
            using var reader = new Reader(memory);

            const double value = 0.123456789;

            writer.Write(value);
            memory.Position = 0;

            Assert.Equal(value, reader.ReadDouble());
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