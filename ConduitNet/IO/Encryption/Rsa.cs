using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static byte[] GetPublicKey(RSA rsa)
        {
            return X509SignatureGenerator.CreateForRSA(rsa, RSASignaturePadding.Pkcs1)
                .PublicKey.ExportSubjectPublicKeyInfo();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static byte[] Encrypt(byte[] data)
        {
            return s_rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static byte[] Decrypt(byte[] data)
        {
            return s_rsa.Decrypt(data, RSAEncryptionPadding.Pkcs1);
        }
    }
}