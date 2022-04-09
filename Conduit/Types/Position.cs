using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Types
{
    public struct Position
    {
        public int X;
        public short Y;
        public int Z;

        public Position(ulong val)
        {
            X = (int)(val >> 38);
            Y = (short)(val & 0xFFF);
            Z = (int)((val >> 12) & 0x3FFFFFF);
        }

        public ulong Encode()
        {
            return (ulong)((X & 0x3FFFFFF) << 38) | (ulong)((Z & 0x3FFFFFF) << 12) | (ulong)(Y & 0xFFF);
        }

        public static Position Decode(ulong val)
        {
            return new Position(val);
        }

        public static implicit operator Position(ulong val)
        {
            return new Position(val);
        }
    }
}
