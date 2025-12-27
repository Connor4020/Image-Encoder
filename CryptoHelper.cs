using System;
using System.Security.Cryptography;
using System.Text;

namespace Barton___Y2_Project
{
    internal static class CryptoHelper
    {



        // Encrypts string and returns it.
        // Needs password and string to be encrypted.
        public static string EncryptString(string password, string plaintext)
        {

            // TODO: Implement input validation.
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            var key = new Rfc2898DeriveBytes(password, salt, 10000);

            // Creates AES object and attributes key and Initial vector.
            using Aes aes = Aes.Create();
            aes.Key = key.GetBytes(32);
            aes.GenerateIV();
            var encryptor = aes.CreateEncryptor();

            using MemoryStream memStream = new MemoryStream();
            // Store salt and IV.
            memStream.Write(aes.IV, 0, aes.IV.Length); 
            memStream.Write(salt, 0, salt.Length);

            using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
            {
                byte[] hiddenMessageBytes = Encoding.UTF8.GetBytes(plaintext);
                cryptoStream.Write(hiddenMessageBytes, 0, hiddenMessageBytes.Length);
            }

            return Convert.ToBase64String(memStream.ToArray());
        }



        // Decrypts inputted cipher text and returns it as plaintext.
        // Requires password.
        // Cipher text string needs to be base64 (text).
        
        public static string? DecryptString(string password, string cipherText)
        {
            Console.WriteLine(cipherText);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Creates ne byte lists for iv and salt.
            byte[] iv = new byte[16];
            byte[] salt = new byte[16];

            // Copys into byte arrays.
            Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
            Array.Copy(cipherBytes, iv.Length, salt, 0, salt.Length);

            using var key = new Rfc2898DeriveBytes(password, salt, 10000);
            using Aes aes = Aes.Create();

            aes.Key = key.GetBytes(32);
            aes.IV = iv;

            using MemoryStream memStream = new MemoryStream(cipherBytes, iv.Length + salt.Length, cipherBytes.Length - salt.Length - iv.Length);
            using CryptoStream cryptoStream = new CryptoStream(memStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader reader = new StreamReader(cryptoStream, Encoding.UTF8);

            return reader.ReadToEnd();

        }
    }
}