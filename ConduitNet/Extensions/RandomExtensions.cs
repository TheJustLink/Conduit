using System;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Extensions
{
    public static class RandomExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static byte[] NextBytes(this Random random, int count)
        {
            var data = new byte[count];
            random.NextBytes(data);

            return data;
        }
    }
}