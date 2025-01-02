namespace Affirm8.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
        InitializeComponent();
	}

    private async void BtnNavProfile_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }

    private async void BtnNavMates_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MatesPage));
    }

    private async void BtnNavHome_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HomePage));
    }

    private async void BtnNavMail_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MailPage));
    }
}