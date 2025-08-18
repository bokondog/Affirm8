namespace Affirm8.Pages;

public partial class MatesPage : ContentPage
{
    public class Mate
    {
        public int UserId { get; set; }
    }
    List<Mate> mates = new List<Mate>();

    public MatesPage()
	{
		InitializeComponent();
        InitCollectionView();
	}

    private void InitCollectionView()
    {
        GenerateDummyMates(10);
        CollViewMates.ItemsSource = mates;
    }

    private void GenerateDummyMates(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            mates.Add(new Mate() { UserId = i });
        }
    }


}