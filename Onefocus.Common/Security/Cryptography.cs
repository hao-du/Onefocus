using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Security
{
    public static class Cryptography
    {
        public static string Encrypt(string data, string? securityKeyString = default)
        {
            using var aes = Aes.Create();
            aes.Key = CreateSecurityKey(securityKeyString);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var iv = new byte[16];
            new Random().NextBytes(iv);
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor();

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using var streamWriter = new StreamWriter(cryptoStream);

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string Decrypt(string data, string? securityKeyString = default)
        {
            var key = CreateSecurityKey(securityKeyString);

            using var aes = Aes.Create();
            aes.KeySize = key.Length;
            aes.Key = 
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var iv = new byte[16];
            new Random().NextBytes(iv);
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor();

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write);
            using var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }

        private static byte[] CreateSecurityKey(string securityKeyString)
        {
            Rfc2898DeriveBytes pwdGen = Rfc2898DeriveBytes.Pbkdf2(securityKeyString, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }, 1000);

            return Encoding.UTF8.GetBytes(saltedSecurityKeyString, );
        }
    }
}
