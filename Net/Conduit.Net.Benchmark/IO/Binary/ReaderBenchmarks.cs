using System;
using System.IO;

using BenchmarkDotNet.Order;
using BenchmarkDotNet.Attributes;

using fNbt.Tags;

using Conduit.Net.IO.Binary;

namespace Conduit.Net.Benchmark.IO.Binary
{
    [ShortRunJob]
    [HtmlExporter]
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class ReaderBenchmarks
    {
        private readonly MemoryStream _memory;
        private readonly IReader _reader;
        private readonly IWriter _writer;

        public ReaderBenchmarks()
        {
            _memory = new MemoryStream();
            _reader = new Reader(_memory);
            _writer = new Writer(_memory);
        }
        [GlobalCleanup] public void Cleanup()
        {
            _memory.Close();
            _reader.Dispose();
            _writer.Dispose();
        }
        
        [GlobalSetup(Target = nameof(Boolean))]
        public void BooleanSetup()
        {
            _writer.Write(true);
            _memory.Position = 0;
        }
        [Benchmark] public bool Boolean()
        {
            var result = _reader.ReadBoolean();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(Byte))]
        public void ByteSetup()
        {
            _writer.Write((byte)1);
            _memory.Position = 0;
        }
        [Benchmark] public byte Byte()
        {
            var result = _reader.ReadByte();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(SByte))]
        public void SByteSetup()
        {
            _writer.Write((sbyte)-1);
            _memory.Position = 0;
        }
        [Benchmark] public sbyte SByte()
        {
            var result = _reader.ReadSByte();
            _memory.Position = 0;

            return result;
        }
        
        [GlobalSetup(Targets = new [] { nameof(Bytes), nameof(SpanBytes) })]
        public void BytesSetup()
        {
            _writer.Write(stackalloc byte[] { 0, 1, 254, 255 });
            _memory.Position = 0;
        }
        [Benchmark] public byte[] Bytes()
        {
            var result = _reader.ReadBytes(4);
            _memory.Position = 0;

            return result;
        }
        [Benchmark] public void SpanBytes()
        {
            _reader.Read(stackalloc byte[4]);
            _memory.Position = 0;
        }
        
        [GlobalSetup(Target = nameof(Int16))]
        public void Int16Setup()
        {
            _writer.Write((short)-1);
            _memory.Position = 0;
        }
        [Benchmark] public short Int16()
        {
            var result = _reader.ReadInt16();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(Int32))]
        public void Int32Setup()
        {
            _writer.Write(-1);
            _memory.Position = 0;
        }
        [Benchmark] public int Int32()
        {
            var result = _reader.ReadInt32();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(Int64))]
        public void Int64Setup()
        {
            _writer.Write(-1L);
            _memory.Position = 0;
        }
        [Benchmark] public long Int64()
        {
            var result = _reader.ReadInt64();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(UInt16))]
        public void UInt16Setup()
        {
            _writer.Write((ushort)1);
            _memory.Position = 0;
        }
        [Benchmark] public ushort UInt16()
        {
            var result = _reader.ReadUInt16();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(UInt32))]
        public void UInt32Setup()
        {
            _writer.Write(1U);
            _memory.Position = 0;
        }
        [Benchmark] public uint UInt32()
        {
            var result = _reader.ReadUInt32();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(UInt64))]
        public void UInt64Setup()
        {
            _writer.Write(1UL);
            _memory.Position = 0;
        }
        [Benchmark] public ulong UInt64()
        {
            var result = _reader.ReadUInt64();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(Int32In7Bit))]
        public void Int32In7BitSetup()
        {
            _writer.Write7BitEncodedInt(int.MaxValue);
            _memory.Position = 0;
        }
        [Benchmark] public int Int32In7Bit()
        {
            var result = _reader.Read7BitEncodedInt();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(Int64In7Bit))]
        public void Int64In7BitSetup()
        {
            _writer.Write7BitEncodedInt64(long.MaxValue);
            _memory.Position = 0;
        }
        [Benchmark] public long Int64In7Bit()
        {
            var result = _reader.Read7BitEncodedInt64();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(Half))]
        public void HalfSetup()
        {
            _writer.Write(System.Half.MaxValue);
            _memory.Position = 0;
        }
        [Benchmark] public Half Half()
        {
            var result = _reader.ReadHalf();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(Single))]
        public void SingleSetup()
        {
            _writer.Write(float.MaxValue);
            _memory.Position = 0;
        }
        [Benchmark] public float Single()
        {
            var result = _reader.ReadSingle();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(Double))]
        public void DoubleSetup()
        {
            _writer.Write(double.MaxValue);
            _memory.Position = 0;
        }
        [Benchmark] public double Double()
        {
            var result = _reader.ReadDouble();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(Decimal))]
        public void DecimalSetup()
        {
            _writer.Write(decimal.MaxValue);
            _memory.Position = 0;
        }
        [Benchmark] public decimal Decimal()
        {
            var result = _reader.ReadDecimal();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(Char))]
        public void CharSetup()
        {
            _writer.Write('a');
            _memory.Position = 0;
        }
        [Benchmark] public char Char()
        {
            var result = _reader.ReadChar();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Targets = new[] { nameof(Chars), nameof(SpanChars) })]
        public void CharsSetup()
        {
            _writer.Write(stackalloc char[] { 'a', 'b' });
            _memory.Position = 0;
        }
        [Benchmark] public char[] Chars()
        {
            var chars = _reader.ReadChars(2);
            _memory.Position = 0;

            return chars;
        }
        [Benchmark] public void SpanChars()
        {
            _reader.Read(stackalloc char[2]);
            _memory.Position = 0;
        }

        [GlobalSetup(Target = nameof(String))]
        public void StringSetup()
        {
            _writer.Write("Hello, world!");
            _memory.Position = 0;
        }
        [Benchmark] public string String()
        {
            var result = _reader.ReadString();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(Guid))]
        public void GuidSetup()
        {
            _writer.Write(System.Guid.NewGuid());
            _memory.Position = 0;
        }
        [Benchmark] public Guid Guid()
        {
            var result = _reader.ReadGuid();
            _memory.Position = 0;

            return result;
        }

        [GlobalSetup(Target = nameof(Nbt))]
        public void NbtSetup()
        {
            _writer.Write(new NbtCompound("Root"));
            _memory.Position = 0;
        }
        [Benchmark] public NbtTag Nbt()
        {
            var result = _reader.ReadNbt();
            _memory.Position = 0;

            return result;
        }
    }
}