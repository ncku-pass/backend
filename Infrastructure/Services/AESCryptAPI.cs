using Infrastructure.Services.Interface;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace Infrastructure.Services
{
    public class AESCryptAPI : IAESCryptAPI
    {
        private string _key { get; set; }
        private string _iv { get; set; }

        public AESCryptAPI()
        {
            this._key = Environment.GetEnvironmentVariable("IMAGE_KEY");
            this._iv = Environment.GetEnvironmentVariable("IMAGE_IV");
            //this._key = "iwMi02keNza9QaZr";
            //this._iv = "qjFpNkieP92nkpam";

        }

        public string Encrypt(string input)
        {
            var sault = GenerateRandomString(10);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(this._key);
                aes.IV = Encoding.UTF8.GetBytes(this._iv);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] encrypted;

                using (var ms = new System.IO.MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(input + sault);
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();
                    encrypted = ms.ToArray();
                }

                return Convert.ToBase64String(encrypted);
            }
        }

        public string Decrypt(string input)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(this._key);
                aes.IV = Encoding.UTF8.GetBytes(this._iv);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] encrypted = Convert.FromBase64String(input);
                byte[] decrypted;

                using (var ms = new System.IO.MemoryStream())
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(encrypted, 0, encrypted.Length);
                    cs.FlushFinalBlock();
                    decrypted = ms.ToArray();
                }

                var output = Encoding.UTF8.GetString(decrypted);

                return output.Substring(0, output.Length - 10);
            }
        }


        public string GenerateRandomString(int length)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var result = new string(Enumerable.Range(0, length)
                                              .Select(i => chars[random.Next(chars.Length)])
                                              .ToArray());
            return result;
        }
    }
}