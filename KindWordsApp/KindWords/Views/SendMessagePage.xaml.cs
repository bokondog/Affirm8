using KindWords.ViewModels;

namespace KindWords.Views;

public partial class SendMessagePage : ContentPage
{
    public SendMessagePage(SendMessageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 