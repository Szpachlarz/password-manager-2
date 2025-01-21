using PasswordManager2.Interfaces;
using PasswordManager2.Models.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager2.Services
{
    internal class PasswordService : IPasswordService
    {
        private readonly HttpClient _httpClient;

        public PasswordService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PasswordDto>> GetPasswordsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/passwords");
                response.EnsureSuccessStatusCode();

                var passwords = await response.Content.ReadFromJsonAsync<List<PasswordDto>>();
                return passwords ?? new List<PasswordDto>();
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
                var response = await _httpClient.GetAsync($"api/passwords/{id}");
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
                var response = await _httpClient.PostAsJsonAsync("api/passwords", password);
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
                var response = await _httpClient.PutAsJsonAsync($"api/passwords/{password.Id}", password);
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
                var response = await _httpClient.DeleteAsync($"api/passwords/{id}");
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
