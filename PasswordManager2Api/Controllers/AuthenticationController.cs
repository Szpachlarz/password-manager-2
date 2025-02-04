using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PasswordManager2Api.Interfaces;
using PasswordManager2Api.Data;
using Microsoft.AspNetCore.Identity.Data;
using PasswordManager2Api.Models;
using Microsoft.AspNetCore.Identity;
using PasswordManager2Api.Repositories;

namespace PasswordManager2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAccountRepository _accountRepository;

        public AuthenticationController(IAccountService accountService, IAccountRepository accountRepository)
        {
            _accountService = accountService;
            _accountRepository = accountRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto request)
        {
            var user = await _accountRepository.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

            var result = await _accountService.ValidateUserAsync(request.Username, request.Password);
            if (!result.IsSuccess)
            {
                return Unauthorized(result.Message);
            }            

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Ok(new { message = "Login successful" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthDto request)
        {
            var result = await _accountService.CreateUserAsync(request.Username, request.Password);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logout successful" });
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "User not found or not authenticated" });
            }
            var username = User.Identity.Name;
            var user = await _accountRepository.GetByUsernameAsync(username);
            return Ok(new { username = username, id = user.Id });
        }
    }
}
