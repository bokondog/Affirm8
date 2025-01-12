namespace Affirm8.Views.Settings
{
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            this.profileImage.Source = App.ImageServerPath + "ProfileImage1.png";
        }

        private async void BtnLogout_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Login");
        }
    }
}