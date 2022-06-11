using Xunit;

using Conduit.Net.Data;

namespace Conduit.Net.Tests.Data
{
    public class ByteTests : SerializableTests<Byte>
    {
        [Fact]
        public void ReadWrite() => ReadWriteInternal(Byte.MinValue, Byte.MaxValue);
    }
}