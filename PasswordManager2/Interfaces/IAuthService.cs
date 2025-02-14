using PasswordManager2.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager2.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(string username, string password);
        Task<RegisterResult> RegisterAsync(string username, string password);
        Task<bool> LogoutAsync();
        Task<string> GetCurrentUserAsync();
        bool IsAuthenticated { get; }
    }
}
