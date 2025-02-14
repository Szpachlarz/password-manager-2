using Microsoft.Extensions.Configuration;
using PasswordManager2.Interfaces;
using PasswordManager2.Models.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PasswordManager2.Services
{
    internal class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private bool _isAuthenticated;
        private CookieContainer _cookieContainer;

        public bool IsAuthenticated => _isAuthenticated;

        public AuthService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            try
            {
                var request = new AuthDto
                {
                    Username = username,
                    Password = password
                };

                var response = await _httpClient.PostAsJsonAsync("authentication/login", request);

                if (response.IsSuccessStatusCode)
                {
                    _isAuthenticated = true;
                    return new LoginResult { Success = true };
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                _isAuthenticated = false;
                return new LoginResult
                {
                    Success = false,
                    Message = errorMessage ?? "Login failed"
                };
            }
            catch (Exception ex)
            {
                _isAuthenticated = false;
                return new LoginResult
                {
                    Success = false,
                    Message = "Unable to connect to the server. Please check your connection."
                };
            }
        }

        public async Task<RegisterResult> RegisterAsync(string username, string password)
        {
            try
            {
                var request = new AuthDto
                {
                    Username = username,
                    Password = password
                };

                var response = await _httpClient.PostAsJsonAsync("authentication/register", request);

                if (response.IsSuccessStatusCode)
                {
                    return new RegisterResult { Success = true };
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                return new RegisterResult
                {
                    Success = false,
                    Message = errorMessage ?? "Register failed"
                };
            }
            catch (Exception ex)
            {
                return new RegisterResult
                {
                    Success = false,
                    Message = "Unable to connect to the server. Please check your connection."
                };
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync("authentication/logout", null);
                if (response.IsSuccessStatusCode)
                {
                    _isAuthenticated = false;
                    _cookieContainer = new CookieContainer();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new AuthenticationException("Logout failed", ex);
            }
        }

        public async Task<string> GetCurrentUserAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("authentication/me");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    if (result.TryGetProperty("username", out var usernameElement))
                    {
                        return usernameElement.GetString();
                    }
                }

                _isAuthenticated = false;
                throw new AuthenticationException("User not authenticated");
            }
            catch (Exception ex)
            {
                _isAuthenticated = false;
                throw new AuthenticationException("Failed to get current user", ex);
            }
        }
    }
}
