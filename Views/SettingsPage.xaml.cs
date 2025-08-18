using KindWords.ViewModels;
using KindWords.Services;

namespace KindWords.Views;

public partial class SettingsPage : ContentPage
{
    private readonly AuthenticationViewModel _authViewModel;
    private readonly AuthenticationService _authService;
    private readonly SettingsViewModel _settingsViewModel;
    private bool _isRegistering = false;

    public SettingsPage(AuthenticationViewModel authViewModel, AuthenticationService authService, SettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        _authViewModel = authViewModel;
        _authService = authService;
        _settingsViewModel = settingsViewModel;
        
        // Set the BindingContext to the SettingsViewModel for the appearance section
        BindingContext = _settingsViewModel;
        
        // Subscribe to authentication state changes
        _authService.CurrentUserChanged += OnAuthenticationStateChanged;
        
        // Update UI based on current state
        UpdateUIForAuthenticationState();
    }

    private void OnAuthenticationStateChanged(Models.User? user)
    {
        UpdateUIForAuthenticationState();
    }

    private void UpdateUIForAuthenticationState()
    {
        var isAuthenticated = _settingsViewModel.IsAuthenticated;
        
        AuthForm.IsVisible = !isAuthenticated;
        LoggedInSection.IsVisible = isAuthenticated;
        
        if (isAuthenticated)
        {
            WelcomeLabel.Text = $"Welcome back, {_settingsViewModel.UserDisplayName}! 🌟";
        }
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        _authViewModel.Email = EmailEntry.Text ?? "";
        _authViewModel.Password = PasswordEntry.Text ?? "";

        try
        {
            await _authViewModel.LoginCommand.ExecuteAsync(null);
            if (!string.IsNullOrEmpty(_authViewModel.ErrorMessage))
            {
                ErrorLabel.Text = _authViewModel.ErrorMessage;
                ErrorLabel.IsVisible = true;
            }
            else
            {
                ErrorLabel.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = $"Login failed: {ex.Message}";
            ErrorLabel.IsVisible = true;
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        _authViewModel.Email = EmailEntry.Text ?? "";
        _authViewModel.Password = PasswordEntry.Text ?? "";
        _authViewModel.NickName = NickNameEntry.Text ?? "";

        try
        {
            await _authViewModel.RegisterCommand.ExecuteAsync(null);
            if (!string.IsNullOrEmpty(_authViewModel.ErrorMessage))
            {
                ErrorLabel.Text = _authViewModel.ErrorMessage;
                ErrorLabel.IsVisible = true;
            }
            else
            {
                ErrorLabel.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = $"Registration failed: {ex.Message}";
            ErrorLabel.IsVisible = true;
        }
    }

    private void OnSwitchModeClicked(object sender, EventArgs e)
    {
        _isRegistering = !_isRegistering;
        
        if (_isRegistering)
        {
            NickNameEntry.IsVisible = true;
            RegisterButton.IsVisible = true;
            SwitchModeButton.Text = "Already have an account? Login";
        }
        else
        {
            NickNameEntry.IsVisible = false;
            RegisterButton.IsVisible = false;
            SwitchModeButton.Text = "Need an account? Register";
        }
        
        // Clear any errors
        ErrorLabel.IsVisible = false;
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await _authService.Logout();
        
        // Clear form
        EmailEntry.Text = "";
        PasswordEntry.Text = "";
        NickNameEntry.Text = "";
        ErrorLabel.IsVisible = false;
    }
    
    private void OnThemeToggleClicked(object sender, EventArgs e)
    {
        _settingsViewModel.ToggleThemeCommand.Execute(null);
    }
    
    private void OnLanguageToggleClicked(object sender, EventArgs e)
    {
        _settingsViewModel.ToggleLanguageCommand.Execute(null);
    }
}
