using Affirm8.Views.Social;
using Affirm8.Views.Chat;
using Affirm8.Views.Feedback;
using Affirm8.Views.Catalog;
using Affirm8.Views.Settings;
using Affirm8.Views.Forms;
using Affirm8.Views.Navigation;
using Affirm8.Pages;

namespace Affirm8
{
    public partial class App : Application
    {
		        public static string ImageServerPath { get; } = "https://picsum.photos/";

        public App()
        {
            InitializeComponent();
            
            MainPage = new AppShell();
        }
    }
}
