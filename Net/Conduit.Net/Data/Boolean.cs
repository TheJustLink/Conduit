using System.IO;

namespace Conduit.Net.Data
{
    public struct Boolean : ISerializable
    {
        public bool Value;

        public Boolean(bool value) => Value = value;
        
        public void Read(Stream stream) => Value = stream.ReadByte() > 0;
        public void Write(Stream stream) => stream.WriteByte(Value ? (byte)1 : (byte)0);

        public static implicit operator bool(Boolean value) => value.Value;
        public static implicit operator Boolean(bool value) => new(value);
    }
}