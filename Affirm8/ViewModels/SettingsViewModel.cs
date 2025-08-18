using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Affirm8.Services;
using Microsoft.Maui.Controls;

namespace Affirm8.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ThemeService _themeService;
        private readonly AuthenticationService _authService;
        private readonly LocalizationService _localizationService;
        
        [ObservableProperty]
        private bool isDarkMode;
        
        [ObservableProperty]
        private string themeToggleText = "Enable Dark Mode";
        
        [ObservableProperty]
        private string themeIcon = "🌙";
        
        [ObservableProperty]
        private string languageToggleText = "Switch to Dutch";
        
        [ObservableProperty]
        private string languageIcon = "🇳🇱";
        
        public SettingsViewModel(ThemeService themeService, AuthenticationService authService, LocalizationService localizationService)
        {
            _themeService = themeService;
            _authService = authService;
            _localizationService = localizationService;
            
            // Subscribe to theme changes
            _themeService.ThemeChanged += OnThemeChanged;
            
            // Subscribe to authentication changes
            _authService.CurrentUserChanged += OnAuthenticationChanged;
            
            // Subscribe to language changes
            _localizationService.LanguageChanged += OnLanguageChanged;
            
            // Initialize with current theme, auth state, and language
            UpdateThemeDisplay();
            UpdateAuthenticationState();
            UpdateLanguageDisplay();
        }
        
        [RelayCommand]
        private void ToggleTheme()
        {
            _themeService.ToggleTheme();
        }
        
        [RelayCommand]
        private void ToggleLanguage()
        {
            _localizationService.ToggleLanguage();
        }
        
        private void OnThemeChanged(AppTheme theme)
        {
            UpdateThemeDisplay();
        }
        
        private void OnAuthenticationChanged(Models.User? user)
        {
            UpdateAuthenticationState();
        }
        
        private void OnLanguageChanged(System.Globalization.CultureInfo culture)
        {
            UpdateLanguageDisplay();
        }
        
        private void UpdateThemeDisplay()
        {
            IsDarkMode = _themeService.IsDarkMode;
            ThemeToggleText = IsDarkMode ? "Enable Light Mode" : "Enable Dark Mode";
            ThemeIcon = IsDarkMode ? "☀️" : "🌙";
        }
        
        [ObservableProperty]
        private bool isAuthenticated;
        
        [ObservableProperty]
        private string userDisplayName = "Guest";
        
        [ObservableProperty]
        private string userEmail = "Not logged in";
        
        public void UpdateAuthenticationState()
        {
            IsAuthenticated = _authService.IsAuthenticated;
            UserDisplayName = _authService.CurrentUser?.NickName ?? "Guest";
            UserEmail = _authService.CurrentUser?.Email ?? "Not logged in";
        }
        
        private void UpdateLanguageDisplay()
        {
            if (_localizationService.IsEnglish)
            {
                LanguageToggleText = "Switch to Dutch";
                LanguageIcon = "🇳🇱";
            }
            else
            {
                LanguageToggleText = "Switch to English";
                LanguageIcon = "🇺🇸";
            }
        }
    }
}
