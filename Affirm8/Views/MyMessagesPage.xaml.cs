using Affirm8.ViewModels;

namespace Affirm8.Views;

public partial class MyMessagesPage : ContentPage
{
    public MyMessagesPage(MyMessagesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is MyMessagesViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
} 