using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MoonSharp.Extensions
{
    public partial class SecuritySupport : LuaSupport
    {
        public const int AES_IV_LENGTH = 16;

        [LuaFunction("en-US", "Using AES to encrypt string data.")]
        [LuaFunction("zn-CN", "使用AES加密字符串")]
        public static string AesEncryptString(string plaintext, string key)
        {
            return Convert.ToBase64String(
                AesEncrypt(Encoding.UTF8.GetBytes(plaintext), Encoding.UTF8.GetBytes(key)));
        }

        [LuaFunction("en-US", "Using AES to decrypt data.")]
        [LuaFunction("zn-CN", "使用AES解密数据")]
        public static string AesDecryptString(string base64_ciphertextWithIV, string key)
        {
            return Encoding.UTF8.GetString(
                AesDecrypt(Convert.FromBase64String(base64_ciphertextWithIV), Encoding.UTF8.GetBytes(key)));
        }

        public static byte[] AesEncrypt(byte[] plaintext, byte[] key)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                var iv = aes.IV;
                using (var encryptor = aes.CreateEncryptor(key, iv))
                {
                    using (var memory = new MemoryStream())
                    {
                        using (var crypto = new CryptoStream(memory, encryptor, CryptoStreamMode.Write))
                        {
                            var writer = new BinaryWriter(crypto);
                            writer.Write(plaintext);
                        }

                        var ciphertext = memory.ToArray();
                        var ciphertextWithIV = new byte[iv.Length + ciphertext.Length];

                        Buffer.BlockCopy(iv, 0, ciphertextWithIV, 0, iv.Length);
                        Buffer.BlockCopy(ciphertext, 0, ciphertextWithIV, iv.Length, ciphertext.Length);

                        return ciphertextWithIV;
                    }
                }
            }
        }

        public static byte[] AesDecrypt(byte[] ciphertextWithIV, byte[] key)
        {
            var iv = new byte[AES_IV_LENGTH];
            var ciphertext = new byte[ciphertextWithIV.Length - AES_IV_LENGTH];

            Buffer.BlockCopy(ciphertextWithIV, 0, iv, 0, AES_IV_LENGTH);
            Buffer.BlockCopy(ciphertextWithIV, AES_IV_LENGTH, ciphertext, 0, ciphertext.Length);

            using (var aesAlg = new AesCryptoServiceProvider())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    using (var memory = new MemoryStream(ciphertext))
                    {
                        using (var crypto = new CryptoStream(memory, decryptor, CryptoStreamMode.Read))
                        {
                            var reader = new BinaryReader(crypto);
                            var buffer = reader.ReadBytes(ciphertext.Length);
                            return buffer;
                        }
                    }
                }
            }
        }

    }

}
