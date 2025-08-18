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
        public string ImpactRatioText => $"{ImpactRatio:P0}";
        public string DaysActiveText => DaysActive == 1 ? "1 day" : $"{DaysActive} days";
        public string JoinedText => $"Joined {JoinedAt:MMMM yyyy}";
        
        public string ImpactDescription => ImpactRatio switch
        {
            >= 0.9 => "Incredible replies! 🌟",
            >= 0.75 => "Great replies! 🎉", 
            >= 0.5 => "Good replies! 👍",
            >= 0.25 => "Building reputation! 💪",
            >= 0.1 => "Getting started! 🌱",
            _ => "Send replies to earn likes! 💭"
        };
    }
}
