using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Data
{
    public struct Boolean : ISerializable, IComparable, IComparable<Boolean>, IEquatable<Boolean>
    {
        public static readonly string FalseString = bool.FalseString;
        public static readonly string TrueString = bool.TrueString;

        public static readonly Boolean False = new(false);
        public static readonly Boolean True = new(true);

        public bool Value;

        public Boolean(bool value) => Value = value;
        
        public void Read(Stream stream) => Value = stream.ReadByte() > 0;
        public void Write(Stream stream) => stream.WriteByte(Value ? (byte)1 : (byte)0);

        public int CompareTo(object obj) => CompareTo(Unsafe.Unbox<Boolean>(obj));
        public int CompareTo(Boolean other) => Value.CompareTo(other.Value);

        public override string ToString() => Value.ToString();
        public override bool Equals(object obj) => Equals(Unsafe.Unbox<Boolean>(obj));
        public bool Equals(Boolean other) => Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator bool(Boolean value) => value.Value;
        public static implicit operator Boolean(bool value) => new(value);

        public static bool operator ==(Boolean left, Boolean right) => left.Equals(right);
        public static bool operator !=(Boolean left, Boolean right) => !left.Equals(right);
    }
}