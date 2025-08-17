using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KindWordsApi.Data;
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
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());
                if (existingUser != null)
                {
                    return BadRequest(new { message = "User with this email already exists" });
                }

                // Hash password
                var passwordHash = HashPassword(request.Password);

                // Create user
                var user = new User
                {
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    NickName = request.NickName,
                    JoinedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Generate JWT token
                var token = _jwtService.GenerateToken(user.Id, user.Email, user.NickName);

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
                    Token = token
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
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid email or password" });
                }

                // Verify password
                if (!VerifyPassword(request.Password, user.PasswordHash))
                {
                    return BadRequest(new { message = "Invalid email or password" });
                }

                // Generate JWT token
                var token = _jwtService.GenerateToken(user.Id, user.Email, user.NickName);

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
                    Token = token
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


    }
} 