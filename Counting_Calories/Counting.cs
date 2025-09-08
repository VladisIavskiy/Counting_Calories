using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Counting_Calories
{
    public class Counting
    {
        // Коллекция выбранных продуктов
        public ObservableCollection<FoodItem> SelectedItems { get; } = new();
        public List<FoodItem> Items { get; private set; } = new();


        // Суммы калорий и БЖУ
        public double TotalCalories => CalculateTotal(x => x.Calories);
        public double TotalProtein => CalculateTotal(x => x.Protein);
        public double TotalFats => CalculateTotal(x => x.Fats);
        public double TotalCarbs => CalculateTotal(x => x.Carbs);

        // Добавляем
        public void AddItem(FoodItem item)
        {
            Items.Add(item);
        }

        // Общая логика расчёта
        private double CalculateTotal(Func<FoodItem, double> selector)
            => Items.Sum(selector);

        public void Clear()
        {
            Items.Clear();
        }
    }
}
