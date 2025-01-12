using Affirm8.Models;
using System.Collections.ObjectModel;

namespace Affirm8.Services
{
    public interface IHTTPService
    {
        Task<ObservableCollection<Post>> GetPosts();
        Task<bool> Login(string email, string password);
        Task<bool> Register(string email, string password, string nickname);
    }
}