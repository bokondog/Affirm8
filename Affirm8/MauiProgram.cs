using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Affirm8.Services;
using Affirm8.ViewModels;
using Affirm8.Views;
using System.Net.Http;

namespace Affirm8;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Register services
#if ANDROID
        // Configure HttpClient for Android to handle localhost HTTPS
        builder.Services.AddHttpClient("default", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();
            // For development only - bypass SSL certificate validation
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            return handler;
        });
#else
        builder.Services.AddHttpClient();
#endif
        builder.Services.AddSingleton<AuthenticationService>();
        builder.Services.AddSingleton<MessageService>();
        builder.Services.AddSingleton<ThemeService>();
        builder.Services.AddSingleton<LocalizationService>();

		// Register ViewModels
		builder.Services.AddTransient<SendMessageViewModel>();
		builder.Services.AddTransient<InboxViewModel>();
		builder.Services.AddTransient<MyMessagesViewModel>();
		builder.Services.AddTransient<ProfileViewModel>();
		builder.Services.AddTransient<AuthenticationViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();

		// Register Views
		builder.Services.AddTransient<SendMessagePage>();
		builder.Services.AddTransient<InboxPage>();
		builder.Services.AddTransient<MyMessagesPage>();
		builder.Services.AddTransient<ProfilePage>();
		builder.Services.AddTransient<SettingsPage>();
		// Note: LoginPage is created manually in App.xaml.cs and logout flow

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
