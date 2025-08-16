using System.ComponentModel.DataAnnotations;

namespace KindWordsApi.Models.DTOs
{
    public class CreateMessageRequest
    {
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        public string Category { get; set; } = "Support";
        
        public bool IsAnonymous { get; set; } = true;
    }
    
    public class CreateReplyRequest
    {
        [Required]
        public int MessageId { get; set; }
        
        [Required]
        [StringLength(300, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;
        
        public bool IsAnonymous { get; set; } = true;
    }
    
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public bool IsAnonymous { get; set; }
        public int ReplyCount { get; set; }
        public bool HasBeenRepliedTo { get; set; }
        public List<ReplyDto> Replies { get; set; } = new();
    }
    
    public class ReplyDto
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public bool IsAnonymous { get; set; }
    }
} 