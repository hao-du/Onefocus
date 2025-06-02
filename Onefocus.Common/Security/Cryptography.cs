using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Onefocus.Common.Security
{
    public static class Cryptography
    {
        public static async Task<string> Encrypt(string data, string securityKey)
        {
            using var aes = Aes.Create();
            aes.Key = CreateSecurityKey(securityKey);
            aes.IV = new byte[16];
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);
            await sw.WriteAsync(data);
            return Convert.ToBase64String(ms.ToArray());
        }

        public static async Task<string> Decrypt(string data, string securityKey)
        {
            using var aes = Aes.Create();
            aes.Key = CreateSecurityKey(securityKey);
            aes.IV = new byte[16];
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(data));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return await sr.ReadToEndAsync();
        }

        private static byte[] CreateSecurityKey(string securityKeyString) => [.. Encoding.UTF8.GetBytes(securityKeyString).Take(16)];

        public static SymmetricSecurityKey CreateSymmetricSecurityKey(string symmetricSecurityKey) => new(Encoding.UTF8.GetBytes(symmetricSecurityKey));
    }
}
