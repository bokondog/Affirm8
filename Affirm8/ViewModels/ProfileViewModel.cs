using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Affirm8.Models;
using Affirm8.Services;

namespace Affirm8.ViewModels
{
    /// <summary>
    /// ViewModel for Profile screen - shows user statistics and achievements
    /// </summary>
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly MessageService _messageService;
        private readonly AuthenticationService _authService;

        [ObservableProperty]
        private UserStatistics? userStatistics;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private bool hasData = false;

        [ObservableProperty]
        private string userName = "Kind User";

        [ObservableProperty]
        private string userEmail = "";

        public ProfileViewModel(MessageService messageService, AuthenticationService authService)
        {
            _messageService = messageService;
            _authService = authService;
            
            // Listen for authentication changes
            _authService.CurrentUserChanged += OnCurrentUserChanged;
            
            // Initialize with current user if available
            UpdateUserInfo();
        }

        public async Task InitializeAsync()
        {
            await LoadStatisticsAsync();
        }

        [RelayCommand]
        public async Task LoadStatisticsAsync()
        {
            if (!_authService.IsAuthenticated)
            {
                HasData = false;
                return;
            }

            IsLoading = true;
            try
            {
                var stats = await _messageService.GetUserStatisticsAsync();
                if (stats != null)
                {
                    UserStatistics = stats;
                    HasData = true;
                }
                else
                {
                    HasData = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading statistics: {ex.Message}");
                HasData = false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task RefreshAsync()
        {
            await LoadStatisticsAsync();
        }

        private void OnCurrentUserChanged(User? user)
        {
            UpdateUserInfo();
            
            if (user == null)
            {
                // User logged out - clear data
                UserStatistics = null;
                HasData = false;
            }
            else
            {
                // User logged in - load stats
                _ = Task.Run(async () => await LoadStatisticsAsync());
            }
        }

        private void UpdateUserInfo()
        {
            var currentUser = _authService.CurrentUser;
            if (currentUser != null)
            {
                UserName = currentUser.NickName ?? "Kind User";
                UserEmail = currentUser.Email ?? "";
            }
            else
            {
                UserName = "Kind User";
                UserEmail = "";
            }
        }
    }
}
