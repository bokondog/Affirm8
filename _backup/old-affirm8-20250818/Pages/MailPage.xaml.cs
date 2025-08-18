namespace Affirm8.Pages;

public partial class MailPage : ContentPage
{
    public class Post
    {
        public int PostId { get; set; }
    }
    List<Post> posts = new List<Post>();
    public MailPage()
	{
		InitializeComponent();
    
    InitCollectionView();
	}

    private void InitCollectionView()
    {
        GenerateDummyPosts(15);
        CollViewMail.ItemsSource = posts;
    }

    private void GenerateDummyPosts(int repetitions)
    {
        var dummyCount = 4;
        var postId = 0;
        for (int i = 0; i < repetitions; i++)
        {
            postId = i * dummyCount;
            posts.Add(new Post() { PostId = postId + 0 });
            posts.Add(new Post() { PostId = postId + 1 });
            posts.Add(new Post() { PostId = postId + 2 });
            posts.Add(new Post() { PostId = postId + 3 });
        }
    }
}