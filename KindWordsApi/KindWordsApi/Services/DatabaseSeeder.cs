using KindWordsApi.Data;
using KindWordsApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace KindWordsApi.Services
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Apply any pending migrations (creates database if needed)
            await context.Database.MigrateAsync();

            // Check if data already exists
            if (await context.Users.AnyAsync())
            {
                return; // Database already seeded
            }

            // Create sample users with consistent GUIDs
            var user1 = new User
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Email = "alice@kindwords.com",
                NickName = "SunflowerDreamer",
                PasswordHash = HashPassword("password123"),
                JoinedAt = DateTime.UtcNow.AddDays(-30)
            };

            var user2 = new User
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Email = "bob@kindwords.com",
                NickName = "KindSoul88",
                PasswordHash = HashPassword("password123"),
                JoinedAt = DateTime.UtcNow.AddDays(-15)
            };

            var user3 = new User
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                Email = "charlie@kindwords.com",
                NickName = "WisdomSeeker",
                PasswordHash = HashPassword("password123"),
                JoinedAt = DateTime.UtcNow.AddDays(-7)
            };

            context.Users.AddRange(user1, user2, user3);
            await context.SaveChangesAsync();

            // Create sample messages
            var messages = new List<Message>
            {
                new Message
                {
                    Content = "I'm feeling overwhelmed with work lately. Could use some encouragement.",
                    Category = "Support",
                    UserId = user1.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    IsAnonymous = true
                },
                new Message
                {
                    Content = "Just got promoted at work! Feeling grateful for all the support.",
                    Category = "Celebration",
                    UserId = user2.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    IsAnonymous = false
                },
                new Message
                {
                    Content = "Looking for hope during tough times. Anyone else going through challenges?",
                    Category = "Hope",
                    UserId = user3.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    IsAnonymous = true
                },
                new Message
                {
                    Content = "Thank you to everyone who makes this world a little brighter each day.",
                    Category = "Gratitude",
                    UserId = user1.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    IsAnonymous = true
                },
                new Message
                {
                    Content = "Started a new hobby and it's bringing me so much joy!",
                    Category = "Celebration",
                    UserId = user2.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    IsAnonymous = false
                },
                new Message
                {
                    Content = "Struggling with anxiety lately. Any kind words would mean the world to me.",
                    Category = "Support",
                    UserId = user3.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(-12),
                    IsAnonymous = true
                },
                new Message
                {
                    Content = "Grateful for my family and friends who always believe in me.",
                    Category = "Gratitude",
                    UserId = user1.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(-6),
                    IsAnonymous = false
                },
                new Message
                {
                    Content = "Hoping for better days ahead. Sending love to anyone who needs it.",
                    Category = "Hope",
                    UserId = user2.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(-2),
                    IsAnonymous = true
                }
            };

            context.Messages.AddRange(messages);
            await context.SaveChangesAsync();

            // Create sample replies
            var replies = new List<Reply>
            {
                new Reply
                {
                    MessageId = messages[0].Id, // Support message
                    Content = "You've got this! Remember that challenges are temporary, but your strength is permanent. 💪",
                    UserId = user2.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-4).AddHours(2),
                    IsAnonymous = true
                },
                new Reply
                {
                    MessageId = messages[0].Id, // Support message
                    Content = "Take it one day at a time. You're doing better than you think! 🌟",
                    UserId = user3.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-4).AddHours(5),
                    IsAnonymous = true
                },
                new Reply
                {
                    MessageId = messages[1].Id, // Celebration message
                    Content = "Congratulations! Your hard work paid off! 🎉",
                    UserId = user1.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-3).AddHours(1),
                    IsAnonymous = false
                },
                new Reply
                {
                    MessageId = messages[2].Id, // Hope message
                    Content = "Yes, you're not alone. Hope is the light that guides us through darkness. ✨",
                    UserId = user1.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-2).AddHours(3),
                    IsAnonymous = true
                },
                new Reply
                {
                    MessageId = messages[5].Id, // Support message about anxiety
                    Content = "Anxiety is tough, but you are tougher. Take deep breaths and be kind to yourself. 🤗",
                    UserId = user1.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(-10),
                    IsAnonymous = true
                },
                new Reply
                {
                    MessageId = messages[5].Id, // Support message about anxiety
                    Content = "Sending you calm and peaceful thoughts. You're braver than you believe. 🌸",
                    UserId = user2.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(-8),
                    IsAnonymous = true
                }
            };

            context.Replies.AddRange(replies);

            // Update reply counts and replied status
            foreach (var message in messages)
            {
                var messageReplyList = replies.Where(r => r.MessageId == message.Id).ToList();
                message.ReplyCount = messageReplyList.Count;
                message.HasBeenRepliedTo = messageReplyList.Any();
            }

            // Create MessageReply tracking entries
            var messageReplies = new List<MessageReply>();
            foreach (var reply in replies)
            {
                var existingEntry = await context.MessageReplies
                    .FirstOrDefaultAsync(mr => mr.MessageId == reply.MessageId && mr.UserId == reply.UserId);
                
                if (existingEntry == null)
                {
                    messageReplies.Add(new MessageReply
                    {
                        MessageId = reply.MessageId,
                        UserId = reply.UserId,
                        RepliedAt = reply.CreatedAt
                    });
                }
            }

            context.MessageReplies.AddRange(messageReplies);
            await context.SaveChangesAsync();

            Console.WriteLine("Database seeded successfully!");
            Console.WriteLine($"Created {await context.Users.CountAsync()} users");
            Console.WriteLine($"Created {await context.Messages.CountAsync()} messages");
            Console.WriteLine($"Created {await context.Replies.CountAsync()} replies");
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "salt"));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
