using System;

namespace Conduit.Net.Data
{
    public class TypeMap : IdMap<Type>
    {
        public TypeMap(params Type[] valueByIndexTable) : base(valueByIndexTable) { }
    }
}