using Microsoft.AspNetCore.Mvc;
using KindWordsApi.Models.DTOs;
using KindWordsApi.Services;
using System.Security.Cryptography;
using System.Text;

namespace KindWordsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly KindWordsDataService _dataService;
        private readonly JwtService _jwtService;

        public AuthController(KindWordsDataService dataService, JwtService jwtService)
        {
            _dataService = dataService;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if user already exists
            var existingUser = _dataService.GetUserByEmail(request.Email);
            if (existingUser != null)
                return BadRequest(new { message = "User with this email already exists" });

            // Hash password
            var passwordHash = HashPassword(request.Password);

            // Create user
            var user = _dataService.CreateUser(request.Email, request.NickName, passwordHash);

            return Ok(new { message = "User registered successfully" });
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find user
            var user = _dataService.GetUserByEmail(request.Email);
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password" });

            // Generate JWT token
            var token = _jwtService.GenerateToken(user.Id, user.Email, user.NickName);

            // Create response
            var response = new LoginResponse
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    NickName = user.NickName,
                    JoinedAt = user.JoinedAt,
                    MessagesSent = user.MessagesSent,
                    RepliesGiven = user.RepliesGiven,
                    RepliesReceived = user.RepliesReceived,
                    PreferAnonymous = user.PreferAnonymous,
                    AllowNotifications = user.AllowNotifications
                }
            };

            return Ok(response);
        }

        /// <summary>
        /// Get current user info (requires authentication)
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var user = _dataService.GetUserById(userId.Value);
            if (user == null)
                return NotFound();

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                NickName = user.NickName,
                JoinedAt = user.JoinedAt,
                MessagesSent = user.MessagesSent,
                RepliesGiven = user.RepliesGiven,
                RepliesReceived = user.RepliesReceived,
                PreferAnonymous = user.PreferAnonymous,
                AllowNotifications = user.AllowNotifications
            };

            return Ok(userDto);
        }

        // Helper methods
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "KindWordsSalt"));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var computedHash = HashPassword(password);
            return computedHash == hash;
        }

        private Guid? GetCurrentUserId()
        {
            var authHeader = Request.Headers.Authorization.FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
                return null;

            var token = authHeader.Substring("Bearer ".Length);
            return _jwtService.GetUserIdFromToken(token);
        }
    }
} 