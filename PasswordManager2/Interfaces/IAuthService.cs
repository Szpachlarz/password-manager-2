using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager2.Interfaces
{
    internal interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string password);
        Task<bool> LogoutAsync();
        Task<string> GetCurrentUserAsync();
        bool IsAuthenticated { get; }
    }
}
