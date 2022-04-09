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
        public virtual object ReadObject(Type ftype, out MemoryStream readed)
        {
            readed = new MemoryStream();
            GuidUnsafe guid = ftype.GUID;
            if (guid == BoolGuid)
            {
                byte[] data = ReadBytes(1);
                bool v = data[0] == 1;
                readed.Write(data);
                return v;
            }
            else if (guid == SByteGuid)
            {
                byte[] data = ReadBytes(1);
                sbyte v = (sbyte)data[0];
                readed.Write(data);
                return v;
            }
            else if (guid == ByteGuid)
            {
                byte[] data = ReadBytes(1);
                byte v = data[0];
                readed.Write(data);
                return v;
            }
            else if (guid == ShortGuid)
            {
                byte[] data = ReadBytes(2);
                short v = Convert.ToInt16(data);
                readed.Write(data);
                return v;
            }
            else if (guid == UShortGuid)
            {
                byte[] data = ReadBytes(2);
                ushort v = Convert.ToUInt16(data);
                readed.Write(data);
                return v;
            }
            else if (guid == IntGuid)
            {
                byte[] data = ReadBytes(2);
                int v = Convert.ToInt32(data);
                readed.Write(data);
                return v;
            }
            else if (guid == LongGuid)
            {
                byte[] data = ReadBytes(2);
                long v = Convert.ToInt64(data);
                readed.Write(data);
                return v;
            }
            else if (guid == FloatGuid)
            {
                byte[] data = ReadBytes(2);
                float v = Convert.ToSingle(data);
                readed.Write(data);
                return v;
            }
            else if (guid == DoubleGuid)
            {
                byte[] data = ReadBytes(2);
                double v = Convert.ToDouble(data);
                readed.Write(data);
                return v;
            }
            else if (guid == StringGuid)
            {
                var str = ReadString();
                readed.Write(Encoding.GetBytes(str));
                return str;
            }
            else if (guid == GuidGuid)
            {
                var g = ReadGuid(out byte[] r);
                readed.Write(r);
                return g;
            }
            else
                throw new NotImplementedException();
        }

        public virtual Guid ReadGuid(out byte[] readed)
        {
            readed = ReadBytes(16);
            return new Guid(readed);
        }
        public virtual Guid ReadGuid()
        {
            return new Guid(ReadBytes(16));
        }
    }
}
