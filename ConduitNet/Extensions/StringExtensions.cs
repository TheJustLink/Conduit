using System;
using System.Security.Cryptography;
using System.Text;

namespace Conduit.Net.Extensions
{
    public static class StringExtensions
    {
        public static Guid GetGuid(this string @string) => new(@string.GetMD5Hash());
        public static byte[] GetMD5Hash(this string @string) => MD5.HashData(@string.GetBytes());
        public static byte[] GetBytes(this string @string) => Encoding.UTF8.GetBytes(@string);
    }
}