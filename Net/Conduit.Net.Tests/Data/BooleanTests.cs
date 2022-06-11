using Xunit;

using Conduit.Net.Data;

namespace Conduit.Net.Tests.Data
{
    public class BooleanTests : SerializableTests<Boolean>
    {
        [Fact]
        public void ReadWrite() => ReadWriteInternal(Boolean.False, Boolean.True);
    }
}