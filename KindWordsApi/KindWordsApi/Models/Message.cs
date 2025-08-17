using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KindWordsApi.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = "Support"; // Support, Hope, Celebration, Gratitude

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid UserId { get; set; }

        public bool IsAnonymous { get; set; } = true;

        public int ReplyCount { get; set; } = 0;

        public bool HasBeenRepliedTo { get; set; } = false;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();

        // Track which users have replied (stored as JSON or separate table)
        // For now, we'll use a separate junction table for better normalization
        public virtual ICollection<MessageReply> MessageReplies { get; set; } = new List<MessageReply>();
    }

    // Junction table to track which users have replied to which messages
    public class MessageReply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int MessageId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public DateTime RepliedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("MessageId")]
        public virtual Message Message { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
