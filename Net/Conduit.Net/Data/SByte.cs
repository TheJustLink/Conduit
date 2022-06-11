using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Data
{
    public struct SByte : ISerializable, IComparable, IComparable<SByte>, IEquatable<SByte>
    {
        public static readonly SByte MinValue = sbyte.MinValue;
        public static readonly SByte MaxValue = sbyte.MaxValue;

        public sbyte Value;
        
        public SByte(sbyte value) => Value = value;

        public void Read(Stream stream) => Value = (sbyte)stream.ReadByte();
        public void Write(Stream stream) => stream.WriteByte((byte)Value);

        public int CompareTo(object obj) => CompareTo(Unsafe.Unbox<SByte>(obj));
        public int CompareTo(SByte other) => Value.CompareTo(other.Value);

        public override string ToString() => Value.ToString();
        public override bool Equals(object obj) => Equals(Unsafe.Unbox<SByte>(obj));
        public bool Equals(SByte other) => Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator sbyte(SByte value) => value.Value;
        public static implicit operator SByte(sbyte value) => new(value);

        public static bool operator ==(SByte left, SByte right) => left.Equals(right);
        public static bool operator !=(SByte left, SByte right) => !left.Equals(right);
    }
}