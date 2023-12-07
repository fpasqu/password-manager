using PasswordManagerAspNet.Models.Entities;
using System.Security.Claims;

namespace PasswordManagerAspNet.Models.Repositories
{
    public interface IFunctions
    {
        public string Encrypt(string data, string key);
        public string Decrypt(string encryptedData, string key);
        public Password EncryptDecryptPassword(Password p, string key, bool encrypt);
        public string? GetCurrentUserEmail(ClaimsPrincipal user);
    }
}
