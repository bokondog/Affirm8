using Syncfusion.Maui.Toolkit.Buttons;
using Affirm8.Data;
using Affirm8.Models;
using System.Collections.ObjectModel;

namespace Affirm8.Views.Catalog
{
    public partial class HomeFeed : ContentPage
    {
        PostDatabase database;
        public ObservableCollection<Post> Items { get; set; } = new();
        public HomeFeed(PostDatabase postDatabase)
        {
            InitializeComponent();
            database = postDatabase;
            BindingContext = this;
            database.GetItemsAsync();
        }

        private void FavouriteButton_Clicked(object sender, EventArgs e)
        {
            SfButton? button = sender as SfButton;
            var product = button?.BindingContext as Affirm8.Models.Product;
            if (product != null)
            {
                product.IsFavourite = !product.IsFavourite;
            }
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            var items = await database.GetItemsAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Items.Clear();
                foreach (var item in items)
                    Items.Add(item);

            });
        }
        async void OnItemAdded(object sender, EventArgs e)
        {

        }


    }
}