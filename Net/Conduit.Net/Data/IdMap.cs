using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Data
{
    public class IdMap<T> where T : class
    {
        public T this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => _valueByIndexTable[i];
        }
        public int this[T i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => _indexByValueTable[i.GetHashCode()];
        }

        private readonly T[] _valueByIndexTable;
        private readonly Dictionary<int, int> _indexByValueTable;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public IdMap(params T[] valueByIndexTable)
        {
            _valueByIndexTable = valueByIndexTable;
            _indexByValueTable = new Dictionary<int, int>(valueByIndexTable.Length);

            for (var i = 0; i < valueByIndexTable.Length; i++)
                _indexByValueTable.Add(valueByIndexTable[i].GetHashCode(), i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool Has(int id) => id < _valueByIndexTable.Length;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public int GetId(int valueHashCode) => _indexByValueTable[valueHashCode];
    }
}