using System.ComponentModel.DataAnnotations;

namespace KindWordsApi.Models
{
    /// <summary>
    /// Message model for Kind Words API
    /// </summary>
    public class Message
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        public string Category { get; set; } = "Support"; // Support, Hope, Celebration, Gratitude
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        public Guid UserId { get; set; }
        
        public bool IsAnonymous { get; set; } = true;
        
        // Navigation properties
        public User? User { get; set; }
        public List<Reply> Replies { get; set; } = new();
        
        // Computed properties
        public int ReplyCount => Replies.Count;
        public bool HasBeenRepliedTo => ReplyCount > 0;
    }
} 