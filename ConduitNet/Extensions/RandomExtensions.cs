using System;

namespace Conduit.Net.Extensions
{
    public static class RandomExtensions
    {
        public static byte[] NextBytes(this Random random, int count)
        {
            var data = new byte[count];
            random.NextBytes(data);

            return data;
        }
    }
}