using System.IO;

namespace Conduit.Net.Data
{
    public struct Byte : ISerializable
    {
        public byte Value;
        
        public Byte(byte value) => Value = value;
        
        public void Read(Stream stream) => Value = (byte)stream.ReadByte();
        public void Write(Stream stream) => stream.WriteByte(Value);

        public static implicit operator byte(Byte value) => value.Value;
        public static implicit operator Byte(byte value) => new(value);
    }
}