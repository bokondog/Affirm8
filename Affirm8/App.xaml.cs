using Affirm8.Services;
using Affirm8.Views;
using Affirm8.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Affirm8;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var window = new Window();
		
		// Now we can safely access the service provider
		var serviceProvider = this.Handler?.MauiContext?.Services ?? IPlatformApplication.Current?.Services;
		
		if (serviceProvider != null)
		{
			var authService = serviceProvider.GetRequiredService<AuthenticationService>();
			var authViewModel = serviceProvider.GetRequiredService<AuthenticationViewModel>();
			var loginPage = new LoginPage(authViewModel);
			
			window.Page = new NavigationPage(loginPage);
		}
		else
		{
			// Fallback to manual creation if DI not available
			var authService = new AuthenticationService();
			var authViewModel = new AuthenticationViewModel(authService);
			var loginPage = new LoginPage(authViewModel);
			
			window.Page = new NavigationPage(loginPage);
		}
		
		return window;
	}
}
