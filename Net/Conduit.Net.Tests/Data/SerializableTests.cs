using System.IO;

using Xunit;

using Conduit.Net.Data;

namespace Conduit.Net.Tests.Data
{
    public class SerializableTests<T>
        where T : struct, ISerializable
    {
        protected void ReadWriteInternal(params T[] values)
        {
            using var memory = new MemoryStream();

            foreach (var value in values)
            {
                value.Write(memory);
                memory.Position = 0;

                var result = new T();
                result.Read(memory);
                memory.Position = 0;

                Assert.Equal(value, result);
            }
        }
    }
}