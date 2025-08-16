using System.ComponentModel;

namespace KindWords.Models
{
    /// <summary>
    /// Core message model for affirmations and support requests
    /// </summary>
    public class Message : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = "Support"; // Support, Hope, Celebration, Gratitude
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid UserId { get; set; } // Changed to Guid to match API
        public bool IsAnonymous { get; set; } = true;
        public int ReplyCount { get; set; } = 0;
        public bool HasBeenRepliedTo { get; set; } = false;

        // Track which users have replied (for inbox filtering)
        public List<Guid> RepliedByUserIds { get; set; } = new();

        // Check if current user has replied to this message
        public bool HasUserReplied(Guid currentUserId) => RepliedByUserIds.Contains(currentUserId);

        // For "My Messages" - only creator can see all replies
        public List<Reply> AllReplies { get; set; } = new();

        // Color coding for categories
        public string CategoryColor => Category switch
        {
            "Support" => "#FF6B9D",    // Soft pink
            "Hope" => "#4ECDC4",       // Teal
            "Celebration" => "#FFE66D", // Yellow
            "Gratitude" => "#95E1D3",  // Mint green
            _ => "#A8DADC"             // Light blue default
        };

        public string CategoryEmoji => Category switch
        {
            "Support" => "🤗",
            "Hope" => "🌟",
            "Celebration" => "🎉",
            "Gratitude" => "🙏",
            _ => "💭"
        };

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 