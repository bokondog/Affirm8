using Microsoft.AspNetCore.Mvc;
using KindWordsApi.Models;
using KindWordsApi.Services;
using System.Security.Cryptography;
using System.Text;

namespace KindWordsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataService _dataService;

        public AuthController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Check if user already exists
                var existingUser = _dataService.GetUserByEmail(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "User with this email already exists" });
                }

                // Hash password
                var passwordHash = HashPassword(request.Password);

                // Create user
                var user = _dataService.CreateUser(request.Email, passwordHash, request.NickName);

                // Create response
                var response = new LoginResponse
                {
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        NickName = user.NickName,
                        JoinedAt = user.JoinedAt
                    },
                    Token = GenerateToken(user.Id) // Simple token for now
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Registration failed", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Find user
                var user = _dataService.GetUserByEmail(request.Email);
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid email or password" });
                }

                // Verify password
                if (!VerifyPassword(request.Password, user.PasswordHash))
                {
                    return BadRequest(new { message = "Invalid email or password" });
                }

                // Create response
                var response = new LoginResponse
                {
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        NickName = user.NickName,
                        JoinedAt = user.JoinedAt
                    },
                    Token = GenerateToken(user.Id)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Login failed", error = ex.Message });
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "salt"));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var newHash = HashPassword(password);
            return newHash == hash;
        }

        private string GenerateToken(Guid userId)
        {
            // Simple token for development - in production use proper JWT
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userId}:{DateTime.UtcNow:yyyy-MM-dd}"));
        }
    }
} 