
using System.ComponentModel.DataAnnotations;
using Affirm8.Services;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Affirm8.ViewModels;

public partial class TabbedLoginViewModel : ObservableObject
{
    public TabbedLoginViewModel()
    {
        LoginInfo = new LoginModel();
        SignUpInfo = new SignUpModel();
    }

    [ObservableProperty]
    private LoginModel loginInfo;

    [ObservableProperty]
    private SignUpModel signUpInfo;

    [RelayCommand]
    private async Task Login()
    {
        try
        {
            // Implement login logic
            await Shell.Current.GoToAsync("//Home");
        }
        catch (Exception ex)
        {
            // Handle login error
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task SignUp()
    {
        try
        {
            // Implement signup logic
            await Shell.Current.GoToAsync("//Home");
        }
        catch (Exception ex)
        {
            // Handle signup error
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task ForgotPassword()
    {
        await Shell.Current.DisplayAlert("Forgot Password", "Password reset functionality coming soon!", "OK");
    }

    [RelayCommand]
    private void SwitchToLogin()
    {
        // Logic to switch to login tab
    }
}

public partial class LoginModel : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    private string email = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    private string password = string.Empty;

    [ObservableProperty]
    private bool rememberMe;
}

public partial class SignUpModel : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Name is required")]
    private string name = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    private string email = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    private string password = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Confirm Password is required")]
    private string confirmPassword = string.Empty;
}
