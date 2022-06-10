using System;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;

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
        private Reader _reader;
        private Stream _memory;

        private ISerializable[] _values;

        [GlobalSetup] public void Setup()
        {
            _values = new ISerializable[10];
            for (var i = 0; i < _values.Length; i++)
                _values[i] = new Short();

            _memory = new MemoryStream();
            _reader = new Reader(_memory);

            using var writer = new Writer(_memory, Encoding.UTF8, true);
            
            for (var i = 0; i < 10; i++)
                writer.Write((short)i);

            _memory.Position = 0;
        }
        [GlobalCleanup] public void Cleanup()
        {
            _memory.Close();
            _reader.Dispose();
        }
        
        [Benchmark(Baseline = true)]
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void NewBitwiseFor()
        {
            Span<Short> values = stackalloc Short[10];

            for (var i = 0; i < values.Length; i++)
                values[i].Read(_memory);

            _memory.Position = 0;
        }
        [Benchmark]
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void NewBitwiseForWithBoxing()
        {
            for (var i = 0; i < _values.Length; i++)
                _values[i].Read(_memory);

            _memory.Position = 0;
        }
        [Benchmark]
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void NewBitwiseForeachWithBoxing()
        {
            foreach (var value in _values)
                value.Read(_memory);

            _memory.Position = 0;
        }
    }
}