using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Affirm8.Models;

namespace Affirm8.Services
{
    public static class UserService
    {
        static List<User> Users { get; }
        static int nextId = 3;
        static UserService()
        {
            Users = new List<User>
        {
            new User { Id = 1, Username = "Ducked_Up", Password = "1234" },
            new User { Id = 2, Username = "BloobyScoo", Password = "5678" }
        };
        }

        public static List<User> GetAll() => Users;

        public static User? Get(int id) => Users.FirstOrDefault(p => p.Id == id);

        public static void Add(User user)
        {
            user.Id = nextId++;
            Users.Add(user);
        }

        public static void Delete(int id)
        {
            var user = Get(id);
            if (user is null)
                return;

            Users.Remove(user);
        }

        public static void Update(User user)
        {
            var index = Users.FindIndex(p => p.Id == user.Id);
            if (index == -1)
                return;

            Users[index] = user;
        }
    }
}
