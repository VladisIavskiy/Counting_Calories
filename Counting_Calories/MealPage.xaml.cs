using System.Text;
using Microsoft.Maui.Controls;

namespace Counting_Calories;

public partial class MealPage : ContentPage
{
    private string mealType; // "Завтрак", "Обед", "Ужин"
    private List<FoodItem> foodItems = new();
    private Counting counting = new();


    public MealPage(string mealType)
    {
        InitializeComponent();
        this.mealType = mealType;
        Title = mealType; // Название страницы

        LoadData();

        favoritesPopup.AddToMeal += FavoritesPopup_AddToMeal;
        Refresh();
    }

    private void FavoritesPopup_AddToMeal(object sender, FoodItem item)
    {
        // Добавляем выбранный элемент из избранного с выбором граммовки
        _ = AddFromFavoritesAsync(item);
    }

    // Обновляет список избранного
    public void Refresh()
    {
        favoritesPopup.Refresh(FavoritesManager.GetAll());
    }

    private void OnCloseFavoritesClicked(object sender, EventArgs e)
    {
        favoritesPopup.IsVisible = false;

        // Обновить список выбранных продуктов, если нужно
        selectedItemsListView.ItemsSource = null;
        selectedItemsListView.ItemsSource = counting.Items;
    }


    private void OnAddFromFavorites(object sender, EventArgs e)
    {
        // Логика добавления из избранного (например, открыть всплывающее окно)
        favoritesPopup.IsVisible = true;
    }



    private async System.Threading.Tasks.Task AddFromFavoritesAsync(FoodItem item)
    {
        string[] grams = { "100", "200", "300", "400", "500" };
        string result = await DisplayActionSheet("Выберите количество", "Отмена", null, grams);

        if (result != null && result != "Отмена" && int.TryParse(result, out int g))
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
            await DisplayAlert("Добавлено", $"{item.Name} ({g} г)", "ОК");
        }
    }

    private void AddToFavorites(FoodItem item)
    {
        if (!FavoritesManager.IsFavorite(item))
        {
            FavoritesManager.Add(item);
            DisplayAlert("Добавлено", $"{item.Name} добавлен в избранное", "ОК");
            Refresh();
        }
        else
        {
            DisplayAlert("Уже в избранном", $"{item.Name} уже есть в избранном", "ОК");
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
        caloriesLabel.Text = $"Ккал: {counting.TotalCalories:F1}";
        proteinLabel.Text = $"Белки: {counting.TotalProtein:F1} г";
        fatsLabel.Text = $"Жиры: {counting.TotalFats:F1} г";
        carbsLabel.Text = $"Углеводы: {counting.TotalCarbs:F1} г";
    }

    private async void OnShowSelectedClicked(object sender, EventArgs e)
    {
        if (!counting.Items.Any())
        {
            await DisplayAlert("Список пуст", "Вы ещё не добавили ни одного продукта.", "ОК");
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
            bool confirm = await DisplayAlert("Удаление", $"Удалить {item.Name} из выбранных?", "Да", "Нет");
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

        await DisplayAlert("Сброшено", "Временные данные очищены.", "ОК");
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (counting.TotalCalories == 0)
        {
            await DisplayAlert("Нет данных", "Добавьте хотя бы один продукт", "ОК");
            return;
        }

        string result = $"Дата: {DateTime.Now:G}\n" +
                        $"Калории: {counting.TotalCalories:F1} ккал\n" +
                        $"Белки: {counting.TotalProtein:F1} г\n" +
                        $"Жиры: {counting.TotalFats:F1} г\n" +
                        $"Углеводы: {counting.TotalCarbs:F1} г\n" +
                        "-----------------------------\n";

        string fileName = Path.Combine(FileSystem.AppDataDirectory, $"{mealType.ToLower()}_log.txt");

        File.AppendAllText(fileName, result);

        await DisplayAlert("Сохранено", "Результат сохранён в файл.", "ОК");
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
            "Выберите количество грамм",
            "Отмена",
            null,
            gramOptions);

        if (result != null && result != "Отмена" && int.TryParse(result, out int grams))
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
            await DisplayAlert("Добавлено", $"{adjusted.Name} - {adjusted.Calories:F1} ккал ({grams} г)", "ОК");
        }

        foodListView.SelectedItem = null;
    }
}
