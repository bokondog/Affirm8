using Affirm8.ViewModels;

namespace Affirm8.Views;

public partial class SendMessagePage : ContentPage
{
    public SendMessagePage(SendMessageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 