using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Affirm8.Models;

namespace Affirm8
{
    public partial class ProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        private string profileImage = "ProfileImage1.png";

        [ObservableProperty]
        private string userName = "John Doe";

        [ObservableProperty]
        private string bio = "Welcome to my profile!";

        [ObservableProperty]
        private string userEmail = "user@example.com";

        [ObservableProperty]
        private int followersCount = 150;

        [ObservableProperty]
        private int followingCount = 89;

        [ObservableProperty]
        private int postsCount = 25;

        [ObservableProperty]
        private List<Post> userPosts = new();

        [ObservableProperty]
        private Post? selectedPost;

        public ObservableCollection<UserPost> Posts { get; set; }
        public ObservableCollection<UserConnection> Connections { get; set; }

        public ProfileViewModel()
        {
            Posts = new ObservableCollection<UserPost>();
            Connections = new ObservableCollection<UserConnection>();
            LoadData();
        }

        private void LoadData()
        {
            // Sample posts
            Posts.Add(new UserPost
            {
                Content = "Beautiful sunset today! 🌅",
                Timestamp = DateTime.Now.AddHours(-2),
                LikesCount = 25
            });

            Posts.Add(new UserPost
            {
                Content = "Had a great workout this morning! 💪",
                Timestamp = DateTime.Now.AddDays(-1),
                LikesCount = 18
            });

            // Sample connections
            Connections.Add(new UserConnection
            {
                Name = "Jane Smith",
                ProfileImage = "ProfileImage2.png",
                IsOnline = true
            });

            Connections.Add(new UserConnection
            {
                Name = "Mike Johnson",
                ProfileImage = "ProfileImage3.png",
                IsOnline = false
            });
        }

        [RelayCommand]
        private async Task EditProfile()
        {
            // Edit profile logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Settings()
        {
            // Settings logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task PostSelectionChanged(Post post)
        {
            SelectedPost = post;
            // Handle post selection
            await Task.CompletedTask;
        }
    }

    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public bool ShowOverlay { get; set; } = false;
        public int LikesCount { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public class UserPost
    {
        public string Content { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public int LikesCount { get; set; }
    }

    public class UserConnection
    {
        public string Name { get; set; } = "";
        public string ProfileImage { get; set; } = "";
        public bool IsOnline { get; set; }
    }
}
