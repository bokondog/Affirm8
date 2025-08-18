using Affirm8.ViewModels;

namespace Affirm8.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly AuthenticationViewModel _viewModel;

        public LoginPage(AuthenticationViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            // Subscribe to authentication state changes
            if (_viewModel != null)
            {
                // If user successfully logs in, navigate to main app
                // We'll handle this through events or direct navigation
                _viewModel.PropertyChanged += OnViewModelPropertyChanged;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }
        }

        private async void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AuthenticationViewModel.IsAuthenticated) && _viewModel.IsAuthenticated)
            {
                // User successfully logged in, navigate to main app
                await NavigateToMainApp();
            }
        }

        private async Task NavigateToMainApp()
        {
            // Navigate to the main shell
            Application.Current.MainPage = new AppShell();
        }
    }
}
