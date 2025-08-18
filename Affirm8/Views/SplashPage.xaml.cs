using Microsoft.Maui.Controls;

namespace Affirm8.Views;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
        
        // Show splash for 2 seconds then navigate to main app
        _ = Task.Run(async () =>
        {
            await Task.Delay(2000);
            
            // Navigate to main shell on UI thread
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                Application.Current!.MainPage = new AppShell();
            });
        });
    }
}
