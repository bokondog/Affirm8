using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KindWordsApi.Data;
using KindWordsApi.Models;
using KindWordsApi.Services;
using System.Security.Claims;
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

        [HttpGet("statistics")]
        [Authorize]
        public async Task<ActionResult<UserStatisticsDto>> GetUserStatistics()
        {
            try
            {
                var userIdClaim = User.FindFirst("userId")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized();

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return NotFound("User not found");

                // Calculate statistics
                var messagesSent = await _context.Messages.CountAsync(m => m.UserId == userId);
                var repliesReceived = await _context.Replies.CountAsync(r => r.Message.UserId == userId);
                var repliesSent = await _context.Replies.CountAsync(r => r.UserId == userId);
                var likesReceived = await _context.ReplyLikes
                    .Include(rl => rl.Reply)
                    .ThenInclude(r => r.Message)
                    .CountAsync(rl => rl.Reply.UserId == userId);

                var impactRatio = repliesSent > 0 ? (double)likesReceived / repliesSent : 0.0;
                var daysActive = (DateTime.UtcNow - user.JoinedAt).Days;

                var statistics = new UserStatisticsDto
                {
                    MessagesSent = messagesSent,
                    RepliesReceived = repliesReceived,
                    RepliesSent = repliesSent,
                    LikesReceived = likesReceived,
                    ImpactRatio = Math.Round(impactRatio, 2),
                    JoinedAt = user.JoinedAt,
                    DaysActive = Math.Max(1, daysActive) // At least 1 day
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to get user statistics", error = ex.Message });
            }
        }

    }
} 