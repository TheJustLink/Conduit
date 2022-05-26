using System;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Extensions
{
    public static class StringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static Guid GetGuid(this string @string) => new(@string.GetMD5Hash());
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static byte[] GetMD5Hash(this string @string) => MD5.HashData(@string.GetBytes());
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(this string @string) => Encoding.UTF8.GetBytes(@string);
    }
}