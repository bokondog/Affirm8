namespace Affirm8.Views.Forms
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void loginButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Login Success", "You have successfully logged in! Congratulations, user", "OK");
        }
    }
}