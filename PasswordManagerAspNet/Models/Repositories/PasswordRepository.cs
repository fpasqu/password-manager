using Microsoft.EntityFrameworkCore;
using PasswordManagerAspNet.Core.Models.Context;
using PasswordManagerAspNet.Models.Entities;

namespace PasswordManagerAspNet.Models.Repositories
{
    public class PasswordRepository : IPasswordRepository
    {
        private readonly BackendDbContext _context;

        public PasswordRepository(BackendDbContext context)
        {
            _context = context;
        }

        //get
        public async Task<List<Password>> GetAllPasswordsAsync()
        {
            return await _context.Passwords.ToListAsync();
        }

        //get by email
        public async Task<List<Password>> GetPasswordsForUserAsync(string email)
        {
            return await _context.Passwords.Where(x => x.UserMail == email).ToListAsync();
        }

        //get by id
        public async Task<Password> GetPasswordByIdAsync(Guid id)
        {
            return await _context.Passwords.FindAsync(id.ToString());
        }

        //post
        public async Task<Password> CreatePasswordAsync(Password password)
        {
            password.Id = Guid.NewGuid().ToString();
            await _context.Passwords.AddAsync(password);
            await _context.SaveChangesAsync();
            return password;
        }

        //delete
        public async Task<Password> DeletePasswordAsync(Password password)
        {
            _context.Passwords.Remove(password);
            await _context.SaveChangesAsync();
            return password;
        }

        //patch
        public async Task<Password> UpdatePasswordAsync(Password password)
        {
            _context.Passwords.Update(password);
            await _context.SaveChangesAsync();
            return password;
        }
    }
}
