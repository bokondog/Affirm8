using System.Collections.ObjectModel;
using System.Text.Json;
using Affirm8.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Affirm8
{
    public partial class CartPageViewModel : ObservableObject
    {
        public List<Post> Items { get; set; }

        public List<Post> Products => Items;

        [ObservableProperty]
        private bool isRefreshing = false;

        [ObservableProperty]
        private Post? selectedProduct;

        public CartPageViewModel()
        {
            Items = new List<Post>();
            LoadFromJson();
        }

        [RelayCommand]
        private async Task Search(string query)
        {
            // Search logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Filter()
        {
            // Filter logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Like(Post post)
        {
            // Like logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Comment(Post post)
        {
            // Comment logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Share(Post post)
        {
            // Share logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task AddToCart(Post post)
        {
            // Add to cart logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Settings()
        {
            // Settings logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Refresh()
        {
            IsRefreshing = true;
            // Refresh logic
            await Task.Delay(1000);
            IsRefreshing = false;
        }

        [RelayCommand]
        private async Task ProductSelectionChanged(Post product)
        {
            SelectedProduct = product;
            // Handle product selection
            await Task.CompletedTask;
        }

        // Method to load data from JSON
        public void LoadFromJson()
        {
            string jsonData = @"
{
    ""items"": [
  {
    ""id"": 1,
    ""previewimage"": ""Image1.png"",
    ""name"": ""Full-Length Skirt"",
    ""summary"": ""This plaid, cotton skirt will keep you warm in the air-conditioned office or outside on cooler days."",
    ""description"": ""This plaid, cotton skirt will keep you warm in the air-conditioned office or outside on cooler days."",
    ""actualprice"": 220,
    ""discountpercent"": 15,
    ""overallrating"": 5,
    ""previewimages"": [ ""Image1.png"", ""Image1.png"", ""Image1.png"", ""Image1.png"", ""Image1.png"" ],
    ""reviews"": [
      {
        ""profileimage"": ""ProfileImage10.png"",
        ""customername"": ""Serina Williams"",
        ""comment"": ""Greatest purchase I have ever made in my life."",
        ""rating"": 5,
        ""images"": [ ""Image1.png"", ""Image1.png"", ""Image1.png"" ],
        ""revieweddate"": ""2019-12-29""
      },
      {
        ""profileimage"": ""ProfileImage11.png"",
        ""customername"": ""Alise Valasquez"",
        ""comment"": ""Absolutely love them! Can't stop wearing!"",
        ""rating"": 5,
        ""images"": [ ""Image1.png"", ""Image1.png"", ""Image1.png"" ],
        ""revieweddate"": ""2019-12-29""
      }
    ],
    ""quantities"": [ ""1"", ""2"", ""3"" ],
    ""totalquantity"": 0,
    ""sizevariants"": [ ""XS"", ""S"", ""M"", ""L"", ""XL"" ],
    ""timestamp"": ""2025-01-09T08:45:46Z"",
    ""tag"": ""be gentle""
  },
  {
    ""id"": 2,
    ""previewimage"": ""Image2.png"",
    ""name"": ""Peasant Blouse"",
    ""summary"": ""Look your best this fall in this V-neck, pleated peasant blouse with full sleeves. Comes in white, chocolate, forest green, and more."",
    ""description"": ""Look your best this fall in this V-neck, pleated peasant blouse with full sleeves. Comes in white, chocolate, forest green, and more."",
    ""actualprice"": 285,
    ""discountpercent"": 30,
    ""overallrating"": 4,
    ""previewimages"": [ ""Image2.png"", ""Image2.png"", ""Image2.png"", ""Image2.png"", ""Image2.png"" ],
    ""reviews"": [],
    ""quantities"": [ ""1"", ""2"", ""3"" ],
    ""totalquantity"": 0,
    ""sizevariants"": [ ""XS"", ""S"", ""M"", ""L"", ""XL"" ],
    ""timestamp"": ""2025-01-09T08:45:46Z"",
    ""tag"": ""be gentle""
  },
  {
    ""id"": 3,
    ""previewimage"": ""Image3.png"",
    ""name"": ""High-Waisted Skirt"",
    ""summary"": ""Available in khaki, taupe, and dove gray, this high-waisted, cotton skirt hits just below the knees."",
    ""description"": ""Available in khaki, taupe, and dove gray, this high-waisted, cotton skirt hits just below the knees."",
    ""actualprice"": 220,
    ""discountpercent"": 15,
    ""overallrating"": 5,
    ""previewimages"": [ ""Image3.png"", ""Image3.png"", ""Image3.png"", ""Image3.png"", ""Image3.png"" ],
    ""reviews"": [],
    ""quantities"": [ ""1"", ""2"", ""3"" ],
    ""totalquantity"": 0,
    ""sizevariants"": [ ""XS"", ""S"", ""M"", ""L"", ""XL"" ],
    ""timestamp"": ""2025-01-09T08:45:46Z"",
    ""tag"": ""be gentle""
  },
  {
    ""id"": 4,
    ""previewimage"": ""Image4.png"",
    ""name"": ""Ducked_Up"",
    ""summary"": ""This cute miniature pack has adjustable straps, allowing it to be carried on your back, or as a purse. The genuine leather comes in cream, black, mahogany, and wine."",
    ""description"": ""Got a new job. What if I suck?"",
    ""actualprice"": 285,
    ""discountpercent"": 30,
    ""overallrating"": 4,
    ""previewimages"": [ ""Image4.png"", ""Image4.png"", ""Image4.png"", ""Image4.png"", ""Image4.png"" ],
    ""reviews"": [],
    ""quantities"": [ ""1"", ""2"", ""3"" ],
    ""totalquantity"": 0,
    ""sizevariants"": [ ""XS"", ""S"", ""M"", ""L"", ""XL"" ],
    ""timestamp"": ""2025-01-09T08:45:46Z"",
    ""tag"": ""be gentle""
  },
  {
    ""id"": 5,
    ""previewimage"": ""Image5.png"",
    ""name"": ""Sweetheart Dress"",
    ""summary"": ""Be bold in red with this fashionable, yet comfortable dress. Three-quarter-length sleeves are perfect for the in-between weather of autumn."",
    ""description"": ""Be bold in red with this fashionable, yet comfortable dress. Three-quarter-length sleeves are perfect for the in-between weather of autumn."",
    ""actualprice"": 220,
    ""discountpercent"": 15,
    ""overallrating"": 5,
    ""previewimages"": [ ""Image5.png"", ""Image5.png"", ""Image5.png"", ""Image5.png"", ""Image5.png"" ],
    ""quantities"": [ ""1"", ""2"", ""3"" ],
    ""totalquantity"": 0,
    ""sizevariants"": [ ""XS"", ""S"", ""M"", ""L"", ""XL"" ],
    ""timestamp"": ""2025-01-09T08:45:46Z"",
    ""tag"": ""be gentle""
  }
]
}";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<Dictionary<string, ObservableCollection<Affirm8.Models.Product>>>(jsonData, options);
            if (data == null)
            {
                return;
            }

            var items = data["items"];

        }
    }
}
