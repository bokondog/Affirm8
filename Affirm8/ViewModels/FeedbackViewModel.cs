using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Affirm8
{
    public partial class FeedbackViewModel : ObservableObject
    {
        public ObservableCollection<Review> Reviews { get; set; }

        [ObservableProperty]
        private string productImage = "Image1.png";

        [ObservableProperty]
        private string productName = "Sample Product";

        [ObservableProperty]
        private string productDescription = "Sample description";

        [ObservableProperty]
        private double overallRating = 4.5;

        [ObservableProperty]
        private double fiveStarPercent = 0.6;

        [ObservableProperty]
        private int fiveStarCount = 12;

        [ObservableProperty]
        private double fourStarPercent = 0.3;

        [ObservableProperty]
        private int fourStarCount = 6;

        [ObservableProperty]
        private double threeStarPercent = 0.1;

        [ObservableProperty]
        private int threeStarCount = 2;

        [ObservableProperty]
        private double twoStarPercent = 0.0;

        [ObservableProperty]
        private int twoStarCount = 0;

        [ObservableProperty]
        private double oneStarPercent = 0.0;

        [ObservableProperty]
        private int oneStarCount = 0;

        public FeedbackViewModel()
        {
            Reviews = new ObservableCollection<Review>();
            LoadReviews();
        }

        private void LoadReviews()
        {
            // Sample reviews data
            Reviews.Add(new Review
            {
                ReviewerName = "John Doe",
                Rating = 5,
                Title = "Great product!",
                Content = "I really love this product. It exceeded my expectations.",
                Date = DateTime.Now.AddDays(-7)
            });

            Reviews.Add(new Review
            {
                ReviewerName = "Jane Smith",
                Rating = 4,
                Title = "Good quality",
                Content = "Nice quality for the price. Would recommend.",
                Date = DateTime.Now.AddDays(-14)
            });
        }

        [RelayCommand]
        private async Task WriteReview()
        {
            // Navigate to write review page
            await Task.CompletedTask;
        }
    }

    public class Review
    {
        public string ReviewerName { get; set; } = "";
        public int Rating { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
