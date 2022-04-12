using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Utilities
{
    public static class MathUtils
    {
        public static bool IsNumber(Type type)
        {
            GuidUnsafe guid = type.GUID;

            if (guid == Types.ByteGuid ||
                guid == Types.SByteGuid ||
                guid == Types.ShortGuid ||
                guid == Types.UShortGuid ||
                guid == Types.IntGuid || 
                guid == Types.UIntGuid ||
                guid == Types.LongGuid ||
                guid == Types.ULongGuid ||
                guid == Types.FloatGuid ||
                guid == Types.DoubleGuid)
                return true;

            return false;
        }
    }
}
