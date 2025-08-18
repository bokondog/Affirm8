using KindWordsApi.Models;
using KindWordsApi.Models.DTOs;
using System.Collections.Concurrent;

namespace KindWordsApi.Services
{
    /// <summary>
    /// In-memory data service for Kind Words API
    /// This can be replaced with Entity Framework later
    /// </summary>
    public class KindWordsDataService
    {
        private readonly ConcurrentDictionary<Guid, User> _users = new();
        private readonly ConcurrentDictionary<int, Message> _messages = new();
        private readonly ConcurrentDictionary<int, Reply> _replies = new();
        private int _nextMessageId = 1;
        private int _nextReplyId = 1;

        public KindWordsDataService()
        {
            SeedSampleData();
        }

        // User operations
        public User? GetUserByEmail(string email)
        {
            return _users.Values.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public User? GetUserById(Guid id)
        {
            return _users.TryGetValue(id, out var user) ? user : null;
        }

        public User CreateUser(string email, string nickname, string passwordHash)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                NickName = nickname,
                PasswordHash = passwordHash,
                JoinedAt = DateTime.UtcNow
            };

            _users[user.Id] = user;
            return user;
        }

        // Message operations
        public List<MessageDto> GetInboxMessages(Guid currentUserId, int count = 5)
        {
            // Get messages that:
            // 1. Are not from the current user
            // 2. The current user hasn't replied to yet
            var userReplies = _replies.Values.Where(r => r.UserId == currentUserId).Select(r => r.MessageId).ToHashSet();
            
            var availableMessages = _messages.Values
                .Where(m => m.UserId != currentUserId && !userReplies.Contains(m.Id))
                .OrderBy(_ => Guid.NewGuid()) // Random order
                .Take(count)
                .ToList();

            return availableMessages.Select(m => MapToMessageDto(m, false)).ToList();
        }

        public List<MessageDto> GetUserMessages(Guid userId)
        {
            var userMessages = _messages.Values
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.CreatedAt)
                .ToList();

            return userMessages.Select(m => MapToMessageDto(m, true)).ToList();
        }

        public MessageDto CreateMessage(Guid userId, CreateMessageRequest request)
        {
            var message = new Message
            {
                Id = _nextMessageId++,
                Content = request.Content,
                Category = request.Category,
                UserId = userId,
                IsAnonymous = request.IsAnonymous,
                CreatedAt = DateTime.UtcNow
            };

            _messages[message.Id] = message;

            // Update user stats
            if (_users.TryGetValue(userId, out var user))
            {
                user.MessagesSent++;
            }

            return MapToMessageDto(message, false);
        }

        public ReplyDto? CreateReply(Guid userId, CreateReplyRequest request)
        {
            if (!_messages.TryGetValue(request.MessageId, out var message))
                return null;

            var reply = new Reply
            {
                Id = _nextReplyId++,
                MessageId = request.MessageId,
                Content = request.Content,
                UserId = userId,
                IsAnonymous = request.IsAnonymous,
                CreatedAt = DateTime.UtcNow
            };

            _replies[reply.Id] = reply;

            // Update user stats
            if (_users.TryGetValue(userId, out var replyUser))
            {
                replyUser.RepliesGiven++;
            }

            if (_users.TryGetValue(message.UserId, out var messageUser))
            {
                messageUser.RepliesReceived++;
            }

            return MapToReplyDto(reply);
        }

        public List<ReplyDto> GetRepliesForMessage(int messageId)
        {
            var replies = _replies.Values
                .Where(r => r.MessageId == messageId)
                .OrderBy(r => r.CreatedAt)
                .ToList();

            return replies.Select(MapToReplyDto).ToList();
        }

        // Helper methods
        private MessageDto MapToMessageDto(Message message, bool includeReplies)
        {
            var dto = new MessageDto
            {
                Id = message.Id,
                Content = message.Content,
                Category = message.Category,
                CreatedAt = message.CreatedAt,
                UserId = message.UserId,
                IsAnonymous = message.IsAnonymous,
                ReplyCount = _replies.Values.Count(r => r.MessageId == message.Id),
                HasBeenRepliedTo = _replies.Values.Any(r => r.MessageId == message.Id)
            };

            if (includeReplies)
            {
                dto.Replies = GetRepliesForMessage(message.Id);
            }

            return dto;
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
                IsAnonymous = reply.IsAnonymous
            };
        }

        private void SeedSampleData()
        {
            // Create sample users
            var user1 = CreateUser("test1@example.com", "KindSoul1", "hashedpassword1");
            var user2 = CreateUser("test2@example.com", "BrightLight2", "hashedpassword2");
            var user3 = CreateUser("test3@example.com", "HopeGiver3", "hashedpassword3");

            // Create sample messages
            CreateMessage(user1.Id, new CreateMessageRequest
            {
                Content = "I'm feeling overwhelmed with school lately. Could use some encouragement.",
                Category = "Support"
            });

            CreateMessage(user2.Id, new CreateMessageRequest
            {
                Content = "Just got my first job! Can't believe it actually happened!",
                Category = "Celebration"
            });

            CreateMessage(user3.Id, new CreateMessageRequest
            {
                Content = "Grateful for my family who supports me through everything.",
                Category = "Gratitude"
            });

            CreateMessage(user1.Id, new CreateMessageRequest
            {
                Content = "Tomorrow is a new day and I believe things will get better.",
                Category = "Hope"
            });

            // Create sample replies
            CreateReply(user2.Id, new CreateReplyRequest
            {
                MessageId = 1,
                Content = "You're stronger than you know! Take it one day at a time. 💪"
            });

            CreateReply(user3.Id, new CreateReplyRequest
            {
                MessageId = 2,
                Content = "Congratulations! Your hard work paid off! 🎉"
            });
        }
    }
} 