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
            if (guid == Types.BoolGuid)
                return ReadBoolean();
            else if (guid == Types.SByteGuid)
                return ReadSByte();
            else if (guid == Types.ByteGuid)
                return ReadByte();
            else if (guid == Types.ShortGuid)
                return ReadInt16();
            else if (guid == Types.UShortGuid)
                return ReadUInt16();
            else if (guid == Types.IntGuid)
                return ReadInt32();
            else if (guid == Types.LongGuid)
                return ReadInt64();
            else if (guid == Types.FloatGuid)
                return ReadSingle();
            else if (guid == Types.DoubleGuid)
                return ReadDouble();
            else if (guid == Types.StringGuid)
                return ReadString();
            else if (guid == Types.GuidGuid)
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
