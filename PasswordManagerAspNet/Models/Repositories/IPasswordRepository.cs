using PasswordManagerAspNet.Models.Entities;

namespace PasswordManagerAspNet.Models.Repositories
{
    public interface IPasswordRepository
    {
        public Task<List<Password>> GetAllPasswordsAsync();
        public Task<List<Password>> GetPasswordsForUserAsync(string userMail);
        public Task<Password> GetPasswordByIdAsync(Guid id);
        public Task<Password> CreatePasswordAsync(Password p);
        public Task<Password> DeletePasswordAsync(Password password);
        public Task<Password> UpdatePasswordAsync(Password password);
    }
}
