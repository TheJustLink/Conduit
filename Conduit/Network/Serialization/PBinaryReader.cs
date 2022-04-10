using Conduit.Utilities;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Serialization
{
    public class PBinaryReader : BinaryReader
    {
        #region Guids
        private GuidUnsafe BoolGuid = typeof(bool).GUID;
        private GuidUnsafe SByteGuid = typeof(sbyte).GUID;
        private GuidUnsafe ByteGuid = typeof(byte).GUID;
        private GuidUnsafe ShortGuid = typeof(short).GUID;
        private GuidUnsafe UShortGuid = typeof(ushort).GUID;
        private GuidUnsafe IntGuid = typeof(int).GUID;
        private GuidUnsafe LongGuid = typeof(long).GUID;
        private GuidUnsafe FloatGuid = typeof(float).GUID;
        private GuidUnsafe DoubleGuid = typeof(double).GUID;
        private GuidUnsafe StringGuid = typeof(string).GUID;
        private GuidUnsafe GuidGuid = typeof(Guid).GUID;
        #endregion
        private Encoding Encoding;

        public PBinaryReader(Stream input) : base(input)
        {
            Encoding = Encoding.Default;
        }

        public PBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {
            Encoding = encoding;
        }

        public PBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            Encoding = encoding;
        }
        public virtual object ReadObject(Type ftype)
        {
            GuidUnsafe guid = ftype.GUID;
            if (guid == BoolGuid)
                return ReadBoolean();
            else if (guid == SByteGuid)
                return ReadSByte();
            else if (guid == ByteGuid)
                return ReadByte();
            else if (guid == ShortGuid)
                return ReadInt16();
            else if (guid == UShortGuid)
                return ReadUInt16();
            else if (guid == IntGuid)
                return ReadInt32();
            else if (guid == LongGuid)
                return ReadInt64();
            else if (guid == FloatGuid)
                return ReadSingle();
            else if (guid == DoubleGuid)
                return ReadDouble();
            else if (guid == StringGuid)
                return ReadString();
            else if (guid == GuidGuid)
                return ReadGuid();
            else 
                throw new NotImplementedException();
        }
        public virtual Guid ReadGuid()
        {
            return new Guid(ReadBytes(16));
        }
    }
}
