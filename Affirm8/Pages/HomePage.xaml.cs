namespace Affirm8.Pages;

public partial class HomePage : ContentPage
{
    public class Post {
        public int PostId {get; set; }
        public string Content { get; set; }
    }
    List<Post> posts = new List<Post>();

	public HomePage()
	{
        InitializeComponent();
        InitCollectionView();
	}

    private void InitCollectionView()
    {
        GenerateDummyPosts();
        CollViewPosts.ItemsSource = posts;
    }

    private void GenerateDummyPosts()
    {
        posts.Add(new Post() { PostId = 0, Content = "first!!1!" });
        posts.Add(new Post() { PostId = 0, Content = "any1 still watching in 2025?" });
        posts.Add(new Post() { PostId = 0, Content = "updog" });
        posts.Add(new Post() { PostId = 1, Content = "want to get rich? click here: (www.scam.com)" });
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