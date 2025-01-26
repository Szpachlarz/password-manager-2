using PasswordManager2.Interfaces;
using PasswordManager2.Models.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using PasswordManager2.Helpers;

namespace PasswordManager2.Services
{
    internal class PasswordService : IPasswordService
    {
        private readonly HttpClient _httpClient;
        private readonly string _encryptionKey;
        private readonly AesEncryptionHelper _aesHelper;

        public PasswordService(HttpClient httpClient, string encryptionKey, AesEncryptionHelper aesHelper)
        {
            _httpClient = httpClient;
            _encryptionKey = encryptionKey;
            _aesHelper = aesHelper;
        }

        public async Task<List<PasswordDto>> GetPasswordsAsync()
        {
            try
            {
                var id = await _httpClient.GetAsync("api/Authentication/me");
                var response = await _httpClient.GetAsync("api/Record/user" + id);
                response.EnsureSuccessStatusCode();

                var encryptedPasswords = await response.Content.ReadFromJsonAsync<List<PasswordDto>>();
                if (encryptedPasswords == null) return new List<PasswordDto>();

                foreach (var password in encryptedPasswords)
                {
                    password.Password = _aesHelper.Decrypt(password.Password, password.IV);
                }

                return encryptedPasswords;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve passwords", ex);
            }
        }

        public async Task<PasswordDto> GetPasswordByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Record/{id}");
                response.EnsureSuccessStatusCode();

                var password = await response.Content.ReadFromJsonAsync<PasswordDto>();
                return password ?? throw new Exception("Password not found");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve password with ID {id}", ex);
            }
        }

        public async Task<PasswordDto> AddPasswordAsync(PasswordDto password)
        {
            try
            {
                var (encryptedPassword, iv) = _aesHelper.Encrypt(password.Password);
                password.Password = encryptedPassword;
                password.IV = iv;

                var response = await _httpClient.PostAsJsonAsync("api/Record", password);
                response.EnsureSuccessStatusCode();

                var createdPassword = await response.Content.ReadFromJsonAsync<PasswordDto>();
                return createdPassword ?? throw new Exception("Failed to create password");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add password", ex);
            }
        }

        public async Task<PasswordDto> UpdatePasswordAsync(PasswordDto password)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Record/{password.Id}", password);
                response.EnsureSuccessStatusCode();

                var updatedPassword = await response.Content.ReadFromJsonAsync<PasswordDto>();
                return updatedPassword ?? throw new Exception("Failed to update password");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update password with ID {password.Id}", ex);
            }
        }

        public async Task<bool> DeletePasswordAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Record/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete password with ID {id}", ex);
            }
        }
    }
}
