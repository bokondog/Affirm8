using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Affirm8
{
    public partial class ArticleCardViewModel : ObservableObject
    {
        [ObservableProperty]
        private Article article = new();

        public List<string> Comments { get; set; } = new() { "Great article!", "Very informative", "Thanks for sharing" };

        public ObservableCollection<Article> Articles { get; set; }

        public ArticleCardViewModel()
        {
            // Initialize with sample data
            Article = new Article
            {
                Title = "Sample Article",
                Summary = "This is a sample article summary",
                Author = "John Doe",
                PublishedDate = DateTime.Now,
                ReadingTime = "5 min read",
                Rating = 4.2
            };
        }

        [RelayCommand]
        private async Task Like()
        {
            // Like article logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Share()
        {
            // Share article logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Bookmark()
        {
            // Bookmark article logic
            await Task.CompletedTask;
        }
    }

    public class Article
    {
        public string Title { get; set; } = "";
        public string Summary { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public string Author { get; set; } = "";
        public DateTime PublishedDate { get; set; }
        public DateTime PublishDate { get; set; }
        public string ReadingTime { get; set; } = "";
        public bool IsBookmarked { get; set; }
        public bool IsFavourite { get; set; }
        public string ImageUrl { get; set; } = "";
        public double Rating { get; set; } = 4.5;
        public string Content { get; set; } = "Article content...";
        public List<string> Tags { get; set; } = new() { "Technology", "News", "Featured" };
    }

    public class Comment
    {
        public string Author { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public string Text { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
