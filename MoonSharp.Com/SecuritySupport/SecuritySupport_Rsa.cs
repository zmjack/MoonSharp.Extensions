using System;
using System.Security.Cryptography;
using System.Text;

namespace MoonSharp.Extensions
{
    public partial class SecuritySupport : LuaSupport
    {
        public static string RsaEncryptString(string pemString, string plaintext)
        {
            return Convert.ToBase64String(
                RsaEncrypt(pemString, Encoding.UTF8.GetBytes(plaintext)));
        }

        public static string RsaDecryptString(string pemString, string ciphertext)
        {
            return Encoding.UTF8.GetString(
                RsaDecrypt(pemString, Convert.FromBase64String(ciphertext)));
        }

        public static byte[] RsaEncrypt(string pemString, byte[] plaintext)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromPemStringCore(pemString, false);
            return rsa.Encrypt(plaintext, false);
        }

        public static byte[] RsaDecrypt(string pemString, byte[] ciphertext)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromPemStringCore(pemString, true);
            return rsa.Decrypt(ciphertext, false);
        }

    }

}
