using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Utilities
{
    public static class Types
    {
        public static GuidUnsafe BoolGuid = typeof(bool).GUID;

        public static GuidUnsafe SByteGuid = typeof(sbyte).GUID;
        public static GuidUnsafe ByteGuid = typeof(byte).GUID;
        public static GuidUnsafe ShortGuid = typeof(short).GUID;
        public static GuidUnsafe UShortGuid = typeof(ushort).GUID;
        public static GuidUnsafe IntGuid = typeof(int).GUID;
        public static GuidUnsafe UIntGuid = typeof(uint).GUID;
        public static GuidUnsafe LongGuid = typeof(long).GUID;
        public static GuidUnsafe ULongGuid = typeof(ulong).GUID;

        public static GuidUnsafe FloatGuid = typeof(float).GUID;
        public static GuidUnsafe DoubleGuid = typeof(double).GUID;

        public static GuidUnsafe StringGuid = typeof(string).GUID;
        public static GuidUnsafe GuidGuid = typeof(Guid).GUID;
        
    }
}
