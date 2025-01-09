using Syncfusion.Maui.Toolkit.Buttons;

namespace Affirm8.Views.Catalog
{
    public partial class HomeFeed : ContentPage
    {
        public HomeFeed()
        {
            InitializeComponent();
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

    }
}