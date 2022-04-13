using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ConduitServer.Nbt
{
    class CodecCollection<T, K> : ConcurrentDictionary<T, K>
    {
        public string Name { get; }
        
        public CodecCollection(string name) : base(new Dictionary<T, K>())
        {
            Name = name;
        }
    }
}
