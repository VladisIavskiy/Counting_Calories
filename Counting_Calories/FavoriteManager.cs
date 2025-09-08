using System.Text.Json;
using Counting_Calories;

public static class FavoritesManager
{
    private static List<FoodItem> favorites;

    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    static FavoritesManager()
    {
        LoadFavorites();
    }

    private static void EnsureLoaded()
    {
        if (favorites == null)
        {
            LoadFavorites();
        }
    }

    public static void Add(FoodItem item)
    {
        EnsureLoaded();
        if (!favorites.Any(f => f.NDB_No == item.NDB_No))
        {
            favorites.Add(item);
            SaveFavorites();
        }
    }

    public static void ClearAll()
    {
        EnsureLoaded();
        favorites.Clear();
        SaveFavorites();
        // Убираем Preferences.Remove("favorites") - не нужно удалять ключ
        System.Diagnostics.Debug.WriteLine($"ClearAll: favorites.Count = {favorites.Count}, prefs after clear: '{DebugGetPrefs()}'");
    }

    public static void Remove(FoodItem item)
    {
        EnsureLoaded();
        favorites.RemoveAll(f => f.NDB_No == item.NDB_No);
        SaveFavorites();
        System.Diagnostics.Debug.WriteLine($"Removed from favorites: {item.Name}");
    }

    public static bool IsFavorite(FoodItem item)
    {
        EnsureLoaded();
        return favorites.Any(f => f.NDB_No == item.NDB_No);
    }

    public static List<FoodItem> GetAll()
    {
        EnsureLoaded();
        return favorites.ToList();
    }

    private static void LoadFavorites()
    {
        var json = Preferences.Get("favorites", null);
        System.Diagnostics.Debug.WriteLine($"LoadFavorites: json = '{json}'");

        if (string.IsNullOrEmpty(json))
        {
            favorites = new List<FoodItem>();
        }
        else
        {
            try
            {
                favorites = JsonSerializer.Deserialize<List<FoodItem>>(json, options) ?? new List<FoodItem>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadFavorites: Ошибка десериализации: {ex.Message}");
                favorites = new List<FoodItem>();
            }
        }
        System.Diagnostics.Debug.WriteLine($"LoadFavorites: favorites.Count = {favorites.Count}");
    }

    private static void SaveFavorites()
    {
        var json = JsonSerializer.Serialize(favorites, options);
        Preferences.Set("favorites", json);
        System.Diagnostics.Debug.WriteLine($"Saved favorites: {favorites.Count} items");
    }

    public static string DebugGetPrefs()
    {
        return Preferences.Get("favorites", "<пусто>");
    }
}
