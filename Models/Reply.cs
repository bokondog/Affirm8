using System.ComponentModel.DataAnnotations;

namespace KindWordsApi.Models
{
    /// <summary>
    /// Reply model for Kind Words API
    /// </summary>
    public class Reply
    {
        public int Id { get; set; }
        
        [Required]
        public int MessageId { get; set; }
        
        [Required]
        [StringLength(300, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        public Guid UserId { get; set; }
        
        public bool IsAnonymous { get; set; } = true;
        
        // Navigation properties
        public Message? Message { get; set; }
        public User? User { get; set; }
    }
} 