using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Security.Cryptography;

namespace PasswordManager2.Helpers
{
    public class AesEncryptionHelper
    {
        private readonly byte[] _key;

        public AesEncryptionHelper(string encryptionKey)
        {
            _key = Encoding.UTF8.GetBytes(encryptionKey.PadRight(32).Substring(0, 32));
        }

        public (string EncryptedData, string IV) Encrypt(string plaintext)
        {
            using var aes = Aes.Create();
            aes.GenerateIV();
            aes.Key = _key;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            var encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

            return (Convert.ToBase64String(encryptedBytes), Convert.ToBase64String(aes.IV));
        }

        public string Decrypt(string encryptedData, string iv)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = Convert.FromBase64String(iv);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var encryptedBytes = Convert.FromBase64String(encryptedData);
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
