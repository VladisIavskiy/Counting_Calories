using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Text.Json.Serialization;

namespace Counting_Calories
{
    public class FoodItem
    {
        [JsonPropertyName("ndb_no")]
        public string NDB_No { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("calories")]
        public double Calories { get; set; }

        [JsonPropertyName("protein")]
        public double Protein { get; set; }

        [JsonPropertyName("fats")]
        public double Fats { get; set; }

        [JsonPropertyName("carbs")]
        public double Carbs { get; set; }

        public FoodItem() { }

        public override string ToString() => Name;
    }

    public static class DataLoader
    {
        public static List<FoodItem> LoadFoodItems(string foodPath, string nutPath)
        {
            var foodItems = new List<FoodItem>();
            var foodLines = File.ReadAllLines(foodPath);

            foreach (var line in foodLines)
            {
                var parts = line.Split('^');
                if (parts.Length < 3) continue;

                var id = parts[0].Trim('~');
                var name = parts[2].Trim('~');

                foodItems.Add(new FoodItem { NDB_No = id, Name = name });
            }

            var nutLines = File.ReadAllLines(nutPath);
            foreach (var line in nutLines)
            {
                var parts = line.Split('^');
                if (parts.Length < 3) continue;
                var id = parts[0].Trim('~');
                var nutrientCode = parts[1].Trim('~');

                if (nutrientCode == "208") // 208 — калории
                {
                    if (double.TryParse(parts[2].Trim('~'), NumberStyles.Any, CultureInfo.InvariantCulture, out double kcal))
                    {
                        var item = foodItems.FirstOrDefault(f => f.NDB_No == id);
                        if (item != null)
                        {
                            item.Calories = kcal;
                        }
                    }
                }

                if (nutrientCode == "203") // 203 - белки
                {
                    if (double.TryParse(parts[2].Trim('~'), NumberStyles.Any, CultureInfo.InvariantCulture, out double protein))
                    {
                        var item = foodItems.FirstOrDefault(f => f.NDB_No == id);
                        if (item != null) item.Protein = protein;
                    }
                }

                if (nutrientCode == "204") // 204 - жиры
                {
                    if (double.TryParse(parts[2].Trim('~'), NumberStyles.Any, CultureInfo.InvariantCulture, out double fats))
                    {
                        var item = foodItems.FirstOrDefault(f => f.NDB_No == id);
                        if (item != null) item.Fats = fats;
                    }
                }

                if (nutrientCode == "205") // 205 - углеводы
                {
                    if (double.TryParse(parts[2].Trim('~'), NumberStyles.Any, CultureInfo.InvariantCulture, out double carbohydrates))
                    {
                        var item = foodItems.FirstOrDefault(f => f.NDB_No == id);
                        if (item != null) item.Carbs = carbohydrates;
                    }
                }
            }

            return foodItems;
        }


    }
}

