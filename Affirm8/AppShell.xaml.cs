namespace Affirm8;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		// Register additional routes
		Routing.RegisterRoute("LoginPage", typeof(Views.LoginPage));
	}
}
