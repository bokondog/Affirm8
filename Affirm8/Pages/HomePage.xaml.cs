namespace Affirm8.Pages;

public partial class HomePage : ContentPage
{
    public class Post {
        public int PostId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
    List<Post> posts = new List<Post>();

	public HomePage()
	{
        InitializeComponent();
        InitCollectionView();
	}

    private void InitCollectionView()
    {
        GenerateDummyPosts(15);
        CollViewPosts.ItemsSource = posts;
    }

    private void GenerateDummyPosts(int repetitions)
    {
        var dummyCount = 4;
        var postId = 0;
        for (int i = 0; i < repetitions; i++)
        {
            postId = i * dummyCount;
            posts.Add(new Post() { PostId = postId + 0, Content = "first!!1!", UserId = 0, Timestamp = DateTime.UtcNow });
            posts.Add(new Post() { PostId = postId + 1, Content = "any1 still watching in 2025?", UserId = 0, Timestamp = DateTime.UtcNow });
            posts.Add(new Post() { PostId = postId + 2, Content = "updog", UserId = 0, Timestamp = DateTime.UtcNow });
            posts.Add(new Post() { PostId = postId + 3, Content = "want to get rich? click here: (www.scam.com)", UserId = 0, Timestamp = DateTime.UtcNow });
        }
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

    private async void sfButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Home");
    }
}