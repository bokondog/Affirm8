using KindWordsApi.Models;
using System.Collections.Concurrent;

namespace KindWordsApi.Services
{
    public class DataService
    {
        private readonly ConcurrentDictionary<Guid, User> _users = new();

        public User? GetUserByEmail(string email)
        {
            return _users.Values.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public User? GetUserById(Guid id)
        {
            return _users.TryGetValue(id, out var user) ? user : null;
        }

        public User CreateUser(string email, string passwordHash, string nickName)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = passwordHash,
                NickName = nickName,
                JoinedAt = DateTime.UtcNow
            };

            _users[user.Id] = user;
            return user;
        }
    }
} 