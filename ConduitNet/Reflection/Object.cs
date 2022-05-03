using System.Linq;
using System.Collections.Generic;

namespace Conduit.Net.Reflection
{
    public static class Object
    {
        public static HashSet<int> StandartMethodsHashcodes =
            (from method in typeof(object).GetMethods()
                select method.Name.GetHashCode()).ToHashSet();
    }
}