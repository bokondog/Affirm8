using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KindWordsApi.Data;
using KindWordsApi.Models;
using System.Security.Claims;

namespace KindWordsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // All endpoints require authentication
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/messages/inbox?count=5
        [HttpGet("inbox")]
        public async Task<ActionResult<List<MessageDto>>> GetInboxMessages([FromQuery] int count = 5)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == null)
                    return Unauthorized();

                // Get messages that:
                // 1. Are not created by current user
                // 2. Current user has not replied to yet
                // 3. Ordered randomly (or by creation time for consistency)
                var repliedMessageIds = await _context.MessageReplies
                    .Where(mr => mr.UserId == currentUserId)
                    .Select(mr => mr.MessageId)
                    .ToListAsync();

                var messages = await _context.Messages
                    .Where(m => m.UserId != currentUserId && !repliedMessageIds.Contains(m.Id))
                    .OrderBy(m => m.CreatedAt) // For now, ordered by creation time
                    .Take(count)
                    .Include(m => m.MessageReplies)
                    .ToListAsync();

                var messageDtos = messages.Select(m => MapToMessageDto(m)).ToList();
                return Ok(messageDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to get inbox messages", error = ex.Message });
            }
        }

        // GET: api/messages/my-messages
        [HttpGet("my-messages")]
        public async Task<ActionResult<List<MessageDto>>> GetMyMessages()
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == null)
                    return Unauthorized();

                // Get all messages created by current user with their replies
                var messages = await _context.Messages
                    .Where(m => m.UserId == currentUserId)
                    .Include(m => m.Replies)
                    .Include(m => m.MessageReplies)
                    .OrderByDescending(m => m.CreatedAt)
                    .ToListAsync();

                var messageDtos = messages.Select(m => MapToMessageDto(m, includeAllReplies: true)).ToList();
                return Ok(messageDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to get your messages", error = ex.Message });
            }
        }

        // POST: api/messages
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage([FromBody] CreateMessageRequest request)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == null)
                    return Unauthorized();

                var message = new Message
                {
                    Content = request.Content,
                    Category = request.Category,
                    UserId = currentUserId.Value,
                    CreatedAt = DateTime.UtcNow,
                    IsAnonymous = true, // Always anonymous for now
                    ReplyCount = 0,
                    HasBeenRepliedTo = false
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                var messageDto = MapToMessageDto(message);
                return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, messageDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create message", error = ex.Message });
            }
        }

        // POST: api/messages/{messageId}/reply
        [HttpPost("{messageId}/reply")]
        public async Task<ActionResult<ReplyDto>> CreateReply(int messageId, [FromBody] CreateReplyRequest request)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == null)
                    return Unauthorized();

                // Validate that the message ID in the route matches the request
                if (messageId != request.MessageId)
                    return BadRequest("Message ID mismatch");

                // Check if message exists
                var message = await _context.Messages.FindAsync(messageId);
                if (message == null)
                    return NotFound("Message not found");

                // Check if user is trying to reply to their own message
                if (message.UserId == currentUserId)
                    return BadRequest("Cannot reply to your own message");

                // Check if user has already replied to this message
                var existingReply = await _context.MessageReplies
                    .FirstOrDefaultAsync(mr => mr.MessageId == messageId && mr.UserId == currentUserId);
                if (existingReply != null)
                    return BadRequest("You have already replied to this message");

                // Create the reply
                var reply = new Reply
                {
                    MessageId = messageId,
                    Content = request.Content,
                    UserId = currentUserId.Value,
                    CreatedAt = DateTime.UtcNow,
                    IsAnonymous = true
                };

                _context.Replies.Add(reply);

                // Create MessageReply tracking entry
                var messageReply = new MessageReply
                {
                    MessageId = messageId,
                    UserId = currentUserId.Value,
                    RepliedAt = DateTime.UtcNow
                };

                _context.MessageReplies.Add(messageReply);

                // Update message reply count and status
                message.ReplyCount++;
                message.HasBeenRepliedTo = true;

                await _context.SaveChangesAsync();

                var replyDto = MapToReplyDto(reply);
                return Ok(replyDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create reply", error = ex.Message });
            }
        }

        // GET: api/messages/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDto>> GetMessage(int id)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == null)
                    return Unauthorized();

                var message = await _context.Messages
                    .Include(m => m.Replies)
                    .Include(m => m.MessageReplies)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (message == null)
                    return NotFound();

                // Only allow access if user created the message or it's in their inbox
                bool canAccess = message.UserId == currentUserId || 
                                !await _context.MessageReplies.AnyAsync(mr => mr.MessageId == id && mr.UserId == currentUserId);

                if (!canAccess)
                    return Forbid();

                var messageDto = MapToMessageDto(message, includeAllReplies: message.UserId == currentUserId);
                return Ok(messageDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to get message", error = ex.Message });
            }
        }

        // GET: api/messages/search?term=anxiety&category=Support
        [HttpGet("search")]
        public async Task<ActionResult<List<MessageDto>>> SearchInboxMessages(
            [FromQuery] string term, 
            [FromQuery] string? category = null)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == null)
                    return Unauthorized();

                if (string.IsNullOrWhiteSpace(term))
                    return BadRequest("Search term is required");

                // Get messages that current user hasn't replied to
                var repliedMessageIds = await _context.MessageReplies
                    .Where(mr => mr.UserId == currentUserId)
                    .Select(mr => mr.MessageId)
                    .ToListAsync();

                var query = _context.Messages
                    .Where(m => m.UserId != currentUserId && 
                               !repliedMessageIds.Contains(m.Id) &&
                               m.Content.Contains(term));

                if (!string.IsNullOrWhiteSpace(category))
                {
                    query = query.Where(m => m.Category == category);
                }

                var messages = await query
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(20) // Limit search results
                    .Include(m => m.MessageReplies)
                    .ToListAsync();

                var messageDtos = messages.Select(m => MapToMessageDto(m)).ToList();
                return Ok(messageDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to search messages", error = ex.Message });
            }
        }

        [HttpPost("replies/{replyId}/like")]
        public async Task<ActionResult> LikeReply(int replyId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == null)
                    return Unauthorized();

                // Get the reply and its message
                var reply = await _context.Replies
                    .Include(r => r.Message)
                    .Include(r => r.Likes)
                    .FirstOrDefaultAsync(r => r.Id == replyId);

                if (reply == null)
                    return NotFound("Reply not found");

                // Check if current user is the message owner
                if (reply.Message.UserId != currentUserId.Value)
                    return Forbid("Only the message owner can like replies");

                // Check if already liked
                var existingLike = reply.Likes.FirstOrDefault(l => l.MessageOwnerId == currentUserId.Value);
                if (existingLike != null)
                    return BadRequest("Reply already liked");

                // Add the like
                var replyLike = new ReplyLike
                {
                    ReplyId = replyId,
                    MessageOwnerId = currentUserId.Value,
                    LikedAt = DateTime.UtcNow
                };

                _context.ReplyLikes.Add(replyLike);

                // Update reply like count and status
                reply.LikeCount++;
                reply.IsLikedByMessageOwner = true;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Reply liked successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to like reply", error = ex.Message });
            }
        }

        [HttpDelete("replies/{replyId}/like")]
        public async Task<ActionResult> UnlikeReply(int replyId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == null)
                    return Unauthorized();

                // Get the reply and its message
                var reply = await _context.Replies
                    .Include(r => r.Message)
                    .Include(r => r.Likes)
                    .FirstOrDefaultAsync(r => r.Id == replyId);

                if (reply == null)
                    return NotFound("Reply not found");

                // Check if current user is the message owner
                if (reply.Message.UserId != currentUserId.Value)
                    return Forbid("Only the message owner can unlike replies");

                // Find and remove the like
                var existingLike = reply.Likes.FirstOrDefault(l => l.MessageOwnerId == currentUserId.Value);
                if (existingLike == null)
                    return BadRequest("Reply not liked yet");

                _context.ReplyLikes.Remove(existingLike);

                // Update reply like count and status
                reply.LikeCount = Math.Max(0, reply.LikeCount - 1);
                reply.IsLikedByMessageOwner = false;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Reply unliked successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to unlike reply", error = ex.Message });
            }
        }

        #region Helper Methods

        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != null && Guid.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return null;
        }

        private MessageDto MapToMessageDto(Message message, bool includeAllReplies = false)
        {
            return new MessageDto
            {
                Id = message.Id,
                Content = message.Content,
                Category = message.Category,
                CreatedAt = message.CreatedAt,
                UserId = message.UserId,
                IsAnonymous = message.IsAnonymous,
                ReplyCount = message.ReplyCount,
                HasBeenRepliedTo = message.HasBeenRepliedTo,
                RepliedByUserIds = message.MessageReplies?.Select(mr => mr.UserId).ToList() ?? new List<Guid>(),
                Replies = includeAllReplies ? 
                    (message.Replies?.Select(MapToReplyDto).ToList() ?? new List<ReplyDto>()) : 
                    new List<ReplyDto>()
            };
        }

        private ReplyDto MapToReplyDto(Reply reply)
        {
            return new ReplyDto
            {
                Id = reply.Id,
                MessageId = reply.MessageId,
                Content = reply.Content,
                CreatedAt = reply.CreatedAt,
                UserId = reply.UserId,
                IsAnonymous = reply.IsAnonymous,
                LikeCount = reply.LikeCount,
                IsLikedByMessageOwner = reply.IsLikedByMessageOwner
            };
        }

        #endregion
    }
}
