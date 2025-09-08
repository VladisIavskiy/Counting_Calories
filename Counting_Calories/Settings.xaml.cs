using Microsoft.Maui.Controls;

namespace Counting_Calories
{
    public partial class SettingsPage : ContentPage
    {
        private int themeIndex = 0; // 0 - Light, 1 - Dark, 2 - Colorful

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void OnThemeButtonClicked(object sender, EventArgs e)
        {
            themeIndex = (themeIndex + 1) % 3;

            switch (themeIndex)
            {
                case 0:
                    App.Current.UserAppTheme = AppTheme.Light; // светлая тема
                    App.ApplyLightTheme();
                    ThemeButton.Text = "Тема: Светлая";
                    break;

                case 1:
                    App.Current.UserAppTheme = AppTheme.Dark;  // темная тема
                    App.ApplyDarkTheme();
                    ThemeButton.Text = "Тема: Тёмная";
                    break;

                case 2:
                    App.Current.UserAppTheme = AppTheme.Unspecified; // кастомная
                    App.ApplyColorfulTheme();
                    ThemeButton.Text = "Тема: Цветная";
                    break;
            }
        }


    }
}
