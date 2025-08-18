using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Affirm8.Services;

namespace Affirm8.ViewModels
{
    /// <summary>
    /// ViewModel for authentication (login/register)
    /// </summary>
    public partial class AuthenticationViewModel : ObservableObject
    {
        private readonly AuthenticationService _authService;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string nickName = string.Empty;

        [ObservableProperty]
        private bool isLoginMode = true;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public bool IsAuthenticated => _authService.IsAuthenticated;

        public AuthenticationViewModel(AuthenticationService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        public async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both email and password";
                return;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var (success, errorMessage) = await _authService.LoginAsync(Email, Password);
                if (success)
                {
                    // Clear form
                    Email = string.Empty;
                    Password = string.Empty;
                    ErrorMessage = string.Empty;

                    await Application.Current.MainPage.DisplayAlert("Success", "Welcome back! 🌟", "OK");
                }
                else
                {
                    ErrorMessage = errorMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login failed: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(NickName))
            {
                ErrorMessage = "Please fill in all fields";
                return;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var (success, errorMessage) = await _authService.RegisterAsync(Email, Password, NickName);
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Account created! Please log in. ✨", "OK");
                    
                    // Switch to login mode
                    IsLoginMode = true;
                    Password = string.Empty; // Clear password but keep email for login
                    NickName = string.Empty;
                }
                else
                {
                    ErrorMessage = errorMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Registration failed: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public void SwitchMode()
        {
            IsLoginMode = !IsLoginMode;
            ErrorMessage = string.Empty;
            // Keep email but clear other fields when switching
            Password = string.Empty;
            if (IsLoginMode)
            {
                NickName = string.Empty;
            }
        }

        [RelayCommand]
        public void Logout()
        {
            _authService.Logout();
            Email = string.Empty;
            Password = string.Empty;
            NickName = string.Empty;
            ErrorMessage = string.Empty;
        }
    }
} 