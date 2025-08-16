using System.ComponentModel;

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
        public bool IsAnonymous { get; set; } = true;

        // Soft, warm colors for replies
        public string ReplyColor => "#F8F4E6"; // Warm cream
        public string BorderColor => "#E8D5C4"; // Soft brown

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 