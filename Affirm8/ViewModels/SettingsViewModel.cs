using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Affirm8.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Affirm8.ViewModels
{
    /// <summary>
    /// ViewModel for the settings page with logout functionality
    /// </summary>
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly AuthenticationService _authService;

        [ObservableProperty]
        private string currentUserEmail = string.Empty;

        [ObservableProperty]
        private string currentUserNickname = string.Empty;

        public SettingsViewModel(AuthenticationService authService)
        {
            _authService = authService;
            
            // Subscribe to authentication changes
            _authService.CurrentUserChanged += OnCurrentUserChanged;
            
            // Initialize with current user if logged in
            UpdateUserInfo();
        }

        private void OnCurrentUserChanged(Models.User? user)
        {
            UpdateUserInfo();
        }

        private void UpdateUserInfo()
        {
            if (_authService.CurrentUser != null)
            {
                CurrentUserEmail = _authService.CurrentUser.Email;
                CurrentUserNickname = _authService.CurrentUser.NickName;
            }
            else
            {
                CurrentUserEmail = "Not logged in";
                CurrentUserNickname = "";
            }
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            try
            {
                // Show confirmation dialog
                bool confirmed = await Application.Current.MainPage.DisplayAlert(
                    "Logout", 
                    "Are you sure you want to logout?", 
                    "Yes", 
                    "Cancel");

                if (confirmed)
                {
                    // Clear authentication
                    _authService.Logout();

                    // Navigate back to login page by setting app's main page
                    var serviceProvider = Application.Current?.Handler?.MauiContext?.Services;
                    if (serviceProvider != null)
                    {
                        var authViewModel = serviceProvider.GetRequiredService<AuthenticationViewModel>();
                        var loginPage = new Views.LoginPage(authViewModel);
                        Application.Current.MainPage = new NavigationPage(loginPage);
                    }
                    else
                    {
                        // This should not happen with proper DI setup
                        throw new InvalidOperationException("Service provider not available. Check DI configuration in MauiProgram.cs");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error", 
                    "Failed to logout. Please try again.", 
                    "OK");
                System.Diagnostics.Debug.WriteLine($"Logout error: {ex.Message}");
            }
        }
    }
}