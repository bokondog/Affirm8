namespace KindWords.Models
{
    /// <summary>
    /// User statistics model for profile dashboard
    /// </summary>
    public class UserStatistics
    {
        public int MessagesSent { get; set; }
        public int RepliesReceived { get; set; }
        public int RepliesSent { get; set; }
        public int LikesReceived { get; set; }
        public double ImpactRatio { get; set; } // LikesReceived / RepliesSent
        public DateTime JoinedAt { get; set; }
        public int DaysActive { get; set; }

        // UI formatting properties
        public string ImpactRatioText => $"{ImpactRatio:F1}x";
        public string DaysActiveText => DaysActive == 1 ? "1 day" : $"{DaysActive} days";
        public string JoinedText => $"Joined {JoinedAt:MMMM yyyy}";
        
        public string ImpactDescription => ImpactRatio switch
        {
            >= 3.0 => "Incredible impact! 🌟",
            >= 2.0 => "Great impact! 🎉",
            >= 1.5 => "Good impact! 👍",
            >= 1.0 => "Making a difference! 💪",
            >= 0.5 => "Getting started! 🌱",
            _ => "Share your story! 💭"
        };
    }
}
