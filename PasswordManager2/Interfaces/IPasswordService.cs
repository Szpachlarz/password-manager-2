using PasswordManager2.Models.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager2.Interfaces
{
    internal interface IPasswordService
    {
        Task<List<PasswordDto>> GetPasswordsAsync();
        Task<PasswordDto> GetPasswordByIdAsync(int id);
        Task<PasswordDto> AddPasswordAsync(PasswordDto password);
        Task<PasswordDto> UpdatePasswordAsync(PasswordDto password);
        Task<bool> DeletePasswordAsync(int id);
    }
}
