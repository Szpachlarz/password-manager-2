using PasswordManager2Api.Helpers;
using PasswordManager2Api.Models;

namespace PasswordManager2Api.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult> ValidateUserAsync(string username, string password);
        Task<ServiceResult> CreateUserAsync(string username, string password);
        //Task<ServiceResult> GetUserByIdAsync(string userId);
    }
}
