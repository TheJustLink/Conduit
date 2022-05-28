namespace Conduit.Net.Data
{
    public class Ref<T> where T : struct
    {
        public T Value;
        
        public static implicit operator T(Ref<T> wrapper) => wrapper.Value;
        public static implicit operator Ref<T>(T value) => new() { Value = value };
        public override string ToString() => Value.ToString();
    }
}