using System.ComponentModel;

namespace Affirm8.Models
{
    /// <summary>
    /// User model matching the API structure with authentication state
    /// </summary>
    public class User : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; } = DateTime.Now;
        
        // Authentication
        public string Token { get; set; } = string.Empty;
        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
        
        // Privacy settings
        public bool PreferAnonymous { get; set; } = true;
        public bool AllowNotifications { get; set; } = true;
        
        // Stats (computed from messages/replies)
        public int MessagesSent { get; set; } = 0;
        public int RepliesGiven { get; set; } = 0;
        public int RepliesReceived { get; set; } = 0;
        
        // Computed properties
        public int KindnessScore => (MessagesSent * 2) + (RepliesGiven * 3) + RepliesReceived;
        public string KindnessLevel => KindnessScore switch
        {
            < 10 => "New Friend 🌱",
            < 50 => "Kind Soul 🌸",
            < 100 => "Bright Light ✨",
            < 200 => "Guardian Angel 👼",
            _ => "Beacon of Hope 🌟"
        };

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Login model for API authentication
    /// </summary>
    public class LoginModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Register model for API user creation
    /// </summary>
    public class RegisterModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Access pass returned from API login
    /// </summary>
    public class AccessPassModel
    {
        public User User { get; set; } = new();
        public string Token { get; set; } = string.Empty;
    }
} 