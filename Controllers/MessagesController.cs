using Microsoft.AspNetCore.Mvc;
using KindWordsApi.Models.DTOs;
using KindWordsApi.Services;

namespace KindWordsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly KindWordsDataService _dataService;
        private readonly JwtService _jwtService;

        public MessagesController(KindWordsDataService dataService, JwtService jwtService)
        {
            _dataService = dataService;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Get inbox messages (random messages user hasn't replied to)
        /// </summary>
        [HttpGet("inbox")]
        public async Task<IActionResult> GetInboxMessages([FromQuery] int count = 5)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var messages = _dataService.GetInboxMessages(userId.Value, count);
            return Ok(messages);
        }

        /// <summary>
        /// Get current user's messages with all replies
        /// </summary>
        [HttpGet("my-messages")]
        public async Task<IActionResult> GetMyMessages()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var messages = _dataService.GetUserMessages(userId.Value);
            return Ok(messages);
        }

        /// <summary>
        /// Send a new message
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] CreateMessageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var message = _dataService.CreateMessage(userId.Value, request);
            return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
        }

        /// <summary>
        /// Reply to a message
        /// </summary>
        [HttpPost("reply")]
        public async Task<IActionResult> ReplyToMessage([FromBody] CreateReplyRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var reply = _dataService.CreateReply(userId.Value, request);
            if (reply == null)
                return NotFound(new { message = "Message not found" });

            return Ok(reply);
        }

        /// <summary>
        /// Get a specific message by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            // For now, just return that the endpoint exists
            // In a real implementation, you'd fetch the specific message
            return Ok(new { message = "Message endpoint - implementation pending" });
        }

        /// <summary>
        /// Search messages by content or category
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchMessages([FromQuery] string? query, [FromQuery] string? category)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            // Get inbox messages and filter
            var messages = _dataService.GetInboxMessages(userId.Value, 20);
            
            if (!string.IsNullOrEmpty(query))
            {
                messages = messages.Where(m => m.Content.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                messages = messages.Where(m => m.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(messages);
        }

        // Helper method
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