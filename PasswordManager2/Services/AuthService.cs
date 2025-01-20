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

        public AuthService(IConfiguration configuration)
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _cookieContainer = new CookieContainer();

            var handler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer,
                UseCookies = true
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }

        public async Task<bool> LoginAsync(string username, string password)
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
                    return true;
                }

                var error = await response.Content.ReadFromJsonAsync<AuthResponse>();
                throw new Exception(error?.Message ?? "Login failed");
            }
            catch (Exception ex)
            {
                _isAuthenticated = false;
                throw new AuthenticationException("Login failed", ex);
            }
        }

        public async Task<bool> RegisterAsync(string username, string password)
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
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    return true;
                }

                var error = await response.Content.ReadFromJsonAsync<AuthResponse>();
                throw new Exception(error?.Message ?? "Registration failed");
            }
            catch (Exception ex)
            {
                throw new AuthenticationException("Registration failed", ex);
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
                    var result = await response.Content.ReadFromJsonAsync<dynamic>();
                    return result.username;
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
