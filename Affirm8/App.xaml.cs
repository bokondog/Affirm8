using Affirm8.Services;
using Affirm8.Views;
using Affirm8.ViewModels;

namespace Affirm8;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// Always start with login page
		SetMainPage();
	}

	private void SetMainPage()
	{
		// Create services manually for now (we could improve this with proper DI later)
		var authService = new AuthenticationService();
		var authViewModel = new AuthenticationViewModel(authService);
		var loginPage = new LoginPage(authViewModel);
		
		MainPage = new NavigationPage(loginPage);
	}
}
