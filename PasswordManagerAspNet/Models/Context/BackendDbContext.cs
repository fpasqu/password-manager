using Microsoft.EntityFrameworkCore;
using PasswordManagerAspNet.Models.Entities;

namespace PasswordManagerAspNet.Core.Models.Context
{
    public class BackendDbContext : DbContext
    {
        public DbSet<Password> Passwords { get; set; }

        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
        {
        }
    }
}
