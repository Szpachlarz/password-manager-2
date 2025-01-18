using PasswordManager2Api.Models;

namespace PasswordManager2Api.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetByUsernameAsync(string username);
        Task<Account> GetByIdAsync(string userId);
        Task<bool> CreateAsync(Account user);
    }
}
