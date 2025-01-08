namespace Affirm8.Pages;

public partial class ProfilePage : ContentPage
{
    public class User
    {
        public int UserId { get; set; }
    }
    User user = new User() { UserId = 0, };
    public ProfilePage()
	{
		InitializeComponent();
        InitForm();
	}

    private void InitForm()
    {
        LblUserId.Text = user.UserId.ToString();
    }
}