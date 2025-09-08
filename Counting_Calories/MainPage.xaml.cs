namespace Counting_Calories
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private async void AddBreakfast(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MealPage("Завтрак"));
        }

        private async void AddLunch(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MealPage("Обед"));
        }

        private async void AddDinner(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MealPage("Ужин"));
        }
    }
}
