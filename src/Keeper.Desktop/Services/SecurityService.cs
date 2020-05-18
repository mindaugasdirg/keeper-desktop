using Keeper.Desktop.Properties;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Keeper.Desktop.Services
{
    public class SecurityService
    {
        private const string usernameSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

        public string Encrypt(string message)
        {
            var key = Settings.Default.SecretKey;
            var saltString = Settings.Default.ClientSecret;
            var encrypted = string.Empty;
            using (var encryptor = Aes.Create())
            {
                var keyGenerator = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes(saltString));
                encryptor.Key = keyGenerator.GetBytes(32);
                encryptor.IV = keyGenerator.GetBytes(16);
                using(var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(message);
                        }
                        encrypted = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
            return encrypted;
        }

        public string Decrypt(string message)
        {
            var key = Settings.Default.SecretKey;
            var saltString = Settings.Default.ClientSecret;
            var decrypted = string.Empty;
            using (var encryptor = Aes.Create())
            {
                var keyGenerator = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes(saltString));
                encryptor.Key = keyGenerator.GetBytes(32);
                encryptor.IV = keyGenerator.GetBytes(16);
                using (var memoryStream = new MemoryStream(Convert.FromBase64String(message)))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cryptoStream))
                        {
                            decrypted = reader.ReadToEnd();
                        }
                    }
                }
            }
            return decrypted;
        }

        public void CreateSecrets()
        {
            var clientSecret = GetRandomString(15, 20);
            var secretKey = GetRandomString(5, 10);

            Settings.Default.ClientSecret = clientSecret;
            Settings.Default.SecretKey = secretKey;
            Settings.Default.Save();
        }

        public string GetRandomString(int minLength, int maxLength)
        {
            var length = GetRandomInt(minLength, maxLength);
            var str = new StringBuilder();
            for (int i = 0; i < length; ++i)
            {
                str.Append(GetRandomSymbol());
            }
            return str.ToString();
        }

        private char GetRandomSymbol() => usernameSymbols[GetRandomByte(Convert.ToByte(usernameSymbols.Length))];

        private int GetRandomInt(int min, int max)
        {
            var boundries = BitConverter.GetBytes(max - min);
            var value = new byte[boundries.Length];
            for (int i = 0; i < value.Length; ++i)
            {
                value[i] = GetRandomByte(boundries[i]);
            }
            return BitConverter.ToInt32(value) + min;
        }

        private byte GetRandomByte(byte max)
        {
            if (max == 0) // interval is [0; 0]
                return 0;
            var value = new byte[1];
            var fair = false;
            while (!fair)
            {
                rand.GetBytes(value);
                fair = IsFair(value[0], max);
            }
            return (byte)((int)value[0] % (int)max);
        }

        private bool IsFair(byte value, byte maxValue)
        {
            int sets = byte.MaxValue / maxValue;
            return value < sets * maxValue;
        }
    }
}
