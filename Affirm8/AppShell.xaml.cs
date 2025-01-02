using Affirm8.Pages;

namespace Affirm8
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        }

        private async void GoToHomePage()
        {
            await Shell.Current.GoToAsync(nameof(HomePage));
        }
    }
}
