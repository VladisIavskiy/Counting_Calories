namespace Counting_Calories;

public partial class FavoritesPopup : ContentView
{
    public event EventHandler<FoodItem> AddToMeal;
    public event EventHandler Closed;

    public FavoritesPopup()
    {
        InitializeComponent();
        Refresh();
    }

    public void Refresh(List<FoodItem>? items = null)
    {
        if (items == null)
            items = FavoritesManager.GetAll();

        FavoritesList.ItemsSource = null;
        FavoritesList.ItemsSource = items;
    }

    private void OnClearFavoritesClicked(object sender, EventArgs e)
    {
        FavoritesManager.ClearAll();
        Refresh();
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        this.IsVisible = false;
        Closed?.Invoke(this, EventArgs.Empty);
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is FoodItem item)
        {
            AddToMeal?.Invoke(this, item);
        }
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is FoodItem item)
        {
            FavoritesManager.Remove(item);
            Refresh();
        }
    }
}

