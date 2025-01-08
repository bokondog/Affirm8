namespace Affirm8.Views.Forms
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            InitPopup();
        }

        private void InitPopup()
        {
            syncPopup.HeaderTitle = "Login success";
            syncPopup.Message = "You have successfully logged in! Congratulations, user";
        }

        private void loginButton_Clicked(object sender, EventArgs e)
        {
            // DisplayAlert("Login SUCCESS", "You have successfully logged in! Congratulations, user", "...ok");
            syncPopup.Show();
        }
    }
}