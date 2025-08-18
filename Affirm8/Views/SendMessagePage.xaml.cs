using Affirm8.ViewModels;

namespace Affirm8.Views;

public partial class SendMessagePage : ContentPage
{
    public SendMessagePage(SendMessageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        
        // Ensure the default category is selected
        Loaded += (s, e) => {
            if (CategoryCollectionView.SelectedItem == null && viewModel.Categories.Contains(viewModel.SelectedCategory))
            {
                CategoryCollectionView.SelectedItem = viewModel.SelectedCategory;
            }
        };
    }
} 