using KindWords.Models;
using System.Collections.ObjectModel;

namespace KindWords.Services
{
    /// <summary>
    /// Service for managing messages and replies with proper business logic
    /// </summary>
    public class MessageService
    {
        private List<Message> _messages = new();
        private List<Reply> _replies = new();
        private int _nextMessageId = 1;
        private int _nextReplyId = 1;
        private readonly AuthenticationService _authService;

        public MessageService(AuthenticationService authService)
        {
            _authService = authService;
            // Add some sample data for demo
            SeedSampleData();
        }

        /// <summary>
        /// Get random messages for inbox that the current user hasn't replied to yet
        /// </summary>
        public async Task<List<Message>> GetInboxMessagesAsync(int count = 5)
        {
            await Task.Delay(100); // Simulate API delay
            
            var currentUserId = _authService.GetCurrentUserId();
            
            // Filter out messages the user has already replied to, and their own messages
            var availableMessages = _messages
                .Where(m => m.UserId != currentUserId && !m.HasUserReplied(currentUserId))
                .OrderBy(x => Guid.NewGuid()) // Random order
                .Take(count)
                .ToList();
            
            return availableMessages;
        }

        /// <summary>
        /// Get current user's messages with all their replies (only visible to message creator)
        /// </summary>
        public async Task<List<Message>> GetMyMessagesAsync()
        {
            await Task.Delay(100);
            
            var currentUserId = _authService.GetCurrentUserId();
            var userMessages = _messages
                .Where(m => m.UserId == currentUserId)
                .OrderByDescending(m => m.CreatedAt)
                .ToList();

            // For each message, load all replies (only creator can see all replies)
            foreach (var message in userMessages)
            {
                message.AllReplies = _replies
                    .Where(r => r.MessageId == message.Id)
                    .OrderBy(r => r.CreatedAt)
                    .ToList();
            }
            
            return userMessages;
        }

        public async Task<List<Reply>> GetRepliesForMessageAsync(int messageId)
        {
            await Task.Delay(100);
            return _replies.Where(r => r.MessageId == messageId).OrderBy(r => r.CreatedAt).ToList();
        }

        public async Task<Message> SendMessageAsync(string content, string category)
        {
            var currentUserId = _authService.GetCurrentUserId();
            
            var message = new Message
            {
                Id = _nextMessageId++,
                Content = content,
                Category = category,
                UserId = currentUserId,
                CreatedAt = DateTime.Now,
                RepliedByUserIds = new List<Guid>() // Initialize empty list
            };

            _messages.Add(message);
            return await Task.FromResult(message);
        }

        public async Task<Reply> SendReplyAsync(int messageId, string content)
        {
            var currentUserId = _authService.GetCurrentUserId();
            
            var reply = new Reply
            {
                Id = _nextReplyId++,
                MessageId = messageId,
                Content = content,
                UserId = currentUserId,
                CreatedAt = DateTime.Now
            };

            _replies.Add(reply);

            // Update message reply count and track that this user has replied
            var message = _messages.FirstOrDefault(m => m.Id == messageId);
            if (message != null)
            {
                message.ReplyCount++;
                message.HasBeenRepliedTo = true;
                
                // Track that this user has replied to this message
                if (!message.RepliedByUserIds.Contains(currentUserId))
                {
                    message.RepliedByUserIds.Add(currentUserId);
                }
            }

            return await Task.FromResult(reply);
        }

        /// <summary>
        /// Search messages in inbox (only messages user hasn't replied to)
        /// </summary>
        public async Task<List<Message>> SearchInboxMessagesAsync(string searchTerm, string? category = null)
        {
            await Task.Delay(100);
            
            var currentUserId = _authService.GetCurrentUserId();
            
            var filtered = _messages.Where(m => 
                m.UserId != currentUserId && // Not user's own messages
                !m.HasUserReplied(currentUserId) && // User hasn't replied yet
                m.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                filtered = filtered.Where(m => m.Category == category);
            }
            
            return filtered.OrderByDescending(m => m.CreatedAt).ToList();
        }

        private void SeedSampleData()
        {
            // Create some dummy users for demo
            var user1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var user2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var user3 = Guid.Parse("00000000-0000-0000-0000-000000000003");
            
            var sampleMessages = new[]
            {
                new Message { 
                    Id = _nextMessageId++, 
                    Content = "I'm feeling overwhelmed with school lately. Could use some encouragement.", 
                    Category = "Support", 
                    UserId = user2, // Different user so current user can see it
                    CreatedAt = DateTime.Now.AddHours(-2),
                    RepliedByUserIds = new List<Guid>()
                },
                new Message { 
                    Id = _nextMessageId++, 
                    Content = "Just got my first job! Can't believe it actually happened!", 
                    Category = "Celebration", 
                    UserId = user3,
                    CreatedAt = DateTime.Now.AddHours(-4),
                    RepliedByUserIds = new List<Guid>()
                },
                new Message { 
                    Id = _nextMessageId++, 
                    Content = "Grateful for my family who supports me through everything.", 
                    Category = "Gratitude", 
                    UserId = user2,
                    CreatedAt = DateTime.Now.AddHours(-6),
                    RepliedByUserIds = new List<Guid>()
                },
                new Message { 
                    Id = _nextMessageId++, 
                    Content = "Tomorrow is a new day and I believe things will get better.", 
                    Category = "Hope", 
                    UserId = user3,
                    CreatedAt = DateTime.Now.AddHours(-8),
                    RepliedByUserIds = new List<Guid>()
                },
                new Message { 
                    Id = _nextMessageId++, 
                    Content = "Struggling to see the light at the end of the tunnel right now.", 
                    Category = "Support", 
                    UserId = user2,
                    CreatedAt = DateTime.Now.AddHours(-10),
                    RepliedByUserIds = new List<Guid>()
                }
            };

            _messages.AddRange(sampleMessages);

            // Add some sample replies
            var sampleReplies = new[]
            {
                new Reply { 
                    Id = _nextReplyId++, 
                    MessageId = 1, 
                    Content = "You're stronger than you know! Take it one day at a time. 💪", 
                    UserId = user3,
                    CreatedAt = DateTime.Now.AddHours(-1) 
                },
                new Reply { 
                    Id = _nextReplyId++, 
                    MessageId = 2, 
                    Content = "Congratulations! Your hard work paid off! 🎉", 
                    UserId = user1,
                    CreatedAt = DateTime.Now.AddHours(-3) 
                },
                new Reply { 
                    Id = _nextReplyId++, 
                    MessageId = 5, 
                    Content = "The storm will pass. You're not alone in this journey. 🌈", 
                    UserId = user3,
                    CreatedAt = DateTime.Now.AddHours(-9) 
                }
            };

            _replies.AddRange(sampleReplies);

            // Update message reply counts and track replied users
            foreach (var reply in sampleReplies)
            {
                var message = _messages.FirstOrDefault(m => m.Id == reply.MessageId);
                if (message != null)
                {
                    message.ReplyCount++;
                    message.HasBeenRepliedTo = true;
                    
                    // Track who replied
                    if (!message.RepliedByUserIds.Contains(reply.UserId))
                    {
                        message.RepliedByUserIds.Add(reply.UserId);
                    }
                }
            }
        }
    }
} 