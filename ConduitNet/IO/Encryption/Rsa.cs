using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Conduit.Net.IO.Encryption
{
    public static class Rsa
    {
        public static readonly byte[] PublicKey;

        private static readonly RSA s_rsa;

        static Rsa()
        {
            s_rsa = RSA.Create(1024);
            PublicKey = GetPublicKey(s_rsa);
        }

        private static byte[] GetPublicKey(RSA rsa)
        {
            return X509SignatureGenerator.CreateForRSA(rsa, RSASignaturePadding.Pkcs1)
                .PublicKey.ExportSubjectPublicKeyInfo();
        }

        public static byte[] Encrypt(byte[] data)
        {
            return s_rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
        }
        public static byte[] Decrypt(byte[] data)
        {
            return s_rsa.Decrypt(data, RSAEncryptionPadding.Pkcs1);
        }
    }
}