using System.Linq;
using System.Collections.Generic;

namespace Conduit.Net.Reflection
{
    public static class Object
    {
        public static readonly HashSet<int> StandartMethodsHashcodes =
            (from method in typeof(object).GetMethods()
                select method.Name.GetHashCode()).ToHashSet();
    }
    public static class Object<T>
    {
        public static readonly int HashCode = typeof(T).GetHashCode();
    }
}