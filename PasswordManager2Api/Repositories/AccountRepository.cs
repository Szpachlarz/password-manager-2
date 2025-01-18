using Microsoft.EntityFrameworkCore;
using PasswordManager2Api.Data;
using PasswordManager2Api.Interfaces;
using PasswordManager2Api.Models;
using System;

namespace PasswordManager2Api.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Account> GetByUsernameAsync(string username)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<Account> GetByIdAsync(string userId)
        {
            return await _context.Accounts.FindAsync(userId);
        }

        public async Task<bool> CreateAsync(Account user)
        {
            _context.Accounts.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
