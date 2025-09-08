using Microsoft.Maui.Storage; // Не забудь добавить

namespace Counting_Calories;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Установим начальную тему
        ApplyColorfulTheme();

        MainPage = new AppShell();
    }

    public static void ApplyLightTheme()
    {
        Application.Current.Resources["PageBackgroundColor"] = Colors.White;
        Application.Current.Resources["SectionBackgroundColor"] = Color.FromArgb("#F8F8F8");
        Application.Current.Resources["TextColor"] = Colors.Black;
        Application.Current.Resources["ButtonBackgroundColor"] = Color.FromArgb("#E0E0E0");
        Application.Current.Resources["ButtonTextColor"] = Colors.Black;
    }

    public static void ApplyDarkTheme()
    {
        Application.Current.Resources["PageBackgroundColor"] = Colors.Black;
        Application.Current.Resources["SectionBackgroundColor"] = Color.FromArgb("#1E1E1E");
        Application.Current.Resources["TextColor"] = Colors.White;
        Application.Current.Resources["ButtonBackgroundColor"] = Color.FromArgb("#333333");
        Application.Current.Resources["ButtonTextColor"] = Colors.White;
    }

    public static void ApplyColorfulTheme()
    {
        Application.Current.Resources["PageBackgroundColor"] = Color.FromArgb("#FFF8DC");
        Application.Current.Resources["SectionBackgroundColor"] = Color.FromArgb("#FFEFD5");
        Application.Current.Resources["TextColor"] = Colors.DarkSlateBlue;
        Application.Current.Resources["ButtonBackgroundColor"] = Color.FromArgb("#40E0D0");
        Application.Current.Resources["ButtonTextColor"] = Colors.DarkBlue;
    }
}
