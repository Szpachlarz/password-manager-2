using Microsoft.EntityFrameworkCore;
using PasswordManager2Api.Models;

namespace PasswordManager2Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
                : base(dbContextOptions)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Record> Records { get; set; }
    }
}
