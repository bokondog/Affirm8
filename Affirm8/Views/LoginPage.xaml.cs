namespace Affirm8.Views.Forms
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void loginButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Login SUCCESS", "You have successfully logged in! Congratulations, user", "...ok");
        }
    }
}