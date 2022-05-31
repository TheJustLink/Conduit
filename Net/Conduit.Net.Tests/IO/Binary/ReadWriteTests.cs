using System;
using System.IO;

using Xunit;

using fNbt;
using fNbt.Tags;

using Conduit.Net.IO.Binary;

namespace Conduit.Net.Tests.IO.Binary
{
    public class BinaryReadWriteDefaultTests : ReadWriteTests<Reader, Writer> { }
    
    public abstract class ReadWriteTests<TReader, TWriter>
        where TReader : IReader, new()
        where TWriter : IWriter, new()
    {
        private readonly TReader _reader;
        private readonly TWriter _writer;
        
        protected ReadWriteTests() => (_reader, _writer) = (new TReader(), new TWriter());

        [Fact]
        public void Boolean()
        {
            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(false);
            writer.Write(true);
            memory.Position = 0;

            Assert.False(reader.ReadBoolean());
            Assert.True(reader.ReadBoolean());
        }
        [Fact]
        public void Byte()
        {
            const byte value1 = byte.MinValue;
            const byte value2 = byte.MaxValue;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            memory.Position = 0;

            Assert.Equal(value1, reader.ReadByte());
        }
        [Fact]
        public void SByte()
        {
            const sbyte value1 = sbyte.MinValue;
            const sbyte value2 = sbyte.MaxValue;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            memory.Position = 0;

            Assert.Equal(value1, reader.ReadSByte());
            Assert.Equal(value2, reader.ReadSByte());
        }
        [Fact]
        public void Bytes()
        {
            ReadOnlySpan<byte> data = stackalloc byte[] { 0, 1, 2, 3, 254, 255 };

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);
            
            writer.Write(data);
            memory.Position = 0;

            Span<byte> receiveData = stackalloc byte[data.Length];
            var countBytes = reader.Read(receiveData);
            
            Assert.Equal(data.Length, countBytes);
            Assert.True(data.SequenceEqual(receiveData));
        }
        [Fact]
        public void Int16()
        {
            const short value1 = short.MinValue;
            const short value2 = short.MaxValue;
            
            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);
            
            writer.Write(value1);
            writer.Write(value2);
            memory.Position = 0;
            
