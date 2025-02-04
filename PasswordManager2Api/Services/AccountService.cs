using PasswordManager2Api.Helpers;
using PasswordManager2Api.Interfaces;
using PasswordManager2Api.Models;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager2Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ServiceResult> ValidateUserAsync(string username, string password)
        {
            var user = await _accountRepository.GetByUsernameAsync(username);
            if (user == null)
                return new ServiceResult(false, "Nieprawidłowy login lub hasło.");

            var hashedPassword = HashPassword(password, user.Salt);
            if (hashedPassword != user.PasswordHash)
                return new ServiceResult(false, "Nieprawidłowy login lub hasło.");

            return new ServiceResult(true, "Zalogowano.");
        }

        //public async Task<Account> GetUserByIdAsync(string userId)
        //{
        //    return await _accountRepository.GetByIdAsync(userId);
        //}

        public async Task<ServiceResult> CreateUserAsync(string username, string password)
        {
            var existingUser = await _accountRepository.GetByUsernameAsync(username);
            if (existingUser != null)
            {
                return new ServiceResult(false, "Użytkownik już istnieje.");
            }

            var salt = GenerateSalt();

            var user = new Account
            {
                UserName = username,
                Salt = salt,
                PasswordHash = HashPassword(password, salt)
            };

            await _accountRepository.CreateAsync(user);

            return new ServiceResult(true, "Zarejestrowano.");
        }

        private string GenerateSalt()
        {
            var buffer = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }
            return Convert.ToBase64String(buffer);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = $"{password}{salt}";
                var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
