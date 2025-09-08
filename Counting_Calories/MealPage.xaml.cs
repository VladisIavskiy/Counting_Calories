using System.Text;
using Microsoft.Maui.Controls;

namespace Counting_Calories;

public partial class MealPage : ContentPage
{
    private string mealType; // "�������", "����", "����"
    private List<FoodItem> foodItems = new();
    private Counting counting = new();


    public MealPage(string mealType)
    {
        InitializeComponent();
        this.mealType = mealType;
        Title = mealType; // �������� ��������

        LoadData();

        favoritesPopup.AddToMeal += FavoritesPopup_AddToMeal;
        Refresh();
    }

    private void FavoritesPopup_AddToMeal(object sender, FoodItem item)
    {
        // ��������� ��������� ������� �� ���������� � ������� ���������
        _ = AddFromFavoritesAsync(item);
    }

    // ��������� ������ ����������
    public void Refresh()
    {
        favoritesPopup.Refresh(FavoritesManager.GetAll());
    }

    private void OnCloseFavoritesClicked(object sender, EventArgs e)
    {
        favoritesPopup.IsVisible = false;

        // �������� ������ ��������� ���������, ���� �����
        selectedItemsListView.ItemsSource = null;
        selectedItemsListView.ItemsSource = counting.Items;
    }


    private void OnAddFromFavorites(object sender, EventArgs e)
    {
        // ������ ���������� �� ���������� (��������, ������� ����������� ����)
        favoritesPopup.IsVisible = true;
    }



    private async System.Threading.Tasks.Task AddFromFavoritesAsync(FoodItem item)
    {
        string[] grams = { "100", "200", "300", "400", "500" };
        string result = await DisplayActionSheet("�������� ����������", "������", null, grams);

        if (result != null && result != "������" && int.TryParse(result, out int g))
        {
            var adjusted = new FoodItem
            {
                Name = item.Name,
                Calories = item.Calories * g / 100.0,
                Protein = item.Protein * g / 100.0,
                Fats = item.Fats * g / 100.0,
                Carbs = item.Carbs * g / 100.0
            };

            counting.AddItem(adjusted);
            UpdateTotals();
            await DisplayAlert("���������", $"{item.Name} ({g} �)", "��");
        }
    }

    private void AddToFavorites(FoodItem item)
    {
        if (!FavoritesManager.IsFavorite(item))
        {
            FavoritesManager.Add(item);
            DisplayAlert("���������", $"{item.Name} �������� � ���������", "��");
            Refresh();
        }
        else
        {
            DisplayAlert("��� � ���������", $"{item.Name} ��� ���� � ���������", "��");
        }
    }



    private void OnFavoritesClicked(object sender, EventArgs e)
    {
        favoritesPopup.IsVisible = !favoritesPopup.IsVisible;
        favoritesPopup.Refresh(GetCurrentFavorites());
    }

    private void OnAddToFavoritesClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is FoodItem food)
        {
            AddToFavorites(food);
        }
    }

    private void UpdateTotals()
    {
        caloriesLabel.Text = $"����: {counting.TotalCalories:F1}";
        proteinLabel.Text = $"�����: {counting.TotalProtein:F1} �";
        fatsLabel.Text = $"����: {counting.TotalFats:F1} �";
        carbsLabel.Text = $"��������: {counting.TotalCarbs:F1} �";
    }

    private async void OnShowSelectedClicked(object sender, EventArgs e)
    {
        if (!counting.Items.Any())
        {
            await DisplayAlert("������ ����", "�� ��� �� �������� �� ������ ��������.", "��");
            return;
        }

        selectedItemsSection.IsVisible = !selectedItemsSection.IsVisible;

        selectedItemsListView.ItemsSource = null;
        selectedItemsListView.ItemsSource = counting.Items;
    }

    private async void OnDeleteItemClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is FoodItem item)
        {
            bool confirm = await DisplayAlert("��������", $"������� {item.Name} �� ���������?", "��", "���");
            if (confirm)
            {
                counting.Items.Remove(item);
                UpdateTotals();

                selectedItemsListView.ItemsSource = null;
                selectedItemsListView.ItemsSource = counting.Items;
            }
        }
    }

    private async void OnResetClicked(object sender, EventArgs e)
    {
        counting.Items.Clear();

        selectedItemsListView.ItemsSource = null;
        selectedItemsListView.ItemsSource = counting.Items;
        selectedItemsSection.IsVisible = false;
        UpdateTotals();

        await DisplayAlert("��������", "��������� ������ �������.", "��");
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (counting.TotalCalories == 0)
        {
            await DisplayAlert("��� ������", "�������� ���� �� ���� �������", "��");
            return;
        }

        string result = $"����: {DateTime.Now:G}\n" +
                        $"�������: {counting.TotalCalories:F1} ����\n" +
                        $"�����: {counting.TotalProtein:F1} �\n" +
                        $"����: {counting.TotalFats:F1} �\n" +
                        $"��������: {counting.TotalCarbs:F1} �\n" +
                        "-----------------------------\n";

        string fileName = Path.Combine(FileSystem.AppDataDirectory, $"{mealType.ToLower()}_log.txt");

        File.AppendAllText(fileName, result);

        await DisplayAlert("���������", "��������� ������� � ����.", "��");
    }

    private void LoadData()
    {
        string basePath = Path.Combine(AppContext.BaseDirectory, "Resources", "Data");

        string foodPath = Path.Combine(basePath, "FOOD_DES_500_ru.txt");
        string nutPath = Path.Combine(basePath, "NUT_DATA_500.txt");

        foodItems = DataLoader.LoadFoodItems(foodPath, nutPath);
        foodListView.ItemsSource = foodItems;
    }

    public List<FoodItem> GetCurrentFavorites()
    {
        return FavoritesManager.GetAll();
    }


    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            foodListView.ItemsSource = foodItems;
            return;
        }

        var filtered = foodItems
            .Where(f => f.Name.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase))
            .ToList();

        foodListView.ItemsSource = filtered;
    }

    private async void OnFoodItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selected = e.SelectedItem as FoodItem;
        if (selected == null)
            return;

        string[] gramOptions = new[] { "100", "200", "300", "400", "500" };

        string result = await DisplayActionSheet(
            "�������� ���������� �����",
            "������",
            null,
            gramOptions);

        if (result != null && result != "������" && int.TryParse(result, out int grams))
        {
            var adjusted = new FoodItem
            {
                Name = selected.Name,
                Calories = selected.Calories * grams / 100.0,
                Protein = selected.Protein * grams / 100.0,
                Fats = selected.Fats * grams / 100.0,
                Carbs = selected.Carbs * grams / 100.0
            };

            counting.AddItem(adjusted);
            UpdateTotals();
            await DisplayAlert("���������", $"{adjusted.Name} - {adjusted.Calories:F1} ���� ({grams} �)", "��");
        }

        foodListView.SelectedItem = null;
    }
}
