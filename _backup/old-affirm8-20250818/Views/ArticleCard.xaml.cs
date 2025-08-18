

namespace Affirm8.Views.Catalog
{
    public partial class ArticleCard : ContentPage
    {
        public ArticleCard()
        {
            InitializeComponent();
        }

        private void BookmarkButton_Clicked(object sender, EventArgs e)
        {
            Button? button = sender as Button;
            var story = button?.BindingContext as Affirm8.Models.Story;
            if (story != null)
            {
                story.IsBookmarked = !story.IsBookmarked;
            }
        }

        private void FavouriteButton_Clicked_1(object sender, EventArgs e)
        {
            Button? button = sender as Button;
            var story = button?.BindingContext as Affirm8.Models.Story;
            if (story != null)
            {
                story.IsFavourite = !story.IsFavourite;
            }
        }
    }
}