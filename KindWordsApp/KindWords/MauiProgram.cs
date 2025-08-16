using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using KindWords.Services;
using KindWords.ViewModels;
using KindWords.Views;

namespace KindWords;

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
		builder.Services.AddSingleton<AuthenticationService>();
		builder.Services.AddSingleton<MessageService>();

		// Register ViewModels
		builder.Services.AddTransient<SendMessageViewModel>();
		builder.Services.AddTransient<InboxViewModel>();
		builder.Services.AddTransient<MyMessagesViewModel>();
		builder.Services.AddTransient<AuthenticationViewModel>();

		// Register Views
		builder.Services.AddTransient<SendMessagePage>();
		builder.Services.AddTransient<InboxPage>();
		builder.Services.AddTransient<MyMessagesPage>();
		builder.Services.AddTransient<ProfilePage>();
		builder.Services.AddTransient<SettingsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
