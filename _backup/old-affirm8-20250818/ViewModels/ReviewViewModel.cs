using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Affirm8
{
    public partial class ReviewViewModel : ObservableObject
    {
        [ObservableProperty]
        private string productImage = "Image1.png";

        [ObservableProperty]
        private string productName = "Sample Product";

        [ObservableProperty]
        private double rating = 0.0;

        [ObservableProperty]
        private string reviewTitle = "";

        [ObservableProperty]
        private string reviewContent = "";

        [ObservableProperty]
        private string reviewerName = "";

        [ObservableProperty]
        private string reviewerEmail = "";

        [ObservableProperty]
        private string productDescription = "Sample product description";

        [ObservableProperty]
        private string reviewDescription = "";

        public ReviewViewModel()
        {
            // Initialize with sample data
            ProductName = "Sample Product";
            ProductImage = "Image1.png";
        }

        [RelayCommand]
        private async Task SubmitReview()
        {
            // Submit review logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Submit()
        {
            // Submit logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Cancel()
        {
            // Cancel logic
            await Task.CompletedTask;
        }
    }
    public class ReviewInfo
    {
        public ReviewInfo()
        {
            this.Title = string.Empty;
            this.Review = string.Empty;
        }

        [Display(Name = "Title (Optional)")]
        public string Title { get; set; }

        [Display(Name = "Describe your review")]
        [Required(ErrorMessage = "Enter your review")]
        public string Review { get; set; }
    }
}
