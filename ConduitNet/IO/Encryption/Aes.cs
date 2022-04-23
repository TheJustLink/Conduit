using System.IO;
using System.Security.Cryptography;

namespace Conduit.Net.IO.Encryption
{
    public static class Aes
    {
        public static CryptoStream CreateEncryptionStream(Stream input, byte[] key)
        {
            return new CryptoStream(input, CreateEncryptor(key), CryptoStreamMode.Write);
        }
        public static CryptoStream CreateDecryptionStream(Stream input, byte[] key)
        {
            return new CryptoStream(input, CreateDecryptor(key), CryptoStreamMode.Read);
        }

        public static ICryptoTransform CreateEncryptor(byte[] key)
        {
            return Create(key).CreateEncryptor();
        }
        public static ICryptoTransform CreateDecryptor(byte[] key)
        {
            return Create(key).CreateDecryptor();
        }

        public static System.Security.Cryptography.Aes Create(byte[] key)
        {
            var aes = System.Security.Cryptography.Aes.Create();

            aes.Mode = CipherMode.CFB;
            aes.Padding = PaddingMode.None;

            aes.FeedbackSize = 8;
            aes.KeySize = 128;
            aes.BlockSize = 128;

            aes.Key = key;
            aes.IV = key;

            return aes;
        }
    }
}