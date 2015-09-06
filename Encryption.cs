using System;
using System.Security.Cryptography;
using System.Text;

namespace UnknownREEncrypter
{
    internal class Encryption
    {
        public static RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));

            return new RijndaelManaged
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.ISO10126,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }

        public static byte[] Encrypt(byte[] input, string password)
        {
            try
            {
                return GetRijndaelManaged(password).CreateEncryptor().TransformFinalBlock(input, 0, input.Length);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void write(String message = "")
        {
            Console.WriteLine(message);
        }

        public static byte[] Decrypt(byte[] input, string password)
        {
            try
            {
                return GetRijndaelManaged(password).CreateDecryptor().TransformFinalBlock(input, 0, input.Length);
            }
            catch (CryptographicException)
            {
                write("Error: Invalid password specified!");
                write();
                return null;
            }
        }
    }
}