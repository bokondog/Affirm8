using System.ComponentModel.DataAnnotations;

namespace KindWordsApi.Models
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string NickName { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public UserDto User { get; set; } = new();
        public string Token { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }

    // Message DTOs
    public class CreateMessageRequest
    {
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = "Support"; // Support, Hope, Celebration, Gratitude
    }

    public class CreateReplyRequest
    {
        [Required]
        public int MessageId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;
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
        
        // For MAUI app compatibility
        public List<Guid> RepliedByUserIds { get; set; } = new();
        public string CategoryColor => Category switch
        {
            "Support" => "#FF6B9D",
            "Hope" => "#4ECDC4",
            "Celebration" => "#FFE66D",
            "Gratitude" => "#95E1D3",
            _ => "#A8DADC"
        };
        public string CategoryEmoji => Category switch
        {
            "Support" => "🤗",
            "Hope" => "🌟",
            "Celebration" => "🎉",
            "Gratitude" => "🙏",
            _ => "💭"
        };
    }

    public class ReplyDto
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public bool IsAnonymous { get; set; }
        public int LikeCount { get; set; }
        public bool IsLikedByMessageOwner { get; set; }
    }

    public class UserStatisticsDto
    {
        public int MessagesSent { get; set; }
        public int RepliesReceived { get; set; }
        public int RepliesSent { get; set; }
        public int LikesReceived { get; set; }
        public double ImpactRatio { get; set; } // RepliesReceived / MessagesSent
        public DateTime JoinedAt { get; set; }
        public int DaysActive { get; set; }
    }

    public class LikeReplyRequest
    {
        public int ReplyId { get; set; }
    }
} 