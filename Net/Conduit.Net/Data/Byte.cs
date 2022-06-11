using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Data
{
    public struct Byte : ISerializable, IComparable, IComparable<Byte>, IEquatable<Byte>
    {
        public static readonly Byte MinValue = byte.MinValue;
        public static readonly Byte MaxValue = byte.MaxValue;

        public byte Value;
        
        public Byte(byte value) => Value = value;

        public void Read(Stream stream) => Value = (byte)stream.ReadByte();
        public void Write(Stream stream) => stream.WriteByte(Value);

        public int CompareTo(object obj) => CompareTo(Unsafe.Unbox<Byte>(obj));
        public int CompareTo(Byte other) => Value.CompareTo(other.Value);

        public override string ToString() => Value.ToString();
        public override bool Equals(object obj) => Equals(Unsafe.Unbox<Byte>(obj));
        public bool Equals(Byte other) => Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator byte(Byte value) => value.Value;
        public static implicit operator Byte(byte value) => new(value);

        public static bool operator ==(Byte left, Byte right) => left.Equals(right);
        public static bool operator !=(Byte left, Byte right) => !left.Equals(right);
    }
}