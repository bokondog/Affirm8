using System.ComponentModel.DataAnnotations;

namespace KindWordsApi.Models
{
    /// <summary>
    /// User model for Kind Words API
    /// </summary>
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string NickName { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        
        // Stats
        public int MessagesSent { get; set; } = 0;
        public int RepliesGiven { get; set; } = 0;
        public int RepliesReceived { get; set; } = 0;
        
        // Preferences
        public bool PreferAnonymous { get; set; } = true;
        public bool AllowNotifications { get; set; } = true;
    }
} 