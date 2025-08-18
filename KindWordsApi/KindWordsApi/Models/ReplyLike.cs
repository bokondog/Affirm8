namespace KindWordsApi.Models
{
    /// <summary>
    /// Represents a like on a reply by a message owner
    /// </summary>
    public class ReplyLike
    {
        public int Id { get; set; }
        public int ReplyId { get; set; }
        public Guid MessageOwnerId { get; set; } // The owner of the original message
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Reply Reply { get; set; } = null!;
        public User MessageOwner { get; set; } = null!;
    }
}