            Assert.Equal(value1, reader.ReadInt16());
            Assert.Equal(value2, reader.ReadInt16());
        }
        [Fact]
        public void Int32()
        {
            const int value1 = int.MinValue;
            const int value2 = int.MaxValue;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            memory.Position = 0;

            Assert.Equal(value1, reader.ReadInt32());
            Assert.Equal(value2, reader.ReadInt32());
        }
        [Fact]
        public void Int64()
        {
            const long value1 = long.MinValue;
            const long value2 = long.MaxValue;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            memory.Position = 0;
            
            Assert.Equal(value1, reader.ReadInt64());
            Assert.Equal(value2, reader.ReadInt64());
        }
        [Fact]
        public void UInt16()
        {
            const ushort value1 = ushort.MinValue;
            const ushort value2 = ushort.MaxValue;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            memory.Position = 0;
            
            Assert.Equal(value1, reader.ReadUInt16());
            Assert.Equal(value2, reader.ReadUInt16());
        }
        [Fact]
        public void UInt32()
        {
            const uint value1 = uint.MinValue;
            const uint value2 = uint.MaxValue;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            memory.Position = 0;

            Assert.Equal(value1, reader.ReadUInt32());
            Assert.Equal(value2, reader.ReadUInt32());
        }
        [Fact]
        public void UInt64()
        {
            const ulong value1 = ulong.MinValue;
            const ulong value2 = ulong.MaxValue;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            memory.Position = 0;

            Assert.Equal(value1, reader.ReadUInt64());
            Assert.Equal(value2, reader.ReadUInt64());
        }
        
        [Fact]
        public void Int32In7Bit()
        {
            const int value1 = int.MinValue;
            const int value2 = int.MaxValue;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write7BitEncodedInt(value1);
            writer.Write7BitEncodedInt(value2);
            memory.Position = 0;

            Assert.Equal(value1, reader.Read7BitEncodedInt());
            Assert.Equal(value2, reader.Read7BitEncodedInt());
        }
        [Fact]
        public void Int64In7Bit()
        {
            const long value1 = long.MinValue;
            const long value2 = long.MaxValue;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write7BitEncodedInt64(value1);
            writer.Write7BitEncodedInt64(value2);
            memory.Position = 0;

            Assert.Equal(value1, reader.Read7BitEncodedInt64());
            Assert.Equal(value2, reader.Read7BitEncodedInt64());
        }

        [Fact]
        public void Half()
        {
            var value1 = System.Half.MinValue;
            var value2 = System.Half.MaxValue;
            var value3 = System.Half.Epsilon;
            var value4 = System.Half.NaN;
            var value5 = System.Half.NegativeInfinity;
            var value6 = System.Half.PositiveInfinity;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            writer.Write(value3);
            writer.Write(value4);
            writer.Write(value5);
            writer.Write(value6);
            memory.Position = 0;

            Assert.Equal(value1, reader.ReadHalf());
            Assert.Equal(value2, reader.ReadHalf());
            Assert.Equal(value3, reader.ReadHalf());
            Assert.Equal(value4, reader.ReadHalf());
            Assert.Equal(value5, reader.ReadHalf());
            Assert.Equal(value6, reader.ReadHalf());
        }
        [Fact]
        public void Single()
        {
            const float value1 = float.MinValue;
            const float value2 = float.MaxValue;
            const float value3 = float.Epsilon;
            const float value4 = float.NaN;
            const float value5 = float.NegativeInfinity;
            const float value6 = float.PositiveInfinity;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            writer.Write(value3);
            writer.Write(value4);
            writer.Write(value5);
            writer.Write(value6);
            memory.Position = 0;

            Assert.Equal(value1, reader.ReadSingle());
            Assert.Equal(value2, reader.ReadSingle());
            Assert.Equal(value3, reader.ReadSingle());
            Assert.Equal(value4, reader.ReadSingle());
            Assert.Equal(value5, reader.ReadSingle());
            Assert.Equal(value6, reader.ReadSingle());
        }
        [Fact]
        public void Double()
        {
            const double value1 = double.MinValue;
            const double value2 = double.MaxValue;
            const double value3 = double.Epsilon;
            const double value4 = double.NaN;
            const double value5 = double.NegativeInfinity;
            const double value6 = double.PositiveInfinity;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            writer.Write(value3);
            writer.Write(value4);
            writer.Write(value5);
            writer.Write(value6);
            memory.Position = 0;

            Assert.Equal(value1, reader.ReadDouble());
            Assert.Equal(value2, reader.ReadDouble());
            Assert.Equal(value3, reader.ReadDouble());
            Assert.Equal(value4, reader.ReadDouble());
            Assert.Equal(value5, reader.ReadDouble());
            Assert.Equal(value6, reader.ReadDouble());
        }
        [Fact]
        public void Decimal()
        {
            const decimal value1 = decimal.MinValue;
            const decimal value2 = decimal.MaxValue;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            memory.Position = 0;

            Assert.Equal(value1, reader.ReadDecimal());
            Assert.Equal(value2, reader.ReadDecimal());
        }

        [Fact]
        public void Char()
        {
            const char value1 = char.MinValue;
            const char value2 = char.MaxValue;

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            memory.Position = 0;

            Assert.Equal(value1, reader.ReadChar());
            Assert.Equal(value2, reader.ReadChar());
        }
        [Fact]
        public void Chars()
        {
            ReadOnlySpan<char> data = stackalloc char[] { char.MinValue, char.MaxValue };

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(data);
            memory.Position = 0;

            Span<char> receivedData = stackalloc char[data.Length];
            var countChars = reader.Read(receivedData);

            Assert.Equal(data.Length, countChars);
            Assert.True(data.SequenceEqual(receivedData));
        }

        [Fact]
        public void String()
        {
            const string value1 = "ABCdefghijklmnopqrstuvwxyz";
            const string value2 = "АБВгдеёжзийклмнопрстуфхцчшщъыьэюя";
            const string value3 = "!@#$%^&*()_+-=[]{}|;':,./<>?`~";
            const string value4 = "0123456789";
            const string value5 = "   ";
            const string value6 = "";
            const string value7 = "\n\r\0";
            const string value8 = "你好，世界！";

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            writer.Write(value3);
            writer.Write(value4);
            writer.Write(value5);
            writer.Write(value6);
            writer.Write(value7);
            writer.Write(value8);
            memory.Position = 0;

            Assert.Equal(value1, reader.ReadString());
            Assert.Equal(value2, reader.ReadString());
            Assert.Equal(value3, reader.ReadString());
            Assert.Equal(value4, reader.ReadString());
            Assert.Equal(value5, reader.ReadString());
            Assert.Equal(value6, reader.ReadString());
            Assert.Equal(value7, reader.ReadString());
            Assert.Equal(value8, reader.ReadString());
        }

        [Fact]
        public void Guid()
        {
            var value1 = System.Guid.Empty;
            var value2 = System.Guid.NewGuid();

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value1);
            writer.Write(value2);
            memory.Position = 0;

            Assert.Equal(value1, reader.ReadGuid());
            Assert.Equal(value2, reader.ReadGuid());
        }

        [Fact]
        public void Read_Nbt()
        {
            var value = new NbtCompound("Test", new NbtTag[]
            {
                new NbtByte("Byte", 1)
            });

            using var memory = new MemoryStream();
            var nbtFile = new NbtFile(value);

            nbtFile.SaveToStream(memory, NbtCompression.None);
            memory.Position = 0;

            using var reader = new Reader(memory);
            var readedNbt = reader.ReadNbt() as NbtCompound;
            
            Assert.NotNull(readedNbt);
            Assert.Equal(value.Name, readedNbt.Name);
            Assert.Equal(value.Count, readedNbt.Count);
            Assert.Equal(value.Get("Byte").ByteValue, readedNbt.Get("Byte").ByteValue);
        }
        [Fact]
        public void Nbt()
        {
            var value = new NbtCompound("Test", new NbtTag[]
            {
                new NbtByte("Byte", 1)
            });

            using var memory = new MemoryStream();
            using var reader = _reader.ChangeInput(memory);
            using var writer = _writer.ChangeOutput(memory);

            writer.Write(value);
            memory.Position = 0;

            var readedNbt = reader.ReadNbt() as NbtCompound;

            Assert.NotNull(readedNbt);
            Assert.Equal(value.Name, readedNbt.Name);
            Assert.Equal(value.Count, readedNbt.Count);
            Assert.Equal(value.Get("Byte").ByteValue, readedNbt.Get("Byte").ByteValue);
        }
    }
}