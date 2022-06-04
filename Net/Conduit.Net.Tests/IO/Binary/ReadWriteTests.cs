using System;
using System.IO;

using Xunit;

using fNbt.Tags;

using Conduit.Net.IO.Binary;

namespace Conduit.Net.Tests.IO.Binary
{
    public class ReadWriteTests : IDisposable
    {
        private readonly MemoryStream _memory;
        private readonly IReader _reader;
        private readonly IWriter _writer;
        
        public ReadWriteTests()
        {
            _memory = new MemoryStream();
            _reader = new Reader(_memory);
            _writer = new Writer(_memory);
        }
        public void Dispose()
        {
            _memory.Close();
            _reader.Dispose();
            _writer.Dispose();
        }

        [Fact] public void Boolean()
        {
            _writer.Write(false);
            _writer.Write(true);
            _memory.Position = 0;

            Assert.False(_reader.ReadBoolean());
            Assert.True(_reader.ReadBoolean());
        }
        [Fact] public void Byte()
        {
            const byte value1 = byte.MinValue;
            const byte value2 = byte.MaxValue;

            _writer.Write(value1);
            _writer.Write(value2);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.ReadByte());
            Assert.Equal(value2, _reader.ReadByte());
        }
        [Fact] public void SByte()
        {
            const sbyte value1 = sbyte.MinValue;
            const sbyte value2 = sbyte.MaxValue;

            _writer.Write(value1);
            _writer.Write(value2);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.ReadSByte());
            Assert.Equal(value2, _reader.ReadSByte());
        }
        [Fact] public void Bytes()
        {
            ReadOnlySpan<byte> data = stackalloc byte[] { 0, 1, 2, 3, 254, 255 };
            
            _writer.Write(data);
            _memory.Position = 0;

            Span<byte> receiveData = stackalloc byte[data.Length];
            var countBytes = _reader.Read(receiveData);
            
            Assert.Equal(data.Length, countBytes);
            Assert.True(data.SequenceEqual(receiveData));
        }
        [Fact] public void Int16()
        {
            const short value1 = short.MinValue;
            const short value2 = short.MaxValue;
            
            _writer.Write(value1);
            _writer.Write(value2);
            _memory.Position = 0;
            
            Assert.Equal(value1, _reader.ReadInt16());
            Assert.Equal(value2, _reader.ReadInt16());
        }
        [Fact] public void Int32()
        {
            const int value1 = int.MinValue;
            const int value2 = int.MaxValue;

            _writer.Write(value1);
            _writer.Write(value2);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.ReadInt32());
            Assert.Equal(value2, _reader.ReadInt32());
        }
        [Fact] public void Int64()
        {
            const long value1 = long.MinValue;
            const long value2 = long.MaxValue;

            _writer.Write(value1);
            _writer.Write(value2);
            _memory.Position = 0;
            
            Assert.Equal(value1, _reader.ReadInt64());
            Assert.Equal(value2, _reader.ReadInt64());
        }
        [Fact] public void UInt16()
        {
            const ushort value1 = ushort.MinValue;
            const ushort value2 = ushort.MaxValue;
            
            _writer.Write(value1);
            _writer.Write(value2);
            _memory.Position = 0;
            
            Assert.Equal(value1, _reader.ReadUInt16());
            Assert.Equal(value2, _reader.ReadUInt16());
        }
        [Fact] public void UInt32()
        {
            const uint value1 = uint.MinValue;
            const uint value2 = uint.MaxValue;

            _writer.Write(value1);
            _writer.Write(value2);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.ReadUInt32());
            Assert.Equal(value2, _reader.ReadUInt32());
        }
        [Fact] public void UInt64()
        {
            const ulong value1 = ulong.MinValue;
            const ulong value2 = ulong.MaxValue;
            
            _writer.Write(value1);
            _writer.Write(value2);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.ReadUInt64());
            Assert.Equal(value2, _reader.ReadUInt64());
        }
        
        [Fact] public void Int32In7Bit()
        {
            const int value1 = int.MinValue;
            const int value2 = int.MaxValue;
            
            _writer.Write7BitEncodedInt(value1);
            _writer.Write7BitEncodedInt(value2);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.Read7BitEncodedInt());
            Assert.Equal(value2, _reader.Read7BitEncodedInt());
        }
        [Fact] public void Int64In7Bit()
        {
            const long value1 = long.MinValue;
            const long value2 = long.MaxValue;

            _writer.Write7BitEncodedInt64(value1);
            _writer.Write7BitEncodedInt64(value2);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.Read7BitEncodedInt64());
            Assert.Equal(value2, _reader.Read7BitEncodedInt64());
        }

        [Fact] public void Half()
        {
            var value1 = System.Half.MinValue;
            var value2 = System.Half.MaxValue;
            var value3 = System.Half.Epsilon;
            var value4 = System.Half.NaN;
            var value5 = System.Half.NegativeInfinity;
            var value6 = System.Half.PositiveInfinity;

            _writer.Write(value1);
            _writer.Write(value2);
            _writer.Write(value3);
            _writer.Write(value4);
            _writer.Write(value5);
            _writer.Write(value6);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.ReadHalf());
            Assert.Equal(value2, _reader.ReadHalf());
            Assert.Equal(value3, _reader.ReadHalf());
            Assert.Equal(value4, _reader.ReadHalf());
            Assert.Equal(value5, _reader.ReadHalf());
            Assert.Equal(value6, _reader.ReadHalf());
        }
        [Fact] public void Single()
        {
            const float value1 = float.MinValue;
            const float value2 = float.MaxValue;
            const float value3 = float.Epsilon;
            const float value4 = float.NaN;
            const float value5 = float.NegativeInfinity;
            const float value6 = float.PositiveInfinity;

            _writer.Write(value1);
            _writer.Write(value2);
            _writer.Write(value3);
            _writer.Write(value4);
            _writer.Write(value5);
            _writer.Write(value6);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.ReadSingle());
            Assert.Equal(value2, _reader.ReadSingle());
            Assert.Equal(value3, _reader.ReadSingle());
            Assert.Equal(value4, _reader.ReadSingle());
            Assert.Equal(value5, _reader.ReadSingle());
            Assert.Equal(value6, _reader.ReadSingle());
        }
        [Fact] public void Double()
        {
            const double value1 = double.MinValue;
            const double value2 = double.MaxValue;
            const double value3 = double.Epsilon;
            const double value4 = double.NaN;
            const double value5 = double.NegativeInfinity;
            const double value6 = double.PositiveInfinity;

            _writer.Write(value1);
            _writer.Write(value2);
            _writer.Write(value3);
            _writer.Write(value4);
            _writer.Write(value5);
            _writer.Write(value6);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.ReadDouble());
            Assert.Equal(value2, _reader.ReadDouble());
            Assert.Equal(value3, _reader.ReadDouble());
            Assert.Equal(value4, _reader.ReadDouble());
            Assert.Equal(value5, _reader.ReadDouble());
            Assert.Equal(value6, _reader.ReadDouble());
        }
        [Fact] public void Decimal()
        {
            const decimal value1 = decimal.MinValue;
            const decimal value2 = decimal.MaxValue;

            _writer.Write(value1);
            _writer.Write(value2);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.ReadDecimal());
            Assert.Equal(value2, _reader.ReadDecimal());
        }

        [Fact] public void Char()
        {
            const char value1 = char.MinValue;
            const char value2 = char.MaxValue;

            _writer.Write(value1);
            _writer.Write(value2);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.ReadChar());
            Assert.Equal(value2, _reader.ReadChar());
        }
        [Fact] public void Chars()
        {
            ReadOnlySpan<char> data = stackalloc char[] { char.MinValue, char.MaxValue };

            _writer.Write(data);
            _memory.Position = 0;

            Span<char> receivedData = stackalloc char[data.Length];
            var countChars = _reader.Read(receivedData);

            Assert.Equal(data.Length, countChars);
            Assert.True(data.SequenceEqual(receivedData));
        }
        [Fact] public void String()
        {
            const string value1 = "ABCdefghijklmnopqrstuvwxyz";
            const string value2 = "АБВгдеёжзийклмнопрстуфхцчшщъыьэюя";
            const string value3 = "!@#$%^&*()_+-=[]{}|;':,./<>?`~";
            const string value4 = "0123456789";
            const string value5 = "   ";
            const string value6 = "";
            const string value7 = "\n\r\0";
            const string value8 = "你好，世界！";

            _writer.Write(value1);
            _writer.Write(value2);
            _writer.Write(value3);
            _writer.Write(value4);
            _writer.Write(value5);
            _writer.Write(value6);
            _writer.Write(value7);
            _writer.Write(value8);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.ReadString());
            Assert.Equal(value2, _reader.ReadString());
            Assert.Equal(value3, _reader.ReadString());
            Assert.Equal(value4, _reader.ReadString());
            Assert.Equal(value5, _reader.ReadString());
            Assert.Equal(value6, _reader.ReadString());
            Assert.Equal(value7, _reader.ReadString());
            Assert.Equal(value8, _reader.ReadString());
        }
        
        [Fact] public void Guid()
        {
            var value1 = System.Guid.Empty;
            var value2 = System.Guid.NewGuid();

            _writer.Write(value1);
            _writer.Write(value2);
            _memory.Position = 0;

            Assert.Equal(value1, _reader.ReadGuid());
            Assert.Equal(value2, _reader.ReadGuid());
        }
        [Fact] public void Nbt()
        {
            var value = new NbtCompound("Test", new NbtTag[]
            {
                new NbtByte("Byte", 1)
            });

            _writer.Write(value);
            _memory.Position = 0;

            var readedNbt = _reader.ReadNbt() as NbtCompound;

            Assert.NotNull(readedNbt);
            Assert.Equal(value.Name, readedNbt.Name);
            Assert.Equal(value.Count, readedNbt.Count);
            Assert.Equal(value.Get("Byte").ByteValue, readedNbt.Get("Byte").ByteValue);
        }
    }
}