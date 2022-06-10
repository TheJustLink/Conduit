using System.IO;

namespace Conduit.Net.Data
{
    public struct SByte : ISerializable
    {
        public sbyte Value;
        
        public SByte(sbyte value) => Value = value;
        
        public void Read(Stream stream) => Value = (sbyte)stream.ReadByte();
        public void Write(Stream stream) => stream.WriteByte((byte)Value);

        public static implicit operator sbyte(SByte value) => value.Value;
        public static implicit operator SByte(sbyte value) => new(value);
    }
}