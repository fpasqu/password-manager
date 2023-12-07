using PasswordManagerAspNet.Models.Entities;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManagerAspNet.Models.Repositories
{
    public class Functions : IFunctions
    {
        private readonly string? _iv = Environment.GetEnvironmentVariable("PM_IV_KEY");
        private readonly string? _salt = Environment.GetEnvironmentVariable("PM_SALT_KEY");

        public string Encrypt(string data, string key)
        {
            using (var aesAlg = Aes.Create())
            {
                var aesConfig = GetDerivedBytesConfig(key);
                aesAlg.Key = aesConfig.Item1;
                aesAlg.IV = aesConfig.Item2;

                using (var msEncrypt = new MemoryStream())
                {
                    using (var encryptor = aesAlg.CreateEncryptor())
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(data);
                    }
                    string encryptedData = Convert.ToBase64String(msEncrypt.ToArray());

                    return encryptedData;
                }
            }
        }

        public string Decrypt(string encryptedData, string key)
        {
            using (var aesAlg = Aes.Create())
            {
                var aesConfig = GetDerivedBytesConfig(key);
                aesAlg.Key = aesConfig.Item1;
                aesAlg.IV = aesConfig.Item2;

                using (var decryptor = aesAlg.CreateDecryptor())
                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedData)))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        private Tuple<byte[], byte[]> GetDerivedBytesConfig(string key)
        {
            byte[] ivBytes = Encoding.UTF8.GetBytes(_iv);
            byte[] saltBytes = Encoding.UTF8.GetBytes(_salt);
            using (var deriveBytes = new Rfc2898DeriveBytes(key, saltBytes, 1000, HashAlgorithmName.SHA512))
            {
                byte[] keyBytes = deriveBytes.GetBytes(16);
                return Tuple.Create(keyBytes, ivBytes);
            }
        }

        public Password EncryptDecryptPassword(Password p, string key, bool encrypt)
        {
            if (encrypt)
            {
                p.UserMail = Encrypt(p.UserMail, key);
                p.AccountName = Encrypt(p.AccountName, key);
                p.AccountEmail = Encrypt(p.AccountEmail, key);
                p.PasswordValue = Encrypt(p.PasswordValue, key);
                p.Notes = Encrypt(p.Notes, key);
            }
            else
            {
                p.UserMail = Decrypt(p.UserMail, key);
                p.AccountName = Decrypt(p.AccountName, key);
                p.AccountEmail = Decrypt(p.AccountEmail, key);
                p.PasswordValue = Decrypt(p.PasswordValue, key);
                p.Notes = Decrypt(p.Notes, key);
            }
            return p;
        }

        public string? GetCurrentUserEmail(ClaimsPrincipal user)
        {
            string userClaimEmailProp = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
            var userClaims = user.Identity as ClaimsIdentity;
            return userClaims?.FindFirst(userClaimEmailProp)?.Value;
        }
    }
}
