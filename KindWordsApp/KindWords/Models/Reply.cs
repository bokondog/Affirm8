using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KindWords.Models
{
    /// <summary>
    /// Reply model for kind responses to messages
    /// </summary>
    public class Reply : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid UserId { get; set; } // Changed to Guid to match API
        public string UserName { get; set; } = "Anonymous";
        public bool IsAnonymous { get; set; } = true;
        private int _likeCount = 0;
        public int LikeCount 
        { 
            get => _likeCount; 
            set 
            { 
                _likeCount = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(LikeText));
            } 
        }
        
        private bool _isLikedByMessageOwner = false;
        public bool IsLikedByMessageOwner 
        { 
            get => _isLikedByMessageOwner; 
            set 
            { 
                _isLikedByMessageOwner = value; 
                OnPropertyChanged(); 
            } 
        }

        // Soft, warm colors for replies
        public string ReplyColor => "#F8F4E6"; // Warm cream
        public string BorderColor => "#E8D5C4"; // Soft brown
        
        // For UI display only
        public string TimeAgo => GetTimeAgo(CreatedAt);
        public string LikeText => LikeCount == 1 ? "1 like" : $"{LikeCount} likes";
        
        private string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;
            
            return timeSpan.TotalDays switch
            {
                >= 365 => $"{(int)(timeSpan.TotalDays / 365)} year{((int)(timeSpan.TotalDays / 365) == 1 ? "" : "s")} ago",
                >= 30 => $"{(int)(timeSpan.TotalDays / 30)} month{((int)(timeSpan.TotalDays / 30) == 1 ? "" : "s")} ago",
                >= 7 => $"{(int)(timeSpan.TotalDays / 7)} week{((int)(timeSpan.TotalDays / 7) == 1 ? "" : "s")} ago",
                >= 1 => $"{(int)timeSpan.TotalDays} day{((int)timeSpan.TotalDays == 1 ? "" : "s")} ago",
                >= 1.0/24 => $"{(int)timeSpan.TotalHours} hour{((int)timeSpan.TotalHours == 1 ? "" : "s")} ago",
                >= 1.0/1440 => $"{(int)timeSpan.TotalMinutes} minute{((int)timeSpan.TotalMinutes == 1 ? "" : "s")} ago",
                _ => "just now"
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 