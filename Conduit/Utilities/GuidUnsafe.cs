using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Utilities
{
    public struct GuidUnsafe
    {
        public Guid Guid;
        public GuidUnsafe(Guid g)
        {
            Guid = g;
        }
        public static bool operator ==(GuidUnsafe g1, GuidUnsafe g2)
        {
            return EqualsGuids(g1.Guid, g2.Guid);
        }
        public static bool operator !=(GuidUnsafe g1, GuidUnsafe g2)
        {
            return !EqualsGuids(g1.Guid, g2.Guid);
        }
        public static implicit operator GuidUnsafe(Guid g)
        {
            return new GuidUnsafe(g);
        }
        public static explicit operator Guid(GuidUnsafe g)
        {
            return g.Guid;
        }
        public static unsafe bool EqualsGuids(Guid g1, Guid g2)
        { // high performance for native execution
            long* p1 = (long*)&g1;
            long* p2 = (long*)&g2;
            
            bool state = 
                p1[0] == p2[0] &&
                p1[1] == p2[1];
            //Console.WriteLine($"p1[0]:{p1[0]} == p2[0]:{p2[0]}\np1[1]:{p1[1]} == p2[1]:{p2[1]}\n");

            return state;
        }
    }
}
