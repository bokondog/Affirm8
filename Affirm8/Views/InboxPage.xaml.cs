using Affirm8.ViewModels;

namespace Affirm8.Views;

public partial class InboxPage : ContentPage
{
    private readonly InboxViewModel _viewModel;

    public InboxPage(InboxViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
} 