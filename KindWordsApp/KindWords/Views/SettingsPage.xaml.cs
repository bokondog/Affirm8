using KindWords.ViewModels;
using KindWords.Services;

namespace KindWords.Views;

public partial class SettingsPage : ContentPage
{
    private readonly AuthenticationViewModel _authViewModel;
    private readonly AuthenticationService _authService;
    private readonly SettingsViewModel _settingsViewModel;
    private bool _isLoginMode = true;

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
        
        await _authViewModel.LoginCommand.ExecuteAsync(null);
        
        if (!string.IsNullOrEmpty(_authViewModel.ErrorMessage))
        {
            ErrorLabel.Text = _authViewModel.ErrorMessage;
            ErrorLabel.IsVisible = true;
        }
        else
        {
            ErrorLabel.IsVisible = false;
            ClearForm();
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (!_isLoginMode)
        {
            _authViewModel.Email = EmailEntry.Text ?? "";
            _authViewModel.Password = PasswordEntry.Text ?? "";
            _authViewModel.NickName = NickNameEntry.Text ?? "";
            
            await _authViewModel.RegisterCommand.ExecuteAsync(null);
            
            if (!string.IsNullOrEmpty(_authViewModel.ErrorMessage))
            {
                ErrorLabel.Text = _authViewModel.ErrorMessage;
                ErrorLabel.IsVisible = true;
            }
            else
            {
                ErrorLabel.IsVisible = false;
                SwitchToLoginMode();
            }
        }
        else
        {
            SwitchToRegisterMode();
        }
    }

    private void OnSwitchModeClicked(object sender, EventArgs e)
    {
        if (_isLoginMode)
        {
            SwitchToRegisterMode();
        }
        else
        {
            SwitchToLoginMode();
        }
    }

    private void SwitchToLoginMode()
    {
        _isLoginMode = true;
        NickNameEntry.IsVisible = false;
        RegisterButton.Text = "Register";
        SwitchModeButton.Text = "Need an account? Register here";
        ErrorLabel.IsVisible = false;
        PasswordEntry.Text = "";
        NickNameEntry.Text = "";
    }

    private void SwitchToRegisterMode()
    {
        _isLoginMode = false;
        NickNameEntry.IsVisible = true;
        RegisterButton.Text = "Create Account";
        SwitchModeButton.Text = "Already have an account? Login here";
        ErrorLabel.IsVisible = false;
        PasswordEntry.Text = "";
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        _authViewModel.LogoutCommand.Execute(null);
        ClearForm();
        SwitchToLoginMode();
    }

    private void ClearForm()
    {
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