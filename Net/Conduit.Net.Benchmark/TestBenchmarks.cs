using System;
using System.IO;
using System.Text;

using BenchmarkDotNet.Order;
using BenchmarkDotNet.Attributes;

using Conduit.Net.Data;
using Conduit.Net.IO.Binary;

namespace Conduit.Net.Benchmark
{
    //[ShortRunJob]
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class TestBenchmarks
    {
        private Reader _reader = null!;
        private Stream _memory = null!;

        private ISerializable[] _values = null!;

        [GlobalSetup]
        public void Setup()
        {
            _values = new ISerializable[1000];
            for (var i = 0; i < _values.Length; i++)
                _values[i] = new Short();

            _memory = new MemoryStream();
            _reader = new Reader(_memory);

            using var writer = new Writer(_memory, Encoding.UTF8, true);

            for (var i = 0; i < 10; i++)
                writer.Write((short)i);

            _memory.Position = 0;
        }
        [GlobalCleanup]
        public void Cleanup()
        {
            _memory.Close();
            _reader.Dispose();
        }
        
        [Benchmark(Baseline = true)]
        public void ReadWithWrap()
        {
            const int length = 1000;
            Span<Short> values = stackalloc Short[length];
            
            var sum = new Short();

            for (var i = 0; i < length; i++)
                sum += values[i];

            //_memory.Position = 0;
        }
        [Benchmark]
        public void ReadWithWrapForeach()
        {
            Span<Short> values = stackalloc Short[1000];

            var sum = new Short();

            foreach (var value in values)
                sum += value;

            //_memory.Position = 0;
        }
        [Benchmark]
        public void ReadWithoutWrap()
        {
            const int length = 1000;
            Span<short> values = stackalloc short[length];

            short sum = 0;

            for (var i = 0; i < length; i++)
                sum += values[i];
            
            //values[i] = (short) (_memory.ReadByte() | (_memory.ReadByte() << 8));

            //_memory.Position = 0;
        }
    }
}