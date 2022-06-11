using Conduit.Net.Data;

using Xunit;

namespace Conduit.Net.Tests.Data
{
    public class SByteTests : SerializableTests<SByte>
    {
        [Fact]
        public void ReadWrite() => ReadWriteInternal(SByte.MinValue, SByte.MaxValue);
    }
}